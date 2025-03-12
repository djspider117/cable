#include "CableRenderer.h"
#include <directxmath.h>
#include <vector>
#include <d3dcompiler.h>

using namespace System::Numerics;
using namespace DirectX;

inline void throw_if_failed(HRESULT hr, System::String^ msg)
{
	if (FAILED(hr))
	{
		System::Exception^ ex = System::Runtime::InteropServices::Marshal::GetExceptionForHR(hr);
		throw gcnew System::Exception(msg, ex);
	}
}

struct Vertex
{
	XMFLOAT3 Position; // 3D position (x, y, z)
	XMFLOAT4 Color;    // RGBA color (r, g, b, a)

	// Optional: Add other vertex attributes like texture coordinates, normals, etc.
	// Example:
	// DirectX::XMFLOAT2 TexCoord; // Texture coordinates (u, v)
};

namespace Cable {
	namespace Renderer {

		CableRendererImpl::CableRendererImpl(UINT width, UINT height, HWND hwnd) :
			_width(width),
			_height(height)
		{
			InitializeDirect2D(hwnd);
		}
		CableRendererImpl::~CableRendererImpl()
		{
			if (_sharedHandle)
			{
				CloseHandle(_sharedHandle);
			}
		}

		void CableRendererImpl::InitializeDirect2D(HWND hwnd)
		{
			// Create the Direct3D 11 device
			D3D_FEATURE_LEVEL featureLevels[] = { D3D_FEATURE_LEVEL_11_0 };
			HRESULT hr = D3D11CreateDevice(
				nullptr,
				D3D_DRIVER_TYPE_HARDWARE,
				nullptr,
				0,
				featureLevels,
				1,
				D3D11_SDK_VERSION,
				&_device,
				nullptr,
				&_deviceContext
			);
			throw_if_failed(hr, "Failed to create Direct3D 11 device");

			// Create a shared texture
			D3D11_TEXTURE2D_DESC textureDesc = {};
			textureDesc.Width = _width;
			textureDesc.Height = _height;
			textureDesc.MipLevels = 1;
			textureDesc.ArraySize = 1;
			textureDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
			textureDesc.SampleDesc.Count = 1;
			textureDesc.Usage = D3D11_USAGE_DEFAULT;
			textureDesc.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;
			textureDesc.MiscFlags = D3D11_RESOURCE_MISC_SHARED;

			hr = _device->CreateTexture2D(&textureDesc, nullptr, &_sharedTexture);
			throw_if_failed(hr, "Failed to create shared texture");

			// Create a shared handle
			CComPtr<IDXGIResource> dxgiResource;
			hr = _sharedTexture->QueryInterface(__uuidof(IDXGIResource), reinterpret_cast<void**>(&dxgiResource));
			throw_if_failed(hr, "Failed to get DXGI resource");

			hr = dxgiResource->GetSharedHandle(&_sharedHandle);
			throw_if_failed(hr, "Failed to create shared handle");

			// Create the Direct2D factory
			hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &_d2dFactory);
			throw_if_failed(hr, "Failed to create Direct2D factory");

			// Create a DXGI surface from the shared texture
			CComPtr<IDXGISurface> dxgiSurface;
			hr = _sharedTexture->QueryInterface(__uuidof(IDXGISurface), reinterpret_cast<void**>(&dxgiSurface));
			throw_if_failed(hr, "Failed to get DXGI surface");

			// Create the Direct2D render target
			D2D1_RENDER_TARGET_PROPERTIES rtProps = D2D1::RenderTargetProperties();
			hr = _d2dFactory->CreateDxgiSurfaceRenderTarget(dxgiSurface, rtProps, &_renderTarget);
			throw_if_failed(hr, "Failed to create Direct2D render target");

			hr = dxgiResource->GetSharedHandle(&_sharedHandle);
			throw_if_failed(hr, "Failed to create shared handle");
			// Create a white brush
			hr = _renderTarget->CreateSolidColorBrush(D2D1::ColorF(D2D1::ColorF::White), &_whiteBrush);
			throw_if_failed(hr, "Failed to create brush");
		}

		void CableRendererImpl::PushCommandList(RenderCommandList^ commandList)
		{
			_renderQueue.push(nativePtr);
		}

		void CableRendererImpl::Render()
		{
			RenderCommandList* commandList = _renderQueue.front();;
			if (commandList == nullptr)
				return;

			_renderQueue.pop();

			// Begin drawing
			_renderTarget->BeginDraw();
			_renderTarget->Clear(D2D1::ColorF(D2D1::ColorF::Black)); // Clear to black

			// Render each command in the command list
			for each (IRenderCommand ^ command in commandList->Commands)
			{
				if (command->GetType() == Mesh2DRenderCommand::typeid)
				{
					Mesh2DRenderCommand^ meshCommand = (Mesh2DRenderCommand^)command;

					Mesh2D^ mesh = meshCommand->Mesh;

					// Extract geometry data
					Geometry2D^ geometry = mesh->Geometry;
					array<Vector2>^ vertices = geometry->Vertices;
					array<int>^ indices = geometry->Indices;

					// Create a Direct2D path geometry
					CComPtr<ID2D1PathGeometry> pathGeometry;
					HRESULT hr = _d2dFactory->CreatePathGeometry(&pathGeometry);
					throw_if_failed(hr, "Failed to create path geometry");

					// Write geometry data to the path geometry
					CComPtr<ID2D1GeometrySink> sink;
					hr = pathGeometry->Open(&sink);
					throw_if_failed(hr, "Failed to open geometry sink");

					// Begin the figure
					sink->BeginFigure(
						D2D1::Point2F(vertices[indices[0]].X, vertices[indices[0]].Y),
						D2D1_FIGURE_BEGIN_FILLED
					);

					// Add lines for each index
					for (int i = 1; i < indices->Length; i++)
					{
						sink->AddLine(D2D1::Point2F(vertices[indices[i]].X, vertices[indices[i]].Y));
					}

					// End the figure
					sink->EndFigure(D2D1_FIGURE_END_CLOSED);
					sink->Close();

					// Draw the geometry
					_renderTarget->DrawGeometry(pathGeometry, _whiteBrush, 1.0f);
				}
			}

			// End drawing
			HRESULT hr = _renderTarget->EndDraw();
			throw_if_failed(hr, "Failed to end drawing");
		}
	}
}

