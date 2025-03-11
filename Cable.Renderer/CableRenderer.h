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

namespace Cable {
	namespace Renderer {

		class CableRendererImpl
		{
		public:
			CableRendererImpl(UINT width, UINT height);
			~CableRendererImpl();

			void Render(RenderCommandList^ commandList);

			void CreateSharedTexture();
			HANDLE GetSharedHandle();

		private:
			UINT _width;
			UINT _height;
			CComPtr<ID3D12Device> _device;
			CComPtr<ID3D12CommandQueue> _commandQueue;
			CComPtr<ID3D12CommandAllocator> _commandAllocator;
			CComPtr<ID3D12GraphicsCommandList> _commandList;

			CComPtr<ID3D12Resource> _sharedTexture;
			CComPtr<IDXGIResource> _dxgiResource;
			HANDLE _sharedHandle;

			CComPtr<ID3D12DescriptorHeap> _rtvHeap;
			CComPtr<ID3D12Resource> _renderTargets[2]; // Double buffering
			CComPtr<ID3D12Fence> _fence;

			HANDLE _fenceEvent;
			UINT64 _fenceValue;
			int _frameIndex;


			void InitializeDX12();
			void WaitForPreviousFrame();
		};


		public ref class CableRenderer
		{
		public:
			CableRenderer(UINT width, UINT height);
			~CableRenderer();

			void Render(RenderCommandList^ commandList);
			void CreateSharedTexture();
			HANDLE GetSharedHandle();

		private:
			CableRendererImpl* _impl;
		};
	}
}

