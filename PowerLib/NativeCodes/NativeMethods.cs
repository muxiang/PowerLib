using System;
using System.Runtime.InteropServices;
using System.Text;

using static PowerLib.NativeCodes.NativeConstants;
using static PowerLib.NativeCodes.NativeStructures;

namespace PowerLib.NativeCodes;

public static class NativeMethods
{
    #region USER32.dll

    [DllImport("USER32.dll")]
    public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("USER32.dll", CharSet = CharSet.Auto)]
    public static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("USER32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool PostMessage(IntPtr hWnd, uint wMsg, int wParam, int lParam);

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

    [DllImport("USER32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


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

    /// <summary>
    /// Flags used for <see cref="GetWindowLong"/> and <see cref="SetWindowLong(HWND, WindowLongFlags, int)"/> methods to retrieve information about a window.
    /// </summary>
    public enum WindowLongFlags
    {
        /// <summary>The extended window styles</summary>
        GWL_EXSTYLE = -20,

        /// <summary>The application instance handle</summary>
        GWL_HINSTANCE = -6,

        /// <summary>The parent window handle</summary>
        GWL_HWNDPARENT = -8,

        /// <summary>The window identifier</summary>
        GWL_ID = -12,

        /// <summary>The window styles</summary>
        GWL_STYLE = -16,

        /// <summary>The window user data</summary>
        GWL_USERDATA = -21,

        /// <summary>The window procedure address or handle</summary>
        GWL_WNDPROC = -4,

        /// <summary>The dialog user data</summary>
        DWLP_USER = 0x8,

        /// <summary>The dialog procedure message result</summary>
        DWLP_MSGRESULT = 0x0,

        /// <summary>The dialog procedure address or handle</summary>
        DWLP_DLGPROC = 0x4
    }

    [DllImport("USER32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [System.Security.SecurityCritical]
    public static extern int GetWindowLong(IntPtr hWnd, WindowLongFlags nIndex);

    [DllImport("USER32.dll")]
    public static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        uint uFlags);

    [DllImport("USER32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern bool TrackMouseEvent([In, Out] TRACKMOUSEEVENT lpEventTrack);

    [DllImport("USER32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("USER32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("USER32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("USER32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern uint TrackPopupMenuEx(IntPtr hMenu, TrackPopupMenuFlags uFlags, int x, int y, IntPtr hWnd, [In, Optional] IntPtr lptpm);

    [DllImport("USER32.dll")]
    public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    #endregion USER32.dll

    #region KERNEL32.dll

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    #endregion KERNEL32.dll

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

    [DllImport("GDI32.dll")]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight,
        IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

    [DllImport("GDI32.dll", EntryPoint = "StretchBlt")]
    public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        TernaryRasterOperations dwRop);

    [DllImport("GDI32.dll")]
    public static extern bool SetStretchBltMode(IntPtr hdc, StretchMode iStretchMode);

    #endregion GDI32.dll

    #region DWMAPI.dll

    [DllImport("DWMAPI.dll")]
    public static extern int DwmIsCompositionEnabled(out bool isEnabled);

    [DllImport("DWMAPI.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

    #endregion DWMAPI.dll
}