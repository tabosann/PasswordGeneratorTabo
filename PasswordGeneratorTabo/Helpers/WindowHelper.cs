using Microsoft.UI.Xaml;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Graphics;
using WinRT.Interop;

namespace PasswordGeneratorTabo.Helpers
{
    internal static class WindowHelper
    {
        private static double GetWindowDpiScale(Window window)
        {
            return window.Content.XamlRoot.RasterizationScale;
        }

        private static void ResizeClinetWithDpiScale(Window window, double width, double height)
        {
            var dpiScale = GetWindowDpiScale(window);
            window.AppWindow.ResizeClient(new SizeInt32(
                (int)(width * dpiScale),
                (int)(height * dpiScale)
            ));
        }

        public static void FitClientToActualSize(Window window, FrameworkElement rootGrid)
        {
            rootGrid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            ResizeClinetWithDpiScale(window, rootGrid.DesiredSize.Width, rootGrid.DesiredSize.Height);
        }
    }
}
