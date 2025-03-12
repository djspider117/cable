#pragma once
#include "Windows.h"
#include "DataTypes.h"

#include <atlbase.h>
#include <d2d1.h>
#include <dxgi1_6.h>
#include <d3d11.h>
#include <dxgi1_2.h>
#include <queue>


#pragma comment(lib, "d2d1.lib")
#pragma comment(lib, "d3d11.lib") // Direct3D 11
#pragma comment(lib, "dxgi.lib")  // DXGI (DirectX Graphics Infrastructure)

using namespace System::Collections::Generic;
using namespace Cable::Data;
using namespace Cable::Data::Types;
using namespace Cable::Data::Types::Native;
using namespace System::Numerics;

namespace Cable::Renderer {

	class CableRendererImpl
	{
	public:
		CableRendererImpl(UINT width, UINT height, HWND hwnd);
		~CableRendererImpl();

		void PushCommandList(RenderCommandList^ commandList);
		void Render();

		inline HANDLE GetSharedHandle() const { return _sharedHandle; }
	private:
		UINT _width;
		UINT _height;
		HANDLE _sharedHandle;
		CComPtr<ID3D11Device> _device;
		CComPtr<ID3D11DeviceContext> _deviceContext;
		CComPtr<ID3D11Texture2D> _sharedTexture;
		CComPtr<ID2D1Factory> _d2dFactory;          // Direct2D factory
		CComPtr<ID2D1RenderTarget> _renderTarget; // Render target
		CComPtr<ID2D1SolidColorBrush> _whiteBrush;    // Brush for drawing

		std::queue<RenderCommand> _renderQueue;

		void InitializeDirect2D(HWND hwnd);
	};


	public ref class CableRenderer
	{
	public:
		CableRenderer(UINT width, UINT height, System::IntPtr hwnd);
		~CableRenderer();

		void PushCommandList(RenderCommandList^ commandList);
		void Render();

	private:
		CableRendererImpl* _impl;
	};
}

