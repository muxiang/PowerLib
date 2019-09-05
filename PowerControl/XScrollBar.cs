using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using PowerControl.Properties;

namespace PowerControl
{
    [Designer(typeof(ScrollbarControlDesigner))]
    public partial class XScrollBar : Control
    {
        public XScrollBar()
        {
            InitializeComponent();
            
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            moChannelColor = Color.FromArgb(51, 166, 3);
            UpArrowImage = Resources.uparrow;
            DownArrowImage = Resources.downarrow;

            ThumbBottomImage = Resources.ThumbBottom;
            ThumbBottomSpanImage = Resources.ThumbSpanBottom;
            ThumbTopImage = Resources.ThumbTop;
            ThumbTopSpanImage = Resources.ThumbSpanTop;
            ThumbMiddleImage = Resources.ThumbMiddle;

            Width = UpArrowImage.Width;
            MinimumSize = new Size(UpArrowImage.Width, UpArrowImage.Height + DownArrowImage.Height + GetThumbHeight());
        }

        protected Color moChannelColor = Color.Empty;
        protected Image moUpArrowImage;
        //protected Image moUpArrowImage_Over = null;
        //protected Image moUpArrowImage_Down = null;
        protected Image moDownArrowImage;
        //protected Image moDownArrowImage_Over = null;
        //protected Image moDownArrowImage_Down = null;
        protected Image moThumbArrowImage = null;

        protected Image moThumbTopImage;
        protected Image moThumbTopSpanImage;
        protected Image moThumbBottomImage;
        protected Image moThumbBottomSpanImage;
        protected Image moThumbMiddleImage;

        protected int moLargeChange = 10;
        protected int moSmallChange = 1;
        protected int moMinimum;
        protected int moMaximum = 100;
        protected int moValue;
        private int nClickPoint;

        protected int moThumbTop;

        protected bool moAutoSize = false;

        private bool moThumbDown;
        private bool moThumbDragging;

        public new event EventHandler Scroll;
        public event EventHandler ValueChanged;

