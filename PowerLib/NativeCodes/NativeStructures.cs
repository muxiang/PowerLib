using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace PowerLib.NativeCodes;

/// <summary>
/// 封装本地代码使用的非托管结构
/// </summary>
public static class NativeStructures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
            
        public int X
        {
            get { return Left; }
            set { Right -= Left - value; Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= Top - value; Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }
            
        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }
            
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", 
                Left, Top, Right, Bottom);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public RECT rcPaint;
        public int fRestore;
        public int fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    public struct COPYDATASTRUCT
    {
        public IntPtr dwData; // 可以是任意值
        public int cbData;    // 指定lpData内存区域的字节数
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData; // 发送给目录窗口所在进程的数据
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;

        public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
        {
            BlendOp = op;
            BlendFlags = flags;
            SourceConstantAlpha = alpha;
            AlphaFormat = format;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public RECT[] rgrc;
        public WINDOWPOS lppos;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TRACKMOUSEEVENT
    {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MSLLHOOKSTRUCT
    {
        /// <summary>
        /// <para>Type: <c>POINT</c></para>
        /// <para>The x- and y-coordinates of the cursor, in per-monitor-aware screen coordinates.</para>
        /// </summary>
        public POINT pt;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. The low-order word is reserved. A
        /// positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel
        /// was rotated backward, toward the user. One wheel click is defined as <c>WHEEL_DELTA</c>, which is 120.
        /// </para>
        /// <para>
        /// If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, or WM_NCXBUTTONDBLCLK,
        /// the high-order word specifies which X button was pressed or released, and the low-order word is reserved. This value can be
        /// one or more of the following values. Otherwise, <c>mouseData</c> is not used.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>XBUTTON1 0x0001</term>
        /// <term>The first X button was pressed or released.</term>
        /// </item>
        /// <item>
        /// <term>XBUTTON2 0x0002</term>
        /// <term>The second X button was pressed or released.</term>
        /// </item>
        /// </list>
        /// </summary>
        public uint mouseData;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// The event-injected flags. An application can use the following values to test the flags. Testing LLMHF_INJECTED (bit 0) will
        /// tell you whether the event was injected. If it was, then testing LLMHF_LOWER_IL_INJECTED (bit 1) will tell you whether or
        /// not the event was injected from a process running at lower integrity level.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>LLMHF_INJECTED 0x00000001</term>
        /// <term>Test the event-injected (from any process) flag.</term>
        /// </item>
        /// <item>
        /// <term>LLMHF_LOWER_IL_INJECTED 0x00000002</term>
        /// <term>Test the event-injected (from a process running at lower integrity level) flag.</term>
        /// </item>
        /// </list>
        /// </summary>
        public uint flags;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>The time stamp for this message.</para>
        /// </summary>
        public uint time;

        /// <summary>
        /// <para>Type: <c>ULONG_PTR</c></para>
        /// <para>Additional information associated with the message.</para>
        /// </summary>
        public UIntPtr dwExtraInfo;
    }
}