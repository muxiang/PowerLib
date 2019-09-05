using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerControl;

namespace ControlTest
{
    public partial class MyForm2Test : XForm2
    {
        //public Win32API.SCROLLINFO tvImageListScrollInfo
        //{
        //    get
        //    {
        //        Win32API.SCROLLINFO si = new Win32API.SCROLLINFO();
        //        si.cbSize = (uint)Marshal.SizeOf(si);
        //        si.fMask = (int)(/*Win32API.ScrollInfoMask.SIF_DISABLENOSCROLL | */Win32API.ScrollInfoMask.SIF_ALL);
        //        Win32API.GetScrollInfo(panel1.Handle, (int)Win32API.ScrollBarDirection.SB_VERT, ref si);
        //        Debug.Print($"--------nmax:{si.nMax},npos:{si.nPos},nTrackPos:{si.nTrackPos}");
        //        //customScrollBar1.Maximum = si.nMax;
                
        //        //customScrollBar1.Value = si.nPos;
        //        return si;
        //    }
        //}

        //当鼠标滚动时，设置该滚动条
        //private void SetImageListScroll()
        //{
        //    Win32API.SCROLLINFO info = tvImageListScrollInfo;

        //    if (info.nMax > 0)
        //    {

        //        int pos = info.nPos - 1;
        //        if (pos >= 0)
        //        {
        //            customScrollBar1.Value = pos;
        //        }
        //    }
        //}

        public MyForm2Test()
        {
            InitializeComponent();
        }

        //private void customScrollBar1_Scroll(object sender, EventArgs e)
        //{
        //    //Win32API.SCROLLINFO info = tvImageListScrollInfo;
        //    //info.nPos = customScrollBar1.Value;
        //    //Debug.Print($"nPos:{info.nPos},nTrackPos:{info.nTrackPos}");
        //    //Debug.Print($"Max:{customScrollBar1.Maximum},Value:{customScrollBar1.Value}");


        //    //int currPos = Win32API.SetScrollInfo(panel1.Handle, (int)Win32API.ScrollBarDirection.SB_VERT, ref info, true);
        //    //Debug.Print($"currPos:{currPos}");
        //    //Win32API.PostMessage(panel1.Handle, Win32API.WM_VSCROLL, Win32API.MakeLong((short)Win32API.SB_THUMBTRACK, (short)(customScrollBar1.Value)), /*Handle.ToInt32()*/0);
            

        //    customScrollBar1.Maximum = panel1.VerticalScroll.Maximum;
        //    customScrollBar1.Minimum = panel1.VerticalScroll.Minimum;
        //    customScrollBar1.LargeChange = panel1.VerticalScroll.LargeChange;
        //    customScrollBar1.SmallChange = panel1.VerticalScroll.SmallChange;
        //    //customScrollBar1.Value = panel1.VerticalScroll.Value;

        //    panel1.VerticalScroll.Value = customScrollBar1.Value;

        //    Debug.Print($"。。。。。。。。Max:{panel1.VerticalScroll.Maximum}");
        //    Debug.Print($"。。。。。。。。Min:{panel1.VerticalScroll.Minimum}");
        //    Debug.Print($"。。。。。。。。Value:{panel1.VerticalScroll.Value}");
        //    Debug.Print($"。。。。。。。。LargeChange:{panel1.VerticalScroll.LargeChange}");



        //    //Win32API.SCROLLINFO infoSetting = new Win32API.SCROLLINFO();
        //    //infoSetting.cbSize = (uint)Marshal.SizeOf(infoSetting);
        //    //infoSetting.nMax = customScrollBar1.Maximum;
        //    //infoSetting.nMin = customScrollBar1.Minimum;
        //    //infoSetting.fMask = (int)(/*Win32API.ScrollInfoMask.SIF_DISABLENOSCROLL | */Win32API.ScrollInfoMask.SIF_RANGE);
        //    //Win32API.SetScrollInfo(panel1.Handle, (int)Win32API.ScrollBarDirection.SB_VERT, ref infoSetting, true);

        //    //Win32API.SCROLLINFO si = new Win32API.SCROLLINFO();
        //    //si.cbSize = (uint)Marshal.SizeOf(si);
        //    //si.fMask = (int)(/*Win32API.ScrollInfoMask.SIF_DISABLENOSCROLL | */Win32API.ScrollInfoMask.SIF_ALL);
        //    //Win32API.GetScrollInfo(panel1.Handle, (int)Win32API.ScrollBarDirection.SB_VERT, ref si);
        //    //Debug.Print($"--------nmax:{si.nMax},npos:{si.nPos},nTrackPos:{si.nTrackPos}");
        //}
    }
}
