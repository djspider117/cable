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
			InitializeDX11(hwnd);
		}
		CableRendererImpl::~CableRendererImpl()
		{
			if (_sharedHandle)
			{
				CloseHandle(_sharedHandle);
			}
		}

		void CableRendererImpl::InitializeDX11(HWND hwnd)
		{
			DXGI_SWAP_CHAIN_DESC swapDesc = {};
			swapDesc.BufferCount = 1;
			swapDesc.BufferDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
			swapDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
			swapDesc.OutputWindow = hwnd;
			swapDesc.SampleDesc.Count = 1;
			swapDesc.Windowed = TRUE;
			swapDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;

			throw_if_failed(
				D3D11CreateDeviceAndSwapChain(
					nullptr, 
					D3D_DRIVER_TYPE_HARDWARE, 
					nullptr, 
					0,
					nullptr, 
					0, 
					D3D11_SDK_VERSION, 
					&swapDesc,
					&_swapChain, 
					&_device, 
					nullptr, 
					&_deviceContext),
				"Failed to create device and swapchain");

			ID3D11Texture2D* backBuffer;
			_swapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (void**)&backBuffer);
			_device->CreateRenderTargetView(backBuffer, nullptr, &_renderTargetView);
			backBuffer->Release();

			InitializeShaders();
		}
		void CableRendererImpl::InitializeShaders()
		{
			// Compile the vertex shader
			CComPtr<ID3DBlob> vsBlob;
			HRESULT hr = D3DCompileFromFile(
				L"VertexShader.hlsl", // Path to the vertex shader file
				nullptr,              // No defines
				nullptr,              // No includes
				"main",               // Entry point
				"vs_5_0",             // Shader model (Vertex Shader 5.0)
				0,                    // Compile flags
				0,                    // Effect flags
				&vsBlob,              // Output blob
				nullptr               // Error blob (optional)
			);

			if (FAILED(hr))
			{
				throw std::exception("Failed to compile vertex shader");
			}

			// Create the vertex shader
			hr = _device->CreateVertexShader(vsBlob->GetBufferPointer(), vsBlob->GetBufferSize(), nullptr, &_vertexShader);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create vertex shader");
			}

			// Define the input layout
			D3D11_INPUT_ELEMENT_DESC layout[] =
			{
				{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
				{ "COLOR", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 }
			};

			// Create the input layout
			hr = _device->CreateInputLayout(layout, 2, vsBlob->GetBufferPointer(), vsBlob->GetBufferSize(), &_inputLayout);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create input layout");
			}

			// Compile the pixel shader
			CComPtr<ID3DBlob> psBlob;
			hr = D3DCompileFromFile(
				L"PixelShader.hlsl", // Path to the pixel shader file
				nullptr,             // No defines
				nullptr,             // No includes
				"main",              // Entry point
				"ps_5_0",            // Shader model (Pixel Shader 5.0)
				0,                   // Compile flags
				0,                   // Effect flags
				&psBlob,             // Output blob
				nullptr              // Error blob (optional)
			);

			if (FAILED(hr))
			{
				throw std::exception("Failed to compile pixel shader");
			}

			// Create the pixel shader
			hr = _device->CreatePixelShader(psBlob->GetBufferPointer(), psBlob->GetBufferSize(), nullptr, &_pixelShader);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create pixel shader");
			}
		}
		void CableRendererImpl::InitializeConstantBuffer()
		{
			D3D11_BUFFER_DESC cbDesc = {};
			cbDesc.ByteWidth = sizeof(XMMATRIX);
			cbDesc.Usage = D3D11_USAGE_DEFAULT;
			cbDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;

			HRESULT hr = _device->CreateBuffer(&cbDesc, nullptr, &_constantBuffer);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create constant buffer");
			}
		}

		void CableRendererImpl::Render(RenderCommandList^ commandList)
		{
			const float clearColor[] = { 0.0f, 0.2f, 0.0f, 1.0f };
			_deviceContext->ClearRenderTargetView(_renderTargetView, clearColor);

			// Set primitive topology (e.g., triangle list)
			_deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

			 for each (IRenderCommand ^ command in commandList->Commands)
			 {
			     if (command->GetType() == Mesh2DRenderCommand::typeid)
			     {
			         Mesh2DRenderCommand^ meshCommand = (Mesh2DRenderCommand^)command;
					 RenderMesh(meshCommand->Mesh, meshCommand->Camera);
			     }
			 }

			// Present or flush the context (if using a swap chain)
			_swapChain->Present(1, 0);
		}
		void CableRendererImpl::RenderMesh(Mesh2D^ mesh, Camera2D^ camera)
		{
			// Extract geometry data
			Geometry2D^ geometry = mesh->Geometry;
			array<Vector2>^ vertices = geometry->Vertices;
			array<int>^ indices = geometry->Indices;

			// Create vertex and index buffers for this mesh
			CComPtr<ID3D11Buffer> vertexBuffer;
			CComPtr<ID3D11Buffer> indexBuffer;
			CreateBuffers(vertices, indices, &vertexBuffer, &indexBuffer);

			// Set the vertex and index buffers
			UINT stride = sizeof(Vertex); // Size of one vertex
			UINT offset = 0;
			_deviceContext->IASetVertexBuffers(0, 1, &vertexBuffer.p, &stride, &offset);
			_deviceContext->IASetIndexBuffer(indexBuffer, DXGI_FORMAT_R32_UINT, 0);

			// Apply the transform and camera (you'll need to implement this)
			ApplyTransformAndCamera(mesh->Transform, camera);

			// Draw the mesh
			_deviceContext->DrawIndexed(indices->Length, 0, 0);
		}

		void CableRendererImpl::CreateBuffers(array<Vector2>^ vertices, array<int>^ indices, ID3D11Buffer** vertexBuffer, ID3D11Buffer** indexBuffer)
		{
			// Convert vertices to DirectX format
			std::vector<Vertex> dxVertices;
			for each (Vector2 vertex in vertices)
			{
				dxVertices.push_back({ XMFLOAT3(vertex.X, vertex.Y, 0.0f), XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f) }); // White color
			}

			// Create the vertex buffer
			D3D11_BUFFER_DESC vertexBufferDesc = {};
			vertexBufferDesc.Usage = D3D11_USAGE_DEFAULT;
			vertexBufferDesc.ByteWidth = sizeof(Vertex) * dxVertices.size();
			vertexBufferDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;

			D3D11_SUBRESOURCE_DATA vertexInitData = {};
			vertexInitData.pSysMem = dxVertices.data();

			HRESULT hr = _device->CreateBuffer(&vertexBufferDesc, &vertexInitData, vertexBuffer);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create vertex buffer");
			}

			// Create the index buffer
			D3D11_BUFFER_DESC indexBufferDesc = {};
			indexBufferDesc.Usage = D3D11_USAGE_DEFAULT;
			indexBufferDesc.ByteWidth = sizeof(int) * indices->Length;
			indexBufferDesc.BindFlags = D3D11_BIND_INDEX_BUFFER;

			D3D11_SUBRESOURCE_DATA indexInitData = {};
			pin_ptr<int> pinnedIndices = &indices[0]; // Pin the managed array
			indexInitData.pSysMem = pinnedIndices;    // Assign the pinned pointer

			hr = _device->CreateBuffer(&indexBufferDesc, &indexInitData, indexBuffer);
			if (FAILED(hr))
			{
				throw std::exception("Failed to create index buffer");
			}
		}

		void CableRendererImpl::ApplyTransformAndCamera(Matrix3x2 transform, Camera2D^ camera)
		{
			// Convert the transform and camera to a DirectX-compatible matrix
			// (You'll need to implement this based on your camera and transform logic)
			XMMATRIX worldMatrix = XMMatrixIdentity(); // Replace with actual transform
			XMMATRIX viewMatrix = XMMatrixIdentity();  // Replace with camera view
			XMMATRIX projectionMatrix = XMMatrixOrthographicOffCenterLH(0, _width, _height, 0, 0, 1); // 2D projection

			// Combine matrices and set them in the vertex shader
			XMMATRIX wvpMatrix = worldMatrix * viewMatrix * projectionMatrix;
			_deviceContext->UpdateSubresource(_constantBuffer, 0, nullptr, &wvpMatrix, 0, 0);
			_deviceContext->VSSetConstantBuffers(0, 1, &_constantBuffer.p);
		}
	}
}

