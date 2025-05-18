using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.Logging;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;

namespace TestMulti
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp()
                .UseLiveCharts()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() 
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windows =>
                    {
                        windows.OnWindowCreated(window =>
                        {
                            // Получение AppWindow для управления размерами и положением
                            var nativeWindow = window as Microsoft.UI.Xaml.Window;
                            var appWindow = nativeWindow.GetAppWindow();

                            if (appWindow != null)
                            {
                                appWindow.Resize(new Windows.Graphics.SizeInt32(400, 800)); // Установка размеров окна
                                appWindow.Move(new Windows.Graphics.PointInt32(100, 100)); // Установка положения окна
                            }
                        });
                    });
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
               

#if DEBUG
                 builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
