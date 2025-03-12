#pragma once
#include "Windows.h"
#include <d3d12.h>
#include <dxgi1_6.h>
#include <atlbase.h>

#include <d3d11.h>
#include <dxgi1_2.h>

#pragma comment(lib, "d3d12.lib")
#pragma comment(lib, "dxgi.lib")
#pragma comment(lib, "d3d11.lib") // Direct3D 11
#pragma comment(lib, "dxgi.lib")  // DXGI (DirectX Graphics Infrastructure)
#pragma comment(lib, "d3dcompiler.lib") // HLSL Shader Compiler (optional, if you use shaders)

using namespace Cable::Data;
using namespace Cable::Data::Types;
using namespace System::Numerics;

namespace Cable {
	namespace Renderer {

		class CableRendererImpl
		{
		public:
			CableRendererImpl(UINT width, UINT height, HWND hwnd);
			~CableRendererImpl();

			void Render(RenderCommandList^ commandList);

			void RenderMesh(Mesh2D^ mesh, Camera2D^ camera);
			void CreateBuffers(array<Vector2>^ vertices, array<int>^ indices, ID3D11Buffer** vertexBuffer, ID3D11Buffer** indexBuffer);

			void ApplyTransformAndCamera(Matrix3x2 transform, Camera2D^ camera);

		private:
			UINT _width;
			UINT _height;
			CComPtr<ID3D11Device> _device;
			CComPtr<ID3D11DeviceContext> _deviceContext;
			CComPtr<IDXGISwapChain> _swapChain;
			CComPtr<ID3D11RenderTargetView> _renderTargetView;
			CComPtr<ID3D11InputLayout> _inputLayout;  // Input layout for vertex data
			CComPtr<ID3D11VertexShader> _vertexShader; // Vertex shader
			CComPtr<ID3D11PixelShader> _pixelShader;   // Pixel shader
			CComPtr<ID3D11Buffer> _constantBuffer;

			HANDLE _sharedHandle;

			void InitializeDX11(HWND hwnd);
			void InitializeShaders();
			void InitializeConstantBuffer();
		};


		public ref class CableRenderer
		{
		public:
			CableRenderer(UINT width, UINT height, System::IntPtr hwnd);
			~CableRenderer();

			void Render(RenderCommandList^ commandList);

		private:
			CableRendererImpl* _impl;
		};
	}
}