        private int GetThumbHeight()
        {
            int nTrackHeight = Height - (UpArrowImage.Height + DownArrowImage.Height);
            float fThumbHeight = LargeChange / (float)Maximum * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            return nThumbHeight;
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        public int LargeChange
        {
            get { return moLargeChange; }
            set
            {
                moLargeChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange
        {
            get { return moSmallChange; }
            set
            {
                moSmallChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        public int Minimum
        {
            get { return moMinimum; }
            set
            {
                moMinimum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        public int Maximum
        {
            get { return moMaximum; }
            set
            {
                moMaximum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        public int Value
        {
            get { return moValue; }
            set
            {
                moValue = value;

                int nTrackHeight = Height - (UpArrowImage.Height + DownArrowImage.Height);
                float fThumbHeight = LargeChange / (float)Maximum * nTrackHeight;
                int nThumbHeight = (int)fThumbHeight;

                if (nThumbHeight > nTrackHeight)
                {
                    nThumbHeight = nTrackHeight;
                    fThumbHeight = nTrackHeight;
                }
                if (nThumbHeight < 56)
                {
                    nThumbHeight = 56;
                    fThumbHeight = 56;
                }

                //figure out value
                int nPixelRange = nTrackHeight - nThumbHeight;
                int nRealRange = Maximum - Minimum - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                    fPerc = moValue / (float)nRealRange;

                float fTop = fPerc * nPixelRange;
                moThumbTop = (int)fTop;

                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        public Color ChannelColor
        {
            get { return moChannelColor; }
            set { moChannelColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image UpArrowImage
        {
            get { return moUpArrowImage; }
            set { moUpArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image DownArrowImage
        {
            get { return moDownArrowImage; }
            set { moDownArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopImage
        {
            get { return moThumbTopImage; }
            set { moThumbTopImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopSpanImage
        {
            get { return moThumbTopSpanImage; }
            set { moThumbTopSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomImage
        {
            get { return moThumbBottomImage; }
            set { moThumbBottomImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomSpanImage
        {
            get { return moThumbBottomSpanImage; }
            set { moThumbBottomSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbMiddleImage
        {
            get { return moThumbMiddleImage; }
            set { moThumbMiddleImage = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            if (UpArrowImage != null)
            {
                e.Graphics.DrawImage(UpArrowImage, new Rectangle(new Point(0, 0), new Size(Width, UpArrowImage.Height)));
            }

            Brush oBrush = new SolidBrush(moChannelColor);
            Brush oWhiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));

            //draw channel left and right border colors
            e.Graphics.FillRectangle(oWhiteBrush, new Rectangle(0, UpArrowImage.Height, 1, Height - DownArrowImage.Height));
            e.Graphics.FillRectangle(oWhiteBrush, new Rectangle(Width - 1, UpArrowImage.Height, 1, Height - DownArrowImage.Height));

            //draw channel
            e.Graphics.FillRectangle(oBrush, new Rectangle(1, UpArrowImage.Height, Width - 2, Height - DownArrowImage.Height));

            //draw thumb
            int nTrackHeight = Height - (UpArrowImage.Height + DownArrowImage.Height);
            float fThumbHeight = LargeChange / (float)Maximum * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            //Debug.WriteLine(nThumbHeight.ToString());

            float fSpanHeight = (fThumbHeight - (ThumbMiddleImage.Height + ThumbTopImage.Height + ThumbBottomImage.Height)) / 2.0f;
            int nSpanHeight = (int)fSpanHeight;

            int nTop = moThumbTop;
            nTop += UpArrowImage.Height;

            //draw top
            e.Graphics.DrawImage(ThumbTopImage, new Rectangle(1, nTop, Width - 2, ThumbTopImage.Height));

            nTop += ThumbTopImage.Height;
            //draw top span
            Rectangle rect = new Rectangle(1, nTop, Width - 2, nSpanHeight);


            e.Graphics.DrawImage(ThumbTopSpanImage, 1.0f, nTop, Width - 2.0f, fSpanHeight * 2);

            nTop += nSpanHeight;
            //draw middle
            e.Graphics.DrawImage(ThumbMiddleImage, new Rectangle(1, nTop, Width - 2, ThumbMiddleImage.Height));


            nTop += ThumbMiddleImage.Height;
            //draw top span
            rect = new Rectangle(1, nTop, Width - 2, nSpanHeight * 2);
            e.Graphics.DrawImage(ThumbBottomSpanImage, rect);

            nTop += nSpanHeight;
            //draw bottom
            e.Graphics.DrawImage(ThumbBottomImage, new Rectangle(1, nTop, Width - 2, nSpanHeight));

            if (DownArrowImage != null)
            {
                e.Graphics.DrawImage(DownArrowImage, new Rectangle(new Point(0, Height - DownArrowImage.Height), new Size(Width, DownArrowImage.Height)));
            }

        }

        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                {
                    Width = moUpArrowImage.Width;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Point ptPoint = PointToClient(Cursor.Position);
            int nTrackHeight = Height - (UpArrowImage.Height + DownArrowImage.Height);
            float fThumbHeight = LargeChange / (float)Maximum * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            int nTop = moThumbTop;
            nTop += UpArrowImage.Height;


            Rectangle thumbrect = new Rectangle(new Point(1, nTop), new Size(ThumbMiddleImage.Width, nThumbHeight));
            if (thumbrect.Contains(ptPoint))
            {

                //hit the thumb
                nClickPoint = ptPoint.Y - nTop;
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                moThumbDown = true;
            }

            Rectangle uparrowrect = new Rectangle(new Point(1, 0), new Size(UpArrowImage.Width, UpArrowImage.Height));
            if (uparrowrect.Contains(ptPoint))
            {

                int nRealRange = Maximum - Minimum - LargeChange;
                int nPixelRange = nTrackHeight - nThumbHeight;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (moThumbTop - SmallChange < 0)
                            moThumbTop = 0;
                        else
                            moThumbTop -= SmallChange;

                        //figure out value
                        float fPerc = moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());

                        Invalidate();
                    }
                }
            }

            Rectangle downarrowrect = new Rectangle(new Point(1, UpArrowImage.Height + nTrackHeight), new Size(UpArrowImage.Width, UpArrowImage.Height));
            if (downarrowrect.Contains(ptPoint))
            {
                int nRealRange = Maximum - Minimum - LargeChange;
                int nPixelRange = nTrackHeight - nThumbHeight;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (moThumbTop + SmallChange > nPixelRange)
                            moThumbTop = nPixelRange;
                        else
                            moThumbTop += SmallChange;

                        //figure out value
                        float fPerc = moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());

                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            moThumbDown = false;
            moThumbDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (moThumbDown)
                moThumbDragging = true;

            if (moThumbDragging)
                MoveThumb(e.Y);

            ValueChanged?.Invoke(this, new EventArgs());
            Scroll?.Invoke(this, new EventArgs());
        }

        private void MoveThumb(int y)
        {
            int nRealRange = Maximum - Minimum;
            int nTrackHeight = Height - (UpArrowImage.Height + DownArrowImage.Height);
            float fThumbHeight = LargeChange / (float)Maximum * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            int nSpot = nClickPoint;

            int nPixelRange = nTrackHeight - nThumbHeight;
            if (moThumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (UpArrowImage.Height + nSpot);

                    if (nNewThumbTop < 0)
                    {
                        moThumbTop = nNewThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        moThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else
                    {
                        moThumbTop = y - (UpArrowImage.Height + nSpot);
                    }

                    //figure out value
                    float fPerc = moThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    moValue = (int)fValue;
                    Debug.WriteLine(moValue.ToString());

                    Application.DoEvents();

                    Invalidate();
                }
            }
        }
    }

    internal class ScrollbarControlDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(Component)["AutoSize"];
                if (propDescriptor != null)
                {
                    bool autoSize = (bool)propDescriptor.GetValue(Component);
                    if (autoSize)
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        }
    }
}
