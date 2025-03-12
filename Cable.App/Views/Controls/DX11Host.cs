using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

public class DX11Host : HwndHost
{
    private IntPtr _hwnd;
    private readonly int _width;
    private readonly int _height;

    public DX11Host(int w, int h)
    {
        _width = w;
        _height = h;
    }

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        _hwnd = CreateHostWindow(hwndParent.Handle);
        return new HandleRef(this, _hwnd);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        DestroyWindow(hwnd.Handle);
    }

    public IntPtr GetHandle() => _hwnd;

    private IntPtr CreateHostWindow(IntPtr parentHwnd)
    {
        const int WS_CHILD = 0x40000000;
        const int WS_VISIBLE = 0x10000000;

        return CreateWindowEx(0, "STATIC", "", WS_CHILD | WS_VISIBLE,
                              0, 0, _width, _height, parentHwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr CreateWindowEx(int exStyle, string className, string windowName,
                                                int style, int x, int y, int width, int height,
                                                IntPtr parent, IntPtr menu, IntPtr instance, IntPtr param);

    [DllImport("user32.dll")]
    private static extern bool DestroyWindow(IntPtr hWnd);
}
