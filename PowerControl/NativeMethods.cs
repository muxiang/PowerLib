using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using static PowerControl.NativeStructures;

namespace PowerControl
{
    public class NativeMethods
    {
        #region USER32.dll

        [DllImport("USER32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        
        [DllImport("USER32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("USER32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("USER32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("USER32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("USER32.dll", EntryPoint = "GetCaretBlinkTime")]
        public static extern int GetCaretBlinkTime();

        [DllImport("USER32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

        [DllImport("USER32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

        [DllImport("USER32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("USER32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("USER32.dll", SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
        
        [DllImport("USER32.dll", SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("USER32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("USER32.dll", SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("USER32.dll")]
        public static extern short GetKeyState(NativeConstants.VK vk);

        [DllImport("USER32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, NativeConstants.KeyModifiers fsModifiers, Keys vk);

        [DllImport("USER32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("USER32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("USER32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);

        [DllImport("USER32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);

        [DllImport("USER32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(
            IntPtr hwnd,
            IntPtr hdcDst,
            ref POINT pptDst,
            ref SIZE psize,
            IntPtr hdcSrc,
            ref POINT pptSrc,
            uint crKey,
            [In] ref BLENDFUNCTION pblend,
            uint dwFlags);

        [DllImport("USER32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("USER32.dll")]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        #endregion USER32.dll

        #region GDI32.dll

        [DllImport("GDI32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport("GDI32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        
        [DllImport("GDI32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);

        [DllImport("GDI32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        #endregion GDI32.dll

        #region DWMAPI.dll

        [DllImport("DWMAPI.dll")]
        public static extern int DwmIsCompositionEnabled(out bool isEnabled);

        #endregion DWMAPI.dll
    }
}
