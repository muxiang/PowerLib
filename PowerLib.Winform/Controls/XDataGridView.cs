﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PowerLib.Winform.Controls
{
    /// <inheritdoc cref="XDataGridView"/>
    public partial class XDataGridView : UserControl, ISupportInitialize
    {
        /// <summary>
        /// 缺省表格列头样式
        /// </summary>
        private static readonly DataGridViewCellStyle DefaultTableHeaderStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Font = new Font("微软雅黑", 8F, FontStyle.Bold, GraphicsUnit.Point, 134),
            BackColor = Color.FromArgb(249, 249, 251),
            ForeColor = Color.Black,
            SelectionBackColor = Color.FromArgb(249, 249, 251),
            SelectionForeColor = Color.Black,
            WrapMode = DataGridViewTriState.True,
        };

        /// <summary>
        /// 缺省表格单元格样式
        /// </summary>
        private static readonly DataGridViewCellStyle DefaultTableCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Font = new Font("微软雅黑", 8F, FontStyle.Bold, GraphicsUnit.Point, 134),
            BackColor = Color.FromArgb(255, 255, 255),
            ForeColor = Color.Black,
            SelectionBackColor = Color.FromArgb(115, 180, 247),
            SelectionForeColor = Color.White,
            WrapMode = DataGridViewTriState.False,
        };

        /// <summary>
        /// 初始化<see cref="XDataGridView"/>的实例
        /// </summary>
        public XDataGridView()
        {
            InitializeComponent();

            KeepInnerAndScrollBar();

            xScrollBar1.Scroll += (s1, e1) =>
            {
                if (dataGridView1.RowCount == 0) return;
                dataGridView1.FirstDisplayedScrollingRowIndex = xScrollBar1.Value / dataGridView1.RowTemplate.Height;
                xScrollBar1.Invalidate();
                Application.DoEvents();
            };

            dataGridView1.ColumnHeadersDefaultCellStyle = DefaultTableHeaderStyle;
            dataGridView1.DefaultCellStyle = DefaultTableCellStyle;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.GridColor = Color.FromArgb(233, 233, 235);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            dataGridView1.RowsRemoved += DataGridView1_RowsRemoved;

            dataGridView1.MouseMove += (s1, e1) => { if (!dataGridView1.Focused) dataGridView1.Focus(); };
            dataGridView1.MouseWheel += (s1, e1) => xScrollBar1.Value = Math.Abs(dataGridView1.VerticalScrollingOffset);
        }

        #region 属性
        
        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell this[string columnName, int rowIndex]
        {
            get => dataGridView1[columnName, rowIndex];
            set => dataGridView1[columnName, rowIndex] = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell this[int columnIndex, int rowIndex]
        {
            get => dataGridView1[columnIndex, rowIndex];
            set => dataGridView1[columnIndex, rowIndex] = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool RowHeadersVisible
        {
            get => dataGridView1.RowHeadersVisible;
            set => dataGridView1.RowHeadersVisible = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [AmbientValue(null)]
        public DataGridViewCellStyle RowHeadersDefaultCellStyle
        {
            get => dataGridView1.RowHeadersDefaultCellStyle;
            set => dataGridView1.RowHeadersDefaultCellStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(DataGridViewHeaderBorderStyle.Raised)]
        public DataGridViewHeaderBorderStyle RowHeadersBorderStyle
        {
            get => dataGridView1.RowHeadersBorderStyle;
            set => dataGridView1.RowHeadersBorderStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int RowCount
        {
            get => dataGridView1.RowCount;
            set => dataGridView1.RowCount = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => dataGridView1.ReadOnly;
            set => dataGridView1.ReadOnly = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get => dataGridView1.Padding;
            set => dataGridView1.Padding = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NewRowIndex
        {
            get => dataGridView1.NewRowIndex;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool MultiSelect
        {
            get => dataGridView1.MultiSelect;
            set => dataGridView1.MultiSelect = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public bool IsCurrentRowDirty
        {
            get => dataGridView1.IsCurrentRowDirty;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public bool IsCurrentCellInEditMode
        {
            get => dataGridView1.IsCurrentCellInEditMode;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public bool IsCurrentCellDirty
        {
            get => dataGridView1.IsCurrentCellDirty;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HorizontalScrollingOffset
        {
            get => dataGridView1.HorizontalScrollingOffset;
            set => dataGridView1.HorizontalScrollingOffset = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public Color GridColor
        {
            get => dataGridView1.GridColor;
            set => dataGridView1.GridColor = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Font Font
        {
            get => dataGridView1.Font;
            set => dataGridView1.Font = value;
        }
        
        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FirstDisplayedScrollingRowIndex
        {
            get => dataGridView1.FirstDisplayedScrollingRowIndex;
            set => dataGridView1.FirstDisplayedScrollingRowIndex = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FirstDisplayedScrollingColumnIndex
        {
            get => dataGridView1.FirstDisplayedScrollingColumnIndex;
            set => dataGridView1.FirstDisplayedScrollingColumnIndex = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int FirstDisplayedScrollingColumnHiddenWidth => dataGridView1.FirstDisplayedScrollingColumnHiddenWidth;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell FirstDisplayedCell
        {
            get => dataGridView1.FirstDisplayedCell;
            set => dataGridView1.FirstDisplayedCell = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Localizable(true)]
        public int RowHeadersWidth
        {
            get => dataGridView1.RowHeadersWidth;
            set => dataGridView1.RowHeadersWidth = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(DataGridViewRowHeadersWidthSizeMode.EnableResizing)]
        [RefreshProperties(RefreshProperties.All)]
        public DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
        {
            get => dataGridView1.RowHeadersWidthSizeMode;
            set => dataGridView1.RowHeadersWidthSizeMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewRowCollection Rows => dataGridView1.Rows;

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool VirtualMode
        {
            get => dataGridView1.VirtualMode;
            set => dataGridView1.VirtualMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VerticalScrollingOffset => dataGridView1.VerticalScrollingOffset;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Cursor UserSetCursor => dataGridView1.UserSetCursor;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewHeaderCell TopLeftHeaderCell
        {
            get => dataGridView1.TopLeftHeaderCell;
            set => dataGridView1.TopLeftHeaderCell = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text
        {
            get => dataGridView1.Text;
            set => dataGridView1.Text = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool StandardTab
        {
            get => dataGridView1.StandardTab;
            set => dataGridView1.StandardTab = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public SortOrder SortOrder => dataGridView1.SortOrder;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewColumn SortedColumn => dataGridView1.SortedColumn;

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool EnableHeadersVisualStyles
        {
            get => dataGridView1.EnableHeadersVisualStyles;
            set => dataGridView1.EnableHeadersVisualStyles = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool ShowRowErrors
        {
            get => dataGridView1.ShowRowErrors;
            set => dataGridView1.ShowRowErrors = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool ShowCellToolTips
        {
            get => dataGridView1.ShowCellToolTips;
            set => dataGridView1.ShowCellToolTips = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool ShowCellErrors
        {
            get => dataGridView1.ShowCellErrors;
            set => dataGridView1.ShowCellErrors = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(DataGridViewSelectionMode.RowHeaderSelect)]
        public DataGridViewSelectionMode SelectionMode
        {
            get => dataGridView1.SelectionMode;
            set => dataGridView1.SelectionMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewSelectedRowCollection SelectedRows => dataGridView1.SelectedRows;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewSelectedColumnCollection SelectedColumns => dataGridView1.SelectedColumns;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewSelectedCellCollection SelectedCells => dataGridView1.SelectedCells;

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(ScrollBars.Both)]
        [Localizable(true)]
        public ScrollBars ScrollBars
        {
            get => dataGridView1.ScrollBars;
            set => dataGridView1.ScrollBars = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataGridViewRow RowTemplate
        {
            get => dataGridView1.RowTemplate;
            set => dataGridView1.RowTemplate = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(true)]
        public bool ShowEditingIcon
        {
            get => dataGridView1.ShowEditingIcon;
            set => dataGridView1.ShowEditingIcon = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Panel EditingPanel => dataGridView1.EditingPanel;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new Color ForeColor
        {
            get => dataGridView1.ForeColor;
            set => dataGridView1.ForeColor = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(DataGridViewEditMode.EditOnKeystrokeOrF2)]
        public DataGridViewEditMode EditMode
        {
            get => dataGridView1.EditMode;
            set => dataGridView1.EditMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage
        {
            get => dataGridView1.BackgroundImage;
            set => dataGridView1.BackgroundImage = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public Color BackgroundColor
        {
            get => dataGridView1.BackgroundColor;
            set => dataGridView1.BackgroundColor = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor
        {
            get => dataGridView1.BackColor;
            set => dataGridView1.BackColor = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(DataGridViewAutoSizeRowsMode.None)]
        public DataGridViewAutoSizeRowsMode AutoSizeRowsMode
        {
            get => dataGridView1.AutoSizeRowsMode;
            set => dataGridView1.AutoSizeRowsMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(DataGridViewAutoSizeColumnsMode.None)]
        public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        {
            get => dataGridView1.AutoSizeColumnsMode;
            set => dataGridView1.AutoSizeColumnsMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Control EditingControl => dataGridView1.EditingControl;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool AutoGenerateColumns
        {
            get => dataGridView1.AutoGenerateColumns;
            set => dataGridView1.AutoGenerateColumns = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public DataGridViewCellStyle AlternatingRowsDefaultCellStyle
        {
            get => dataGridView1.AlternatingRowsDefaultCellStyle;
            set => dataGridView1.AlternatingRowsDefaultCellStyle = value;
        }
        
        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedRowHeadersBorderStyle => dataGridView1.AdvancedRowHeadersBorderStyle;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedColumnHeadersBorderStyle => dataGridView1.AdvancedColumnHeadersBorderStyle;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedCellBorderStyle => dataGridView1.AdvancedCellBorderStyle;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdjustedTopLeftHeaderBorderStyle => dataGridView1.AdjustedTopLeftHeaderBorderStyle;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ImageLayout BackgroundImageLayout
        {
            get => dataGridView1.BackgroundImageLayout;
            set => dataGridView1.BackgroundImageLayout = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(BorderStyle.FixedSingle)]
        public new BorderStyle BorderStyle
        {
            get => dataGridView1.BorderStyle;
            set => dataGridView1.BorderStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public new bool AutoSize
        {
            get => dataGridView1.AutoSize;
            set => dataGridView1.AutoSize = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(DataGridViewCellBorderStyle.Single)]
        public DataGridViewCellBorderStyle CellBorderStyle
        {
            get => dataGridView1.CellBorderStyle;
            set => dataGridView1.CellBorderStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [AmbientValue(null)]
        public DataGridViewCellStyle DefaultCellStyle
        {
            get => dataGridView1.DefaultCellStyle;
            set => dataGridView1.DefaultCellStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource
        {
            get => dataGridView1.DataSource;
            set => dataGridView1.DataSource = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get => dataGridView1.DataMember;
            set => dataGridView1.DataMember = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public DataGridViewRow CurrentRow => dataGridView1.CurrentRow;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        public Point CurrentCellAddress => dataGridView1.CurrentCellAddress;

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell CurrentCell
        {
            get => dataGridView1.CurrentCell;
            set => dataGridView1.CurrentCell = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Editor(typeof(XDataGridViewColumnCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        // [MergableProperty(false)]
        public DataGridViewColumnCollection Columns => dataGridView1.Columns;

        /// <inheritdoc cref="XDataGridView"/>
        public new Rectangle DisplayRectangle => dataGridView1.DisplayRectangle;

        /// <inheritdoc cref="XDataGridView"/>
        [DefaultValue(DataGridViewColumnHeadersHeightSizeMode.EnableResizing)]
        [RefreshProperties(RefreshProperties.All)]
        public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get => dataGridView1.ColumnHeadersHeightSizeMode;
            set => dataGridView1.ColumnHeadersHeightSizeMode = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Localizable(true)]
        public int ColumnHeadersHeight
        {
            get => dataGridView1.ColumnHeadersHeight;
            set => dataGridView1.ColumnHeadersHeight = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public bool ColumnHeadersVisible
        {
            get => dataGridView1.ColumnHeadersVisible;
            set => dataGridView1.ColumnHeadersVisible = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [AmbientValue(null)]
        public DataGridViewCellStyle ColumnHeadersDefaultCellStyle
        {
            get => dataGridView1.ColumnHeadersDefaultCellStyle;
            set => dataGridView1.ColumnHeadersDefaultCellStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(DataGridViewHeaderBorderStyle.Raised)]
        public DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle
        {
            get => dataGridView1.ColumnHeadersBorderStyle;
            set => dataGridView1.ColumnHeadersBorderStyle = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int ColumnCount
        {
            get => dataGridView1.ColumnCount;
            set => dataGridView1.ColumnCount = value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(true)]
        [DefaultValue(DataGridViewClipboardCopyMode.EnableWithAutoHeaderText)]
        public DataGridViewClipboardCopyMode ClipboardCopyMode
        {
            get => dataGridView1.ClipboardCopyMode;
            set => dataGridView1.ClipboardCopyMode = value;
        }

        #endregion 属性

        #region 事件

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler BackgroundColorChanged
        {
            add => dataGridView1.BackgroundColorChanged += value;
            remove => dataGridView1.BackgroundColorChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler BackColorChanged
        {
            add => dataGridView1.BackgroundColorChanged += value;
            remove => dataGridView1.BackgroundColorChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewAutoSizeModeEventHandler AutoSizeRowsModeChanged
        {
            add => dataGridView1.AutoSizeRowsModeChanged += value;
            remove => dataGridView1.AutoSizeRowsModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewAutoSizeColumnsModeEventHandler AutoSizeColumnsModeChanged
        {
            add => dataGridView1.AutoSizeColumnsModeChanged += value;
            remove => dataGridView1.AutoSizeColumnsModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler AutoGenerateColumnsChanged
        {
            add => dataGridView1.AutoGenerateColumnsChanged += value;
            remove => dataGridView1.AutoGenerateColumnsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AlternatingRowsDefaultCellStyleChanged
        {
            add => dataGridView1.AlternatingRowsDefaultCellStyleChanged += value;
            remove => dataGridView1.AlternatingRowsDefaultCellStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AllowUserToResizeRowsChanged
        {
            add => dataGridView1.AllowUserToResizeRowsChanged += value;
            remove => dataGridView1.AllowUserToResizeRowsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AllowUserToResizeColumnsChanged
        {
            add => dataGridView1.AllowUserToResizeColumnsChanged += value;
            remove => dataGridView1.AllowUserToResizeColumnsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AllowUserToOrderColumnsChanged
        {
            add => dataGridView1.AllowUserToOrderColumnsChanged += value;
            remove => dataGridView1.AllowUserToOrderColumnsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AllowUserToAddRowsChanged
        {
            add => dataGridView1.AllowUserToAddRowsChanged += value;
            remove => dataGridView1.AllowUserToAddRowsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler Sorted
        {
            add => dataGridView1.Sorted += value;
            remove => dataGridView1.Sorted -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewSortCompareEventHandler SortCompare
        {
            add => dataGridView1.SortCompare += value;
            remove => dataGridView1.SortCompare -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler SelectionChanged
        {
            add => dataGridView1.SelectionChanged += value;
            remove => dataGridView1.SelectionChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event ScrollEventHandler Scroll
        {
            add => dataGridView1.Scroll += value;
            remove => dataGridView1.Scroll -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewCellCancelEventHandler RowValidating
        {
            add => dataGridView1.RowValidating += value;
            remove => dataGridView1.RowValidating -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler AllowUserToDeleteRowsChanged
        {
            add => dataGridView1.AllowUserToDeleteRowsChanged += value;
            remove => dataGridView1.AllowUserToDeleteRowsChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler StyleChanged
        {
            add => dataGridView1.StyleChanged += value;
            remove => dataGridView1.StyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler BackgroundImageChanged
        {
            add => dataGridView1.BackgroundImageChanged += value;
            remove => dataGridView1.BackgroundImageChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler BorderStyleChanged
        {
            add => dataGridView1.BorderStyleChanged += value;
            remove => dataGridView1.BorderStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete
        {
            add => dataGridView1.DataBindingComplete += value;
            remove => dataGridView1.DataBindingComplete -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler CurrentCellDirtyStateChanged
        {
            add => dataGridView1.CurrentCellDirtyStateChanged += value;
            remove => dataGridView1.CurrentCellDirtyStateChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler CurrentCellChanged
        {
            add => dataGridView1.CurrentCellChanged += value;
            remove => dataGridView1.CurrentCellChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnWidthChanged
        {
            add => dataGridView1.ColumnWidthChanged += value;
            remove => dataGridView1.ColumnWidthChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnToolTipTextChanged
        {
            add => dataGridView1.ColumnToolTipTextChanged += value;
            remove => dataGridView1.ColumnToolTipTextChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnStateChangedEventHandler ColumnStateChanged
        {
            add => dataGridView1.ColumnStateChanged += value;
            remove => dataGridView1.ColumnStateChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnSortModeChanged
        {
            add => dataGridView1.ColumnSortModeChanged += value;
            remove => dataGridView1.ColumnSortModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnRemoved
        {
            add => dataGridView1.ColumnRemoved += value;
            remove => dataGridView1.ColumnRemoved -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnNameChanged
        {
            add => dataGridView1.ColumnNameChanged += value;
            remove => dataGridView1.ColumnNameChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnMinimumWidthChanged
        {
            add => dataGridView1.ColumnMinimumWidthChanged += value;
            remove => dataGridView1.ColumnMinimumWidthChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewDataErrorEventHandler DataError
        {
            add => dataGridView1.DataError += value;
            remove => dataGridView1.DataError -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnHeaderCellChanged
        {
            add => dataGridView1.ColumnHeaderCellChanged += value;
            remove => dataGridView1.ColumnHeaderCellChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseClick
        {
            add => dataGridView1.ColumnHeaderMouseClick += value;
            remove => dataGridView1.ColumnHeaderMouseClick -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnDividerWidthChanged
        {
            add => dataGridView1.ColumnDividerWidthChanged += value;
            remove => dataGridView1.ColumnDividerWidthChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnDividerDoubleClickEventHandler ColumnDividerDoubleClick
        {
            add => dataGridView1.ColumnDividerDoubleClick += value;
            remove => dataGridView1.ColumnDividerDoubleClick -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnDisplayIndexChanged
        {
            add => dataGridView1.ColumnDisplayIndexChanged += value;
            remove => dataGridView1.ColumnDisplayIndexChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnDefaultCellStyleChanged
        {
            add => dataGridView1.ColumnDefaultCellStyleChanged += value;
            remove => dataGridView1.ColumnDefaultCellStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnDataPropertyNameChanged
        {
            add => dataGridView1.ColumnDataPropertyNameChanged += value;
            remove => dataGridView1.ColumnDataPropertyNameChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnContextMenuStripChanged
        {
            add => dataGridView1.ColumnContextMenuStripChanged += value;
            remove => dataGridView1.ColumnContextMenuStripChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewColumnEventHandler ColumnAdded
        {
            add => dataGridView1.ColumnAdded += value;
            remove => dataGridView1.ColumnAdded -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellValueEventHandler CellValuePushed
        {
            add => dataGridView1.CellValuePushed += value;
            remove => dataGridView1.CellValuePushed -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellValueEventHandler CellValueNeeded
        {
            add => dataGridView1.CellValueNeeded += value;
            remove => dataGridView1.CellValueNeeded -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseDoubleClick
        {
            add => dataGridView1.ColumnHeaderMouseDoubleClick += value;
            remove => dataGridView1.ColumnHeaderMouseDoubleClick -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewCellEventHandler CellValueChanged
        {
            add => dataGridView1.CellValueChanged += value;
            remove => dataGridView1.CellValueChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowEventHandler DefaultValuesNeeded
        {
            add => dataGridView1.DefaultValuesNeeded += value;
            remove => dataGridView1.DefaultValuesNeeded -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewRowEventHandler NewRowNeeded
        {
            add => dataGridView1.NewRowNeeded += value;
            remove => dataGridView1.NewRowNeeded -= value;
        }

        public event DataGridViewRowStateChangedEventHandler RowStateChanged
        {
            add => dataGridView1.RowStateChanged += value;
            remove => dataGridView1.RowStateChanged -= value;
        }

        public event DataGridViewRowsRemovedEventHandler RowsRemoved
        {
            add => dataGridView1.RowsRemoved += value;
            remove => dataGridView1.RowsRemoved -= value;
        }

        public event DataGridViewRowsAddedEventHandler RowsAdded
        {
            add => dataGridView1.RowsAdded += value;
            remove => dataGridView1.RowsAdded -= value;
        }

        public event DataGridViewRowPrePaintEventHandler RowPrePaint
        {
            add => dataGridView1.RowPrePaint += value;
            remove => dataGridView1.RowPrePaint -= value;
        }

        public event DataGridViewRowPostPaintEventHandler RowPostPaint
        {
            add => dataGridView1.RowPostPaint += value;
            remove => dataGridView1.RowPostPaint -= value;
        }

        public event DataGridViewRowEventHandler RowMinimumHeightChanged
        {
            add => dataGridView1.RowMinimumHeightChanged += value;
            remove => dataGridView1.RowMinimumHeightChanged -= value;
        }

        public event DataGridViewCellEventHandler RowLeave
        {
            add => dataGridView1.RowLeave += value;
            remove => dataGridView1.RowLeave -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowHeightInfoPushedEventHandler RowHeightInfoPushed
        {
            add => dataGridView1.RowHeightInfoPushed += value;
            remove => dataGridView1.RowHeightInfoPushed -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowHeightInfoNeededEventHandler RowHeightInfoNeeded
        {
            add => dataGridView1.RowHeightInfoNeeded += value;
            remove => dataGridView1.RowHeightInfoNeeded -= value;
        }

        public event DataGridViewRowEventHandler RowHeightChanged
        {
            add => dataGridView1.RowHeightChanged += value;
            remove => dataGridView1.RowHeightChanged -= value;
        }

        public event DataGridViewEditingControlShowingEventHandler EditingControlShowing
        {
            add => dataGridView1.EditingControlShowing += value;
            remove => dataGridView1.EditingControlShowing -= value;
        }

        public event DataGridViewRowEventHandler RowHeaderCellChanged
        {
            add => dataGridView1.RowHeaderCellChanged += value;
            remove => dataGridView1.RowHeaderCellChanged -= value;
        }

        public event DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            add => dataGridView1.RowHeaderMouseClick += value;
            remove => dataGridView1.RowHeaderMouseClick -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowErrorTextNeededEventHandler RowErrorTextNeeded
        {
            add => dataGridView1.RowErrorTextNeeded += value;
            remove => dataGridView1.RowErrorTextNeeded -= value;
        }

        public event DataGridViewRowEventHandler RowErrorTextChanged
        {
            add => dataGridView1.RowErrorTextChanged += value;
            remove => dataGridView1.RowErrorTextChanged -= value;
        }

        public event DataGridViewCellEventHandler RowEnter
        {
            add => dataGridView1.RowEnter += value;
            remove => dataGridView1.RowEnter -= value;
        }

        public event DataGridViewRowEventHandler RowDividerHeightChanged
        {
            add => dataGridView1.RowDividerHeightChanged += value;
            remove => dataGridView1.RowDividerHeightChanged -= value;
        }

        public event DataGridViewRowDividerDoubleClickEventHandler RowDividerDoubleClick
        {
            add => dataGridView1.RowDividerDoubleClick += value;
            remove => dataGridView1.RowDividerDoubleClick -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event QuestionEventHandler RowDirtyStateNeeded
        {
            add => dataGridView1.RowDirtyStateNeeded += value;
            remove => dataGridView1.RowDirtyStateNeeded -= value;
        }

        public event DataGridViewRowEventHandler RowDefaultCellStyleChanged
        {
            add => dataGridView1.RowDefaultCellStyleChanged += value;
            remove => dataGridView1.RowDefaultCellStyleChanged -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowContextMenuStripNeededEventHandler RowContextMenuStripNeeded
        {
            add => dataGridView1.RowContextMenuStripNeeded += value;
            remove => dataGridView1.RowContextMenuStripNeeded -= value;
        }

        public event DataGridViewRowEventHandler RowContextMenuStripChanged
        {
            add => dataGridView1.RowContextMenuStripChanged += value;
            remove => dataGridView1.RowContextMenuStripChanged -= value;
        }

        public event DataGridViewCellMouseEventHandler RowHeaderMouseDoubleClick
        {
            add => dataGridView1.RowHeaderMouseDoubleClick += value;
            remove => dataGridView1.RowHeaderMouseDoubleClick -= value;
        }

        public event DataGridViewCellValidatingEventHandler CellValidating
        {
            add => dataGridView1.CellValidating += value;
            remove => dataGridView1.CellValidating -= value;
        }

        public event DataGridViewCellEventHandler CellValidated
        {
            add => dataGridView1.CellValidated += value;
            remove => dataGridView1.CellValidated -= value;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellToolTipTextNeededEventHandler CellToolTipTextNeeded
        {
            add => dataGridView1.CellToolTipTextNeeded += value;
            remove => dataGridView1.CellToolTipTextNeeded -= value;
        }

        public event DataGridViewAutoSizeColumnModeEventHandler AutoSizeColumnModeChanged
        {
            add => dataGridView1.AutoSizeColumnModeChanged += value;
            remove => dataGridView1.AutoSizeColumnModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TextChanged
        {
            add => dataGridView1.TextChanged += value;
            remove => dataGridView1.TextChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler RowsDefaultCellStyleChanged
        {
            add => dataGridView1.RowsDefaultCellStyleChanged += value;
            remove => dataGridView1.RowsDefaultCellStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewAutoSizeModeEventHandler RowHeadersWidthSizeModeChanged
        {
            add => dataGridView1.RowHeadersWidthSizeModeChanged += value;
            remove => dataGridView1.RowHeadersWidthSizeModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler RowHeadersWidthChanged
        {
            add => dataGridView1.RowHeadersWidthChanged += value;
            remove => dataGridView1.RowHeadersWidthChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler RowHeadersDefaultCellStyleChanged
        {
            add => dataGridView1.RowHeadersDefaultCellStyleChanged += value;
            remove => dataGridView1.RowHeadersDefaultCellStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler RowHeadersBorderStyleChanged
        {
            add => dataGridView1.RowHeadersBorderStyleChanged += value;
            remove => dataGridView1.RowHeadersBorderStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler ReadOnlyChanged
        {
            add => dataGridView1.ReadOnlyChanged += value;
            remove => dataGridView1.ReadOnlyChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler PaddingChanged
        {
            add => dataGridView1.PaddingChanged += value;
            remove => dataGridView1.PaddingChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler MultiSelectChanged
        {
            add => dataGridView1.MultiSelectChanged += value;
            remove => dataGridView1.MultiSelectChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event QuestionEventHandler CancelRowEdit
        {
            add => dataGridView1.CancelRowEdit += value;
            remove => dataGridView1.CancelRowEdit -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler GridColorChanged
        {
            add => dataGridView1.GridColorChanged += value;
            remove => dataGridView1.GridColorChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler ForeColorChanged
        {
            add => dataGridView1.ForeColorChanged += value;
            remove => dataGridView1.ForeColorChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler EditModeChanged
        {
            add => dataGridView1.EditModeChanged += value;
            remove => dataGridView1.EditModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler DefaultCellStyleChanged
        {
            add => dataGridView1.DefaultCellStyleChanged += value;
            remove => dataGridView1.DefaultCellStyleChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler DataSourceChanged
        {
            add => dataGridView1.DataSourceChanged += value;
            remove => dataGridView1.DataSourceChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler DataMemberChanged
        {
            add => dataGridView1.DataMemberChanged += value;
            remove => dataGridView1.DataMemberChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event DataGridViewAutoSizeModeEventHandler ColumnHeadersHeightSizeModeChanged
        {
            add => dataGridView1.ColumnHeadersHeightSizeModeChanged += value;
            remove => dataGridView1.ColumnHeadersHeightSizeModeChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler ColumnHeadersHeightChanged
        {
            add => dataGridView1.ColumnHeadersHeightChanged += value;
            remove => dataGridView1.ColumnHeadersHeightChanged -= value;
        }

        /// <inheritdoc cref="XDataGridView"/>
        public event EventHandler ColumnHeadersDefaultCellStyleChanged
        {
            add => dataGridView1.ColumnHeadersDefaultCellStyleChanged += value;
            remove => dataGridView1.ColumnHeadersDefaultCellStyleChanged -= value;
        }
        public event EventHandler ColumnHeadersBorderStyleChanged
        {
            add => dataGridView1.ColumnHeadersBorderStyleChanged += value;
            remove => dataGridView1.ColumnHeadersBorderStyleChanged -= value;
        }
        public event EventHandler CellBorderStyleChanged
        {
            add => dataGridView1.CellBorderStyleChanged += value;
            remove => dataGridView1.CellBorderStyleChanged -= value;
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler FontChanged
        {
            add => dataGridView1.FontChanged += value;
            remove => dataGridView1.FontChanged -= value;
        }
        public event DataGridViewCellCancelEventHandler CellBeginEdit
        {
            add => dataGridView1.CellBeginEdit += value;
            remove => dataGridView1.CellBeginEdit -= value;
        }
        public event DataGridViewCellEventHandler CellClick
        {
            add => dataGridView1.CellClick += value;
            remove => dataGridView1.CellClick -= value;
        }
        public event DataGridViewCellEventHandler CellContentClick
        {
            add => dataGridView1.CellContentClick += value;
            remove => dataGridView1.CellContentClick -= value;
        }
        public event DataGridViewCellEventHandler CellToolTipTextChanged
        {
            add => dataGridView1.CellToolTipTextChanged += value;
            remove => dataGridView1.CellToolTipTextChanged -= value;
        }
        public event DataGridViewCellStyleContentChangedEventHandler CellStyleContentChanged
        {
            add => dataGridView1.CellStyleContentChanged += value;
            remove => dataGridView1.CellStyleContentChanged -= value;
        }
        public event DataGridViewCellEventHandler CellStyleChanged
        {
            add => dataGridView1.CellStyleChanged += value;
            remove => dataGridView1.CellStyleChanged -= value;
        }
        public event DataGridViewCellStateChangedEventHandler CellStateChanged
        {
            add => dataGridView1.CellStateChanged += value;
            remove => dataGridView1.CellStateChanged -= value;
        }
        public event DataGridViewCellParsingEventHandler CellParsing
        {
            add => dataGridView1.CellParsing += value;
            remove => dataGridView1.CellParsing -= value;
        }
        public event DataGridViewCellPaintingEventHandler CellPainting
        {
            add => dataGridView1.CellPainting += value;
            remove => dataGridView1.CellPainting -= value;
        }
        public event DataGridViewCellMouseEventHandler CellMouseUp
        {
            add => dataGridView1.CellMouseUp += value;
            remove => dataGridView1.CellMouseUp -= value;
        }
        public event DataGridViewCellMouseEventHandler CellMouseMove
        {
            add => dataGridView1.CellMouseMove += value;
            remove => dataGridView1.CellMouseMove -= value;
        }
        public event DataGridViewCellEventHandler CellMouseLeave
        {
            add => dataGridView1.CellMouseLeave += value;
            remove => dataGridView1.CellMouseLeave -= value;
        }
        public event DataGridViewCellEventHandler CellMouseEnter
        {
            add => dataGridView1.CellMouseEnter += value;
            remove => dataGridView1.CellMouseEnter -= value;
        }
        public event DataGridViewCellMouseEventHandler CellMouseDown
        {
            add => dataGridView1.CellMouseDown += value;
            remove => dataGridView1.CellMouseDown -= value;
        }
        public event DataGridViewCellMouseEventHandler CellMouseDoubleClick
        {
            add => dataGridView1.CellMouseDoubleClick += value;
            remove => dataGridView1.CellMouseDoubleClick -= value;
        }
        public event DataGridViewCellMouseEventHandler CellMouseClick
        {
            add => dataGridView1.CellMouseClick += value;
            remove => dataGridView1.CellMouseClick -= value;
        }
        public event DataGridViewCellEventHandler CellLeave
        {
            add => dataGridView1.CellLeave += value;
            remove => dataGridView1.CellLeave -= value;
        }
        public event DataGridViewCellFormattingEventHandler CellFormatting
        {
            add => dataGridView1.CellFormatting += value;
            remove => dataGridView1.CellFormatting -= value;
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded
        {
            add => dataGridView1.CellErrorTextNeeded += value;
            remove => dataGridView1.CellErrorTextNeeded -= value;
        }
        public event DataGridViewCellEventHandler CellErrorTextChanged
        {
            add => dataGridView1.CellErrorTextChanged += value;
            remove => dataGridView1.CellErrorTextChanged -= value;
        }
        public event DataGridViewCellEventHandler CellEnter
        {
            add => dataGridView1.CellEnter += value;
            remove => dataGridView1.CellEnter -= value;
        }
        public event DataGridViewCellEventHandler CellEndEdit
        {
            add => dataGridView1.CellEndEdit += value;
            remove => dataGridView1.CellEndEdit -= value;
        }
        public event DataGridViewCellEventHandler CellDoubleClick
        {
            add => dataGridView1.CellDoubleClick += value;
            remove => dataGridView1.CellDoubleClick -= value;
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellContextMenuStripNeededEventHandler CellContextMenuStripNeeded
        {
            add => dataGridView1.CellContextMenuStripNeeded += value;
            remove => dataGridView1.CellContextMenuStripNeeded -= value;
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellEventHandler CellContextMenuStripChanged
        {
            add => dataGridView1.CellContextMenuStripChanged += value;
            remove => dataGridView1.CellContextMenuStripChanged -= value;
        }
        public event DataGridViewCellEventHandler CellContentDoubleClick
        {
            add => dataGridView1.CellContentDoubleClick += value;
            remove => dataGridView1.CellContentDoubleClick -= value;
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler BackgroundImageLayoutChanged
        {
            add => dataGridView1.BackgroundImageLayoutChanged += value;
            remove => dataGridView1.BackgroundImageLayoutChanged -= value;
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowEventHandler RowUnshared
        {
            add => dataGridView1.RowUnshared += value;
            remove => dataGridView1.RowUnshared -= value;
        }
        public event DataGridViewCellEventHandler RowValidated
        {
            add => dataGridView1.RowValidated += value;
            remove => dataGridView1.RowValidated -= value;
        }
        public event DataGridViewRowEventHandler UserDeletedRow
        {
            add => dataGridView1.UserDeletedRow += value;
            remove => dataGridView1.UserDeletedRow -= value;
        }
        public event DataGridViewRowEventHandler UserAddedRow
        {
            add => dataGridView1.UserAddedRow += value;
            remove => dataGridView1.UserAddedRow -= value;
        }
        public event DataGridViewRowCancelEventHandler UserDeletingRow
        {
            add => dataGridView1.UserDeletingRow += value;
            remove => dataGridView1.UserDeletingRow -= value;
        }

        #endregion 事件

        #region 方法

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdjustColumnHeaderBorderStyle(
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput,
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool isFirstDisplayedColumn,
            bool isLastVisibleColumn)
        {
            return dataGridView1.AdjustColumnHeaderBorderStyle(dataGridViewAdvancedBorderStyleInput,
                dataGridViewAdvancedBorderStylePlaceholder, isFirstDisplayedColumn, isLastVisibleColumn);
        }

        public bool AreAllCellsSelected(bool includeInvisibleCells)
        {
            return dataGridView1.AreAllCellsSelected(includeInvisibleCells);
        }

        public void AutoResizeColumn(int columnIndex)
        {
            dataGridView1.AutoResizeColumn(columnIndex);
        }

        public void AutoResizeColumn(int columnIndex, DataGridViewAutoSizeColumnMode autoSizeColumnMode)
        {
            dataGridView1.AutoResizeColumn(columnIndex, autoSizeColumnMode);
        }

        public void AutoResizeColumnHeadersHeight(int columnIndex)
        {
            dataGridView1.AutoResizeColumnHeadersHeight(columnIndex);
        }
        public void AutoResizeColumnHeadersHeight()
        {
            dataGridView1.AutoResizeColumnHeadersHeight();
        }

        public void AutoResizeColumns(DataGridViewAutoSizeColumnsMode autoSizeColumnsMode)
        {
            dataGridView1.AutoResizeColumns(autoSizeColumnsMode);
        }

        public void AutoResizeColumns()
        {
            dataGridView1.AutoResizeColumns();
        }

        public void AutoResizeRow(int rowIndex, DataGridViewAutoSizeRowMode autoSizeRowMode)
        {
            dataGridView1.AutoResizeRow(rowIndex, autoSizeRowMode);
        }

        public void AutoResizeRow(int rowIndex)
        {
            dataGridView1.AutoResizeRow(rowIndex);
        }

        public void AutoResizeRowHeadersWidth(int rowIndex, DataGridViewRowHeadersWidthSizeMode rowHeadersWidthSizeMode)
        {
            dataGridView1.AutoResizeRowHeadersWidth(rowIndex, rowHeadersWidthSizeMode);
        }

        public void AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode rowHeadersWidthSizeMode)
        {
            dataGridView1.AutoResizeRowHeadersWidth(rowHeadersWidthSizeMode);
        }

        public void AutoResizeRows()
        {
            dataGridView1.AutoResizeRows();
        }

        public void AutoResizeRows(DataGridViewAutoSizeRowsMode autoSizeRowsMode)
        {
            dataGridView1.AutoResizeRows(autoSizeRowsMode);
        }

        public bool BeginEdit(bool selectAll)
        {
            return dataGridView1.BeginEdit(selectAll);
        }

        public bool CancelEdit()
        {
            return dataGridView1.CancelEdit();
        }

        public void ClearSelection()
        {
            dataGridView1.ClearSelection();
        }

        public bool CommitEdit(DataGridViewDataErrorContexts context)
        {
            return dataGridView1.CommitEdit(context);
        }

        public int DisplayedColumnCount(bool includePartialColumns)
        {
            return dataGridView1.DisplayedColumnCount(includePartialColumns);
        }

        public int DisplayedRowCount(bool includePartialRow)
        {
            return dataGridView1.DisplayedRowCount(includePartialRow);
        }

        public bool EndEdit(DataGridViewDataErrorContexts context)
        {
            return dataGridView1.EndEdit(context);
        }

        public bool EndEdit()
        {
            return dataGridView1.EndEdit();
        }

        public int GetCellCount(DataGridViewElementStates includeFilter)
        {
            return dataGridView1.GetCellCount(includeFilter);
        }

        public Rectangle GetCellDisplayRectangle(int columnIndex, int rowIndex, bool cutOverflow)
        {
            return dataGridView1.GetCellDisplayRectangle(columnIndex, rowIndex, cutOverflow);
        }

        public DataObject GetClipboardContent()
        {
            return dataGridView1.GetClipboardContent();
        }

        public Rectangle GetColumnDisplayRectangle(int columnIndex, bool cutOverflow)
        {
            return dataGridView1.GetColumnDisplayRectangle(columnIndex, cutOverflow);
        }

        public Rectangle GetRowDisplayRectangle(int rowIndex, bool cutOverflow)
        {
            return dataGridView1.GetRowDisplayRectangle(rowIndex, cutOverflow);
        }

        public DataGridView.HitTestInfo HitTest(int x, int y)
        {
            return dataGridView1.HitTest(x, y);
        }

        public void InvalidateCell(int columnIndex, int rowIndex)
        {
            dataGridView1.InvalidateCell(columnIndex, rowIndex);
        }

        public void InvalidateCell(DataGridViewCell dataGridViewCell)
        {
            dataGridView1.InvalidateCell(dataGridViewCell);
        }

        public void InvalidateColumn(int columnIndex)
        {
            dataGridView1.InvalidateColumn(columnIndex);
        }

        public void InvalidateRow(int rowIndex)
        {
            dataGridView1.InvalidateRow(rowIndex);
        }

        public void NotifyCurrentCellDirty(bool dirty)
        {
            dataGridView1.NotifyCurrentCellDirty(dirty);
        }

        public bool RefreshEdit()
        {
            return dataGridView1.RefreshEdit();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetText()
        {
            dataGridView1.ResetText();
        }

        public void SelectAll()
        {
            dataGridView1.SelectAll();
        }

        public void Sort(IComparer comparer)
        {
            dataGridView1.Sort(comparer);
        }

        public void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            dataGridView1.Sort(dataGridViewColumn, direction);
        }

        public void UpdateCellErrorText(int columnIndex, int rowIndex)
        {
            dataGridView1.UpdateCellErrorText(columnIndex, rowIndex);
        }

        public void UpdateCellValue(int columnIndex, int rowIndex)
        {
            dataGridView1.UpdateCellValue(columnIndex, rowIndex);
        }

        public void UpdateRowErrorText(int rowIndexStart, int rowIndexEnd)
        {
            dataGridView1.UpdateRowErrorText(rowIndexStart, rowIndexEnd);
        }

        /// <inheritdoc cref="XDataGridView"/>
        public void UpdateRowErrorText(int rowIndex)
        {
            dataGridView1.UpdateRowErrorText(rowIndex);
        }

        /// <inheritdoc cref="XDataGridView"/>
        public void UpdateRowHeightInfo(int rowIndex, bool updateToEnd)
        {
            dataGridView1.UpdateRowHeightInfo(rowIndex, updateToEnd);
        }

        #endregion 方法

        private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            xScrollBar1.Visible = dataGridView1.RowCount * dataGridView1.RowTemplate.Height + dataGridView1.ColumnHeadersHeight > dataGridView1.DisplayRectangle.Height;
            KeepInnerAndScrollBar();
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            xScrollBar1.Visible = dataGridView1.RowCount * dataGridView1.RowTemplate.Height + dataGridView1.ColumnHeadersHeight > dataGridView1.DisplayRectangle.Height;
            KeepInnerAndScrollBar();
        }

        private void KeepInnerAndScrollBar()
        {
            dataGridView1.Size = Size;

            xScrollBar1.Minimum = 0;
            xScrollBar1.Maximum = dataGridView1.RowCount * dataGridView1.RowTemplate.Height + dataGridView1.ColumnHeadersHeight;
            xScrollBar1.LargeChange = xScrollBar1.Maximum / xScrollBar1.Height + dataGridView1.Height;
            xScrollBar1.SmallChange = 15;
            xScrollBar1.Value = (dataGridView1.FirstDisplayedScrollingRowIndex == -1 ? 0 : dataGridView1.FirstDisplayedScrollingRowIndex) 
                                * dataGridView1.RowTemplate.Height;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            KeepInnerAndScrollBar();
        }

        public void BeginInit()
        {
            ((ISupportInitialize)dataGridView1).BeginInit();
        }

        public void EndInit()
        {
            ((ISupportInitialize)dataGridView1).EndInit();
        }
    }
}
