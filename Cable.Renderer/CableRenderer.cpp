#include "CableRenderer.h"

namespace Cable {
	namespace Renderer {

		CableRenderer::CableRenderer(UINT width, UINT height)
		{
			_impl = new CableRendererImpl(width, height);
		}
		CableRenderer::~CableRenderer()
		{
			delete _impl;
		}
		void CableRenderer::Render(RenderCommandList^ commandList)
		{
			_impl->Render(commandList);
		}
		void CableRenderer::CreateSharedTexture()
		{
			_impl->CreateSharedTexture();
		}
		HANDLE CableRenderer::GetSharedHandle()
		{
			return _impl->GetSharedHandle();
		}
	}
}

