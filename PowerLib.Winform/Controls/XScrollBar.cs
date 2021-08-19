using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using PowerLib.Winform.Properties;

namespace PowerLib.Winform.Controls
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

            MoChannelColor = Color.FromArgb(212, 212, 212);
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

        protected Color MoChannelColor = Color.Empty;
        protected Image MoUpArrowImage;
        // protected Image moUpArrowImage_Over = null;
        // protected Image moUpArrowImage_Down = null;
        protected Image MoDownArrowImage;
        // protected Image moDownArrowImage_Over = null;
        // protected Image moDownArrowImage_Down = null;
        protected Image MoThumbArrowImage = null;

        protected Image MoThumbTopImage;
        protected Image MoThumbTopSpanImage;
        protected Image MoThumbBottomImage;
        protected Image MoThumbBottomSpanImage;
        protected Image MoThumbMiddleImage;

        protected int MoLargeChange = 10;
        protected int MoSmallChange = 1;
        protected int MoMinimum;
        protected int MoMaximum = 100;
        protected int MoValue;
        private int _nClickPoint;

        protected int MoThumbTop;

        protected bool MoAutoSize = false;

        private bool _moThumbDown;
        private bool _moThumbDragging;

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
            get { return MoLargeChange; }
            set
            {
                MoLargeChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange
        {
            get { return MoSmallChange; }
            set
            {
                MoSmallChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        public int Minimum
        {
            get { return MoMinimum; }
            set
            {
                MoMinimum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        public int Maximum
        {
            get { return MoMaximum; }
            set
            {
                MoMaximum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        public int Value
        {
            get { return MoValue; }
            set
            {
                MoValue = value;

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

                // figure out value
                int nPixelRange = nTrackHeight - nThumbHeight;
                int nRealRange = Maximum - Minimum - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                    fPerc = MoValue / (float)nRealRange;

                float fTop = fPerc * nPixelRange;
                MoThumbTop = (int)fTop;

                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        public Color ChannelColor
        {
            get { return MoChannelColor; }
            set { MoChannelColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image UpArrowImage
        {
            get { return MoUpArrowImage; }
            set { MoUpArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image DownArrowImage
        {
            get { return MoDownArrowImage; }
            set { MoDownArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopImage
        {
            get { return MoThumbTopImage; }
            set { MoThumbTopImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopSpanImage
        {
            get { return MoThumbTopSpanImage; }
            set { MoThumbTopSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomImage
        {
            get { return MoThumbBottomImage; }
            set { MoThumbBottomImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomSpanImage
        {
            get { return MoThumbBottomSpanImage; }
            set { MoThumbBottomSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbMiddleImage
        {
            get { return MoThumbMiddleImage; }
            set { MoThumbMiddleImage = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            if (UpArrowImage != null)
            {
                e.Graphics.DrawImage(UpArrowImage, new Rectangle(new Point(0, 0), new Size(Width, UpArrowImage.Height)));
            }

            Brush oBrush = new SolidBrush(MoChannelColor);

            // draw channel left and right border colors
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, UpArrowImage.Height, 1, Height - DownArrowImage.Height));
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width - 1, UpArrowImage.Height, 1, Height - DownArrowImage.Height));

            // draw channel
            e.Graphics.FillRectangle(oBrush, new Rectangle(1, UpArrowImage.Height, Width - 2, Height - DownArrowImage.Height));

            // draw thumb
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

            // Debug.WriteLine(nThumbHeight.ToString());

            float fSpanHeight = (fThumbHeight - (ThumbMiddleImage.Height + ThumbTopImage.Height + ThumbBottomImage.Height)) / 2.0f;
            int nSpanHeight = (int)fSpanHeight;

            int nTop = MoThumbTop;
            nTop += UpArrowImage.Height;

            // draw top
            e.Graphics.DrawImage(ThumbTopImage, new Rectangle(1, nTop, Width - 2, ThumbTopImage.Height));

            nTop += ThumbTopImage.Height;
            // draw top span
            Rectangle rect = new Rectangle(1, nTop, Width - 2, nSpanHeight);


            e.Graphics.DrawImage(ThumbTopSpanImage, 1.0f, nTop, Width - 2.0f, fSpanHeight * 2);

            nTop += nSpanHeight;
            // draw middle
            e.Graphics.DrawImage(ThumbMiddleImage, new Rectangle(1, nTop, Width - 2, ThumbMiddleImage.Height));


            nTop += ThumbMiddleImage.Height;
            // draw top span
            rect = new Rectangle(1, nTop, Width - 2, nSpanHeight * 2);
            e.Graphics.DrawImage(ThumbBottomSpanImage, rect);

            nTop += nSpanHeight;
            // draw bottom
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
                    Width = MoUpArrowImage.Width;
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

            int nTop = MoThumbTop;
            nTop += UpArrowImage.Height;


            Rectangle thumbrect = new Rectangle(new Point(1, nTop), new Size(ThumbMiddleImage.Width, nThumbHeight));
            if (thumbrect.Contains(ptPoint))
            {

                // hit the thumb
                _nClickPoint = ptPoint.Y - nTop;
                // MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                _moThumbDown = true;
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
                        if (MoThumbTop - SmallChange < 0)
                            MoThumbTop = 0;
                        else
                            MoThumbTop -= SmallChange;

                        // figure out value
                        float fPerc = MoThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        MoValue = (int)fValue;
                        Debug.WriteLine(MoValue.ToString());

                        ValueChanged?.Invoke(this, new EventArgs());

                        Scroll?.Invoke(this, new EventArgs());

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
                        if (MoThumbTop + SmallChange > nPixelRange)
                            MoThumbTop = nPixelRange;
                        else
                            MoThumbTop += SmallChange;

                        // figure out value
                        float fPerc = MoThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        MoValue = (int)fValue;
                        Debug.WriteLine(MoValue.ToString());

                        ValueChanged?.Invoke(this, new EventArgs());

                        Scroll?.Invoke(this, new EventArgs());

                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _moThumbDown = false;
            _moThumbDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_moThumbDown)
                _moThumbDragging = true;

            if (_moThumbDragging)
            {
                MoveThumb(e.Y);
                ValueChanged?.Invoke(this, new EventArgs());
                Scroll?.Invoke(this, new EventArgs());
            }
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

            int nSpot = _nClickPoint;

            int nPixelRange = nTrackHeight - nThumbHeight;
            if (_moThumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (UpArrowImage.Height + nSpot);

                    if (nNewThumbTop < 0)
                    {
                        MoThumbTop = nNewThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        MoThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else
                    {
                        MoThumbTop = y - (UpArrowImage.Height + nSpot);
                    }

                    // figure out value
                    float fPerc = MoThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    MoValue = (int)fValue;
                    Debug.WriteLine(MoValue.ToString());

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
