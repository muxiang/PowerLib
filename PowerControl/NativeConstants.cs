namespace PowerControl
{
    class NativeConstants
    {
        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_PRINTCLIENT = 0x0318;

        public const int WM_NCACTIVATE = 0x86;
        public const int WM_NCPAINT = 0x85;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCMOUSEHOVER = 0x02A0;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCCREATE = 0x0081;
        //public const int WM_RBUTTONUP = 0x0205;  

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_CLOSE = 0x0010;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_SIZE = 0x0005;
        public const int WM_SIZING = 0x0214;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_CTLCOLOREDIT = 0x133;


        // FOR WM_SIZING MSG WPARAM  
        public const int WMSZ_BOTTOM = 6;
        public const int WMSZ_BOTTOMLEFT = 7;
        public const int WMSZ_BOTTOMRIGHT = 8;
        public const int WMSZ_LEFT = 1;
        public const int WMSZ_RIGHT = 2;
        public const int WMSZ_TOP = 3;
        public const int WMSZ_TOPLEFT = 4;
        public const int WMSZ_TOPRIGHT = 5;

        // left mouse button is down.  
        public const int MK_LBUTTON = 0x0001;

        public const int SC_CLOSE = 0xF060;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MOVE = 0xF010;
        public const int SC_RESTORE = 0xF120;
        public const int SC_CONTEXTHELP = 0xF180;

        public const int HTCAPTION = 2;
        public const int HTCLOSE = 20;
        public const int HTHELP = 21;
        public const int HTMAXBUTTON = 9;
        public const int HTMINBUTTON = 8;
        public const int HTTOP = 12;

        public const int SM_CYBORDER = 6;
        public const int SM_CXBORDER = 5;
        public const int SM_CYCAPTION = 4;

        public const int CS_DropSHADOW = 0x20000;
        public const int GCL_STYLE = (-26);

    }
}
