using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DaliyUtils
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            HWND hwnd = (HWND)WinRT.Interop.WindowNative.GetWindowHandle(this);
            SetWindowSize(hwnd, 1050, 800);
            PlacementCenterWindowInMonitorWin32(hwnd);
        }

        private void SetWindowSize(HWND hwnd, int width, int height)
        {
            // Win32 uses pixels and WinUI 3 uses effective pixels, so you should apply the DPI scale factor
            uint dpi = GetDpiForWindow(hwnd);
            float scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            SetWindowPos(hwnd, default, 0, 0, width, height, SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
        }

        private void PlacementCenterWindowInMonitorWin32(HWND hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);
            ClipOrCenterRectToMonitorWin32(ref rc);
            SetWindowPos(hwnd, default, rc.left, rc.top, 0, 0,
                         SET_WINDOW_POS_FLAGS.SWP_NOSIZE |
                         SET_WINDOW_POS_FLAGS.SWP_NOZORDER |
                         SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
        }

        private void ClipOrCenterRectToMonitorWin32(ref RECT prc)
        {
            HMONITOR hMonitor;
            RECT rc;
            int w = prc.right - prc.left;
            int h = prc.bottom - prc.top;

            hMonitor = MonitorFromRect(prc, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);
            MONITORINFO mi = new MONITORINFO();
            mi.cbSize = (uint)Marshal.SizeOf<MONITORINFO>();

            GetMonitorInfo(hMonitor, ref mi);

            rc = mi.rcWork;
            prc.left = rc.left + (rc.right - rc.left - w) / 2;
            prc.top = rc.top + (rc.bottom - rc.top - h) / 2;
            prc.right = prc.left + w;
            prc.bottom = prc.top + h;
        }
    }
}
