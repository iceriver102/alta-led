using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Alta_LED
{
    partial class MainWindow : Window
    {
       
        const int MONITOR_DEFAULTTONEAREST = 2;
        // To get a handle to the specified monitor
        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        // Monitor information (used by GetMonitorInfo())
        [StructLayout(LayoutKind.Sequential)]
        public class MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor; // Total area
            public RECT rcWork; // Working area
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public char[] szDevice;
        }
        // To get the working area of the specified monitor
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX monitorInfo);

        void Window1_SourceInitialized(object sender, EventArgs e)
        {
            // Make window borderless
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            // Get handle for nearest monitor to this window
            WindowInteropHelper wih = new WindowInteropHelper(this);
            IntPtr hMonitor = MonitorFromWindow(wih.Handle, MONITOR_DEFAULTTONEAREST);
            // Get monitor info
            MONITORINFOEX monitorInfo = new MONITORINFOEX();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(new HandleRef(this, hMonitor), monitorInfo);
            // Create working area dimensions, converted to DPI-independent values
            HwndSource source = HwndSource.FromHwnd(wih.Handle);
            if (source == null) return; // Should never be null
            if (source.CompositionTarget == null) return; // Should never be null
            Matrix matrix = source.CompositionTarget.TransformFromDevice;
            RECT workingArea = monitorInfo.rcWork;
            Point dpiIndependentSize =
            matrix.Transform(
            new Point(
            workingArea.Right - workingArea.Left,
            workingArea.Bottom - workingArea.Top));
            this.Top = 0;
            this.Left = 0;
            this.MaxWidth = dpiIndependentSize.X;
            this.MaxHeight = dpiIndependentSize.Y;
            this.WindowState = WindowState.Maximized;
        }
    }
}
