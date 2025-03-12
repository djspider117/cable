using Cable.Data.Types;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Direct3D9;
using SharpDX.DXGI;
using System.Windows;
using System.Windows.Interop;
using D3D9Device = SharpDX.Direct3D9.Device;
using D9Format = SharpDX.Direct3D9.Format;
using D9PresentParameters = SharpDX.Direct3D9.PresentParameters;
using D9Usage = SharpDX.Direct3D9.Usage;
using Device = SharpDX.Direct3D11.Device;
using Format = SharpDX.DXGI.Format;
using PresentFlags = SharpDX.DXGI.PresentFlags;
using Resource = SharpDX.DXGI.Resource;
using SwapChain = SharpDX.DXGI.SwapChain;
using SwapEffect = SharpDX.DXGI.SwapEffect;
using Usage = SharpDX.DXGI.Usage;

namespace Cable.Renderer.SharpDX;

public class CableRenderer
{
    private Device _device;
    private SwapChain _swapChain;
    private Texture2D _backBuffer;
    private RenderTargetView renderView;
    private int _width;
    private int _height;
    private D3DImage _d3dImage;
    private D3D9Device _d3d9Device;
    private Texture d3d9Texture;

    public CableRenderer()
	{

	}

    public void InitializeSharpDX(int width, int height, IntPtr windowHandle, D3DImage d3dImage)
    {
        _width = width;
        _height = height;
        _d3dImage = d3dImage;

        var swapChainDesc = new SwapChainDescription
        {
            BufferCount = 1,
            ModeDescription = new ModeDescription(width, height, new Rational(60, 1), Format.B8G8R8A8_UNorm),
            Usage = Usage.RenderTargetOutput,
            OutputHandle = windowHandle,
            SampleDescription = new SampleDescription(1, 0),
            SwapEffect = SwapEffect.Discard,
            IsWindowed = true
        };

        Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDesc, out _device, out _swapChain);
        _backBuffer = _swapChain.GetBackBuffer<Texture2D>(0);
        renderView = new RenderTargetView(_device, _backBuffer);


        // Initialize Direct3D9Ex device
        var d3d9 = new Direct3DEx();
        _d3d9Device = new D3D9Device(d3d9, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve, new D9PresentParameters());

        // Create shared texture
        using (var dxgiResource = _backBuffer.QueryInterface<Resource>())
        {
            var handle = dxgiResource.SharedHandle;
            d3d9Texture = new Texture(_d3d9Device, width, height, 1, D9Usage.RenderTarget, D9Format.A8R8G8B8, Pool.Default, ref handle);
        }

        d3dImage.Lock();
        d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, d3d9Texture.GetSurfaceLevel(0).NativePointer);
        d3dImage.Unlock();
    }

    public void Render(RenderCommandList renderCommands)
    {
        _device.ImmediateContext.ClearRenderTargetView(renderView, new Color4(0, 0.5f, 1, 1));
        _swapChain.Present(1, PresentFlags.None);

        _d3dImage.Lock();
        _d3dImage.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
        _d3dImage.Unlock();
    }
}
