#include "CableRenderer.h"

namespace Cable {
	namespace Renderer {

		CableRendererImpl::CableRendererImpl(UINT width, UINT height) :
			_width(width),
			_height(height)
		{
			InitializeDX12();
			CreateSharedTexture();
		}
		CableRendererImpl::~CableRendererImpl()
		{
		}

		void CableRendererImpl::CreateSharedTexture()
		{
			// Create a shared texture
			D3D12_RESOURCE_DESC textureDesc = {};
			textureDesc.Dimension = D3D12_RESOURCE_DIMENSION_TEXTURE2D;
			textureDesc.Width = _width;
			textureDesc.Height = _height;
			textureDesc.DepthOrArraySize = 1;
			textureDesc.MipLevels = 1;
			textureDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
			textureDesc.SampleDesc.Count = 1;
			textureDesc.Flags = D3D12_RESOURCE_FLAG_ALLOW_RENDER_TARGET;

			D3D12_HEAP_PROPERTIES heapProps = {};
			heapProps.Type = D3D12_HEAP_TYPE_DEFAULT;

			// Create the shared texture
			HRESULT hr = _device->CreateCommittedResource(
				&heapProps,
				D3D12_HEAP_FLAG_SHARED, // Enable sharing
				&textureDesc,
				D3D12_RESOURCE_STATE_RENDER_TARGET,
				nullptr,
				IID_PPV_ARGS(&_sharedTexture)
			);

			if (FAILED(hr))
			{
				throw gcnew System::Exception("Failed to create shared texture");
			}

			// Create a shared handle for the texture
			hr = _device->CreateSharedHandle(
				_sharedTexture,
				nullptr, // Default security attributes
				GENERIC_ALL, // Access rights
				nullptr, // Name (optional)
				&_sharedHandle
			);

			if (FAILED(hr))
			{
				throw gcnew System::Exception("Failed to create shared handle");
			}
		}

		HANDLE CableRendererImpl::GetSharedHandle()
		{
			return _sharedHandle;
		}

		void CableRendererImpl::InitializeDX12()
		{
			// Create the DX12 device
			CComPtr<ID3D12Device> device;
			D3D12CreateDevice(nullptr, D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&device));
			_device = device;

			// Create the command queue
			D3D12_COMMAND_QUEUE_DESC queueDesc = {};
			queueDesc.Type = D3D12_COMMAND_LIST_TYPE_DIRECT;
			device->CreateCommandQueue(&queueDesc, IID_PPV_ARGS(&_commandQueue));

			// Create the swap chain
			DXGI_SWAP_CHAIN_DESC1 swapChainDesc = {};
			swapChainDesc.BufferCount = 2;
			swapChainDesc.Width = _width;
			swapChainDesc.Height = _height;
			swapChainDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
			swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
			swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD;
			swapChainDesc.SampleDesc.Count = 1;

			// Create the command allocator and command list
			device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&_commandAllocator));
			device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, _commandAllocator, nullptr, IID_PPV_ARGS(&_commandList));

			// Create the RTV heap
			D3D12_DESCRIPTOR_HEAP_DESC rtvHeapDesc = {};
			rtvHeapDesc.NumDescriptors = 2;
			rtvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
			device->CreateDescriptorHeap(&rtvHeapDesc, IID_PPV_ARGS(&_rtvHeap));

			// Create a fence for synchronization
			device->CreateFence(0, D3D12_FENCE_FLAG_NONE, IID_PPV_ARGS(&_fence));
			_fenceEvent = CreateEvent(nullptr, FALSE, FALSE, nullptr);
		}

		void CableRendererImpl::Render(RenderCommandList^ commandList)
		{
			// Reset the command allocator and command list
			_commandAllocator->Reset();
			_commandList->Reset(_commandAllocator, nullptr);

			// Set the render target (shared texture)
			D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle = _rtvHeap->GetCPUDescriptorHandleForHeapStart();
			_commandList->OMSetRenderTargets(1, &rtvHandle, FALSE, nullptr);

			// Clear the render target
			const float clearColor[] = { 0.0f, 0.2f, 0.4f, 1.0f };
			_commandList->ClearRenderTargetView(rtvHandle, clearColor, 0, nullptr);


			//TODO actually render
			//for each (IRenderCommand ^ command in commandList->Commands)
			//{
			//    if (command->GetType() == Mesh2DRenderCommand::typeid)
			//    {
			//        Mesh2DRenderCommand^ meshCommand = (Mesh2DRenderCommand^)command;
			//        // Bind vertex/index buffers, set pipeline state, and issue draw calls
			//    }
			//}

			_commandList->Close();
			ID3D12CommandList* ppCommandLists[] = { _commandList };
			_commandQueue->ExecuteCommandLists(_countof(ppCommandLists), ppCommandLists);

			// Signal the fence
			const UINT64 fenceValue = _fenceValue;
			_commandQueue->Signal(_fence, fenceValue);
			_fenceValue++;

			// Wait for the GPU to finish
			if (_fence->GetCompletedValue() < fenceValue)
			{
				_fence->SetEventOnCompletion(fenceValue, _fenceEvent);
				WaitForSingleObject(_fenceEvent, INFINITE);
			}
		}

		void CableRendererImpl::WaitForPreviousFrame()
		{
			const UINT64 fence = _fenceValue;
			_commandQueue->Signal(_fence, fence);
			_fenceValue++;

			// Wait until the previous frame is finished
			if (_fence->GetCompletedValue() < fence)
			{
				_fence->SetEventOnCompletion(fence, _fenceEvent);
				WaitForSingleObject(_fenceEvent, INFINITE);
			}
		}
	}
}

