#include "CableRenderer.h"

namespace Cable {
	namespace Renderer {

		CableRenderer::CableRenderer(UINT width, UINT height, System::IntPtr hwnd)
		{
			HWND nativeHwnd = static_cast<HWND>(hwnd.ToPointer());
			_impl = new CableRendererImpl(width, height, nativeHwnd);
		}
		CableRenderer::~CableRenderer()
		{
			delete _impl;
		}
		void CableRenderer::Render(RenderCommandList^ commandList)
		{
			_impl->Render(commandList);
		}
	}
}

