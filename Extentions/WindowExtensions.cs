#if WINDOWS
using Microsoft.UI.Windowing; // Для работы с AppWindow
using Microsoft.UI.Xaml; // Для работы с Window
public static class WindowExtensions
{
    public static AppWindow GetAppWindow(this Microsoft.UI.Xaml.Window window)
    {
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        return AppWindow.GetFromWindowId(windowId);
    }
}
#endif