using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Aga.Controls.Tree.NodeControls;

namespace Aga.Controls.Tree
{
	[ToolboxBitmap(typeof(TreeView))]
	public partial class TreeViewAdv : Control
	{
		private const int DRAG_ZONE_WIDTH = 10;
		private const int DRAG_IMAGE_WIDTH = 200;
		private const int DRAG_IMAGE_HEIGHT = 200;

		#region Properties
		
		private bool _autoRowHeight = false;
		[DefaultValue(false), Category("Behavior")]
		public bool AutoRowHeight { get { return _autoRowHeight; } set { _autoRowHeight = value; } }

		private Pen _borderPen;
		[Browsable(false)]
		public Pen BorderPen { get { return _borderPen; } set { _borderPen = value; } }

		private BorderStyle _borderStyle = BorderStyle.FixedSingle;
		[DefaultValue(BorderStyle.FixedSingle), Category("Appearance")]
		public new BorderStyle BorderStyle
		{
			get { return _borderStyle; }
			set { if (_borderStyle != value) { _borderStyle = value; UpdateStyles(); } }
		}

		private IToolTipProvider _defaultToolTipProvider = null;
		[Browsable(false)]
		public IToolTipProvider DefaultToolTipProvider { get { return _defaultToolTipProvider; } set { _defaultToolTipProvider = value; } }

		private bool _displayDraggingImage = true;
		[DefaultValue(true), Category("Drag and Drop")]
		public bool DisplayDraggingImage { get { return _displayDraggingImage; } set { _displayDraggingImage = value; } }

		private bool _dragDropMarkVisible = true;
		[DefaultValue(true), Category("Drag and Drop")]
		public bool DragDropMarkVisible { get { return _dragDropMarkVisible; } set { _dragDropMarkVisible = value; } }

		private DropMarker _dropMarker;
		[Browsable(false)]
		public DropMarker DropMarker { get { return _dropMarker; } }

		public new Font Font { get { return base.Font; } set { base.Font = value; } }
		public new Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }
		public new Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

		private bool _fullRowSelect = false;
		[DefaultValue(false), Category("Behavior")]
		public bool FullRowSelect { get { return _fullRowSelect; } set { _fullRowSelect = value; } }
		
		private bool _useColumns = true;
		[DefaultValue(true), Category("Appearance")]
		public bool UseColumns
		{
			get { return _useColumns; }
			set { if (_useColumns != value) { _useColumns = value; FullUpdate(); Invalidate(); } }
		}

		private bool _hideSelection = true;
		[DefaultValue(true), Category("Behavior")]
		public bool HideSelection { get { return _hideSelection; } set { _hideSelection = value; } }
		
		private int _indent = 19;
		[DefaultValue(19), Category("Behavior")]
		public int Indent { get { return _indent; } set { _indent = value; FullUpdate(); } }

		private ImageList _imageList;
		[DefaultValue(null), Category("Behavior")]
		public ImageList ImageList { get { return _imageList; } set { _imageList = value; } }
		
		private bool _loadOnDemand;
		[DefaultValue(false), Category("Behavior")]
		public bool LoadOnDemand { get { return _loadOnDemand; } set { _loadOnDemand = value; } }

		private ITreeModel _model;
		[Browsable(false)]
		public ITreeModel Model
		{
			get { return _model; }
			set { if (_model != value) { if (_model != null) UnbindModelEvents(); _model = value; CreateNodes(); FullUpdate(); if (_model != null) BindModelEvents(); } }
		}

		private bool _multiselect = false;
		[DefaultValue(false), Category("Behavior")]
		public bool Multiselect { get { return _multiselect; } set { _multiselect = value; } }

		private Pen _linePen;
		[Browsable(false)]
		public Pen LinePen { get { return _linePen; } set { _linePen = value; } }

		private bool _showLines = true;
		[DefaultValue(true), Category("Appearance")]
		public bool ShowLines { get { return _showLines; } set { _showLines = value; Invalidate(); } }

		private bool _showPlusMinus = true;
		[DefaultValue(true), Category("Appearance")]
		public bool ShowPlusMinus { get { return _showPlusMinus; } set { _showPlusMinus = value; FullUpdate(); Invalidate(); } }

		private List<TreeColumn> _columnsList = new List<TreeColumn>();
		private ReadOnlyCollection<TreeColumn> _columns;
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ReadOnlyCollection<TreeColumn> Columns { get { return _columns; } }

		private List<NodeControl> _controlsList = new List<NodeControl>();
		private ReadOnlyCollection<NodeControl> _controls;
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ReadOnlyCollection<NodeControl> NodeControls { get { return _controls; } }

		private TreeNodeAdv _root;
		[Browsable(false)]
		public TreeNodeAdv Root { get { return _root; } }

		private int _rowHeight = 16;
		[DefaultValue(16), Category("Behavior")]
		public int RowHeight { get { return _rowHeight; } set { if (value <= 0) throw new ArgumentOutOfRangeException(); _rowHeight = value; FullUpdate(); } }

		private bool _showNodeToolTips = false;
		[DefaultValue(false), Category("Behavior")]
		public bool ShowNodeToolTips { get { return _showNodeToolTips; } set { _showNodeToolTips = value; } }

		private List<TreeNodeAdv> _selection;
		[Browsable(false)]
		public ReadOnlyCollection<TreeNodeAdv> SelectedNodes
		{
			get
			{
				if (_selection.Count > 0) return new ReadOnlyCollection<TreeNodeAdv>(_selection);
				else if (CurrentNode != null) return new ReadOnlyCollection<TreeNodeAdv>(new TreeNodeAdv[] { CurrentNode });
				else return new ReadOnlyCollection<TreeNodeAdv>(new TreeNodeAdv[0]);
			}
		}

		private TreeNodeAdv _currentNode;
		[Browsable(false)]
		public TreeNodeAdv CurrentNode
		{
			get { return _currentNode; }
			set
			{
				if (_currentNode != value)
				{
					if (value != null && !value.IsVisible) throw new ArgumentException("Node is not visible");
					ClearSelectionInternal();
					_currentNode = value;
					if (value != null)
					{
						_selection.Add(value);
						EnsureVisible(value);
						InvalidateNode(value);
					}
					OnSelectionChanged(new TreeViewEventArgs(new TreeNodeAdv[] { value }, TreeViewAction.ByMouse));
				}
			}
		}

		private HScrollBar _hScrollBar;
		private VScrollBar _vScrollBar;
		private TreeColumn _treeColumn;
		[Browsable(false)]
		public TreeColumn TreeColumn
		{
			get { if (_treeColumn == null && _columns.Count > 0) return _columns[0]; else return _treeColumn; }
			set { _treeColumn = value; }
		}

		private bool _columnsVisible;
		[DefaultValue(false), Category("Appearance")]
		public bool ColumnsVisible { get { return _columnsVisible; } set { _columnsVisible = value; _header.Visible = value; FullUpdate(); } }

		private ColumnHeader _header;
		internal ColumnHeader Header { get { return _header; } }

		internal int OffsetX { get { return _hScrollBar.Value; } }
		internal int FirstVisibleRow { get { return _vScrollBar.Value; } set { _vScrollBar.Value = value; } }
		internal int DisplayRowCount { get; private set; }

		public TreeViewAdv()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			
			_rowMap = new ReadOnlyCollection<TreeNodeAdv>(_rowMapList);
			_selection = new List<TreeNodeAdv>();
			
			_hScrollBar = new HScrollBar { Visible = false };
			_hScrollBar.Scroll += (s, e) => Invalidate();
			Controls.Add(_hScrollBar);

			_vScrollBar = new VScrollBar { Visible = false };
			_vScrollBar.Scroll += (s, e) => Invalidate();
			Controls.Add(_vScrollBar);

			_header = new ColumnHeader(this) { Visible = false };
			
			_columns = new ReadOnlyCollection<TreeColumn>(_columnsList);
			_controls = new ReadOnlyCollection<NodeControl>(_controlsList);
			_borderPen = SystemPens.ControlDark;
			_linePen = SystemPens.ControlDark;
			_dropMarker = new DropMarker();
		}

		protected override void OnSizeChanged(EventArgs e) { UpdateScrollBars(); base.OnSizeChanged(e); }

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);
			int y = _header.Height;
			for (int i = FirstVisibleRow; i < _rowMap.Count && y < Height; i++)
			{
				DrawRow(e, ref y, _rowMap[i]);
			}
			if (ColumnsVisible) _header.Paint(e.Graphics);
		}

		private void DrawRow(PaintEventArgs e, ref int y, TreeNodeAdv node)
		{
			Rectangle rowRect = new Rectangle(0, y, ClientRectangle.Width, RowHeight - 1);
			if (FullRowSelect && IsSelected(node))
			{
				e.Graphics.FillRectangle(Focused ? SystemBrushes.Highlight : SystemBrushes.Control, rowRect);
			}

			Rectangle rect = new Rectangle(-OffsetX, y, 0, RowHeight);
			foreach (TreeColumn col in Columns)
			{
				if (col.IsVisible)
				{
					rect.Width = col.Width;
					foreach (NodeControl nc in _controlsList)
					{
						if (nc.ParentColumn == col)
						{
							var context = new DrawContext
							{
								Graphics = e.Graphics, Font = Font, Bounds = rect,
								IsSelected = IsSelected(node), IsCurrent = (node == CurrentNode),
								DrawFocus = Focused, ToolTipProvider = DefaultToolTipProvider
							};
							nc.Draw(node, context);
						}
					}
					rect.X += rect.Width;
				}
			}
			y += RowHeight;
		}

		private void CreateNodes() { _root = new TreeNodeAdv(this, null) { IsExpanded = true }; }
		private void BindModelEvents() { if (_model == null) return; _model.NodesChanged += Model_NodesChanged; _model.NodesInserted += Model_NodesInserted; _model.NodesRemoved += Model_NodesRemoved; }
		private void UnbindModelEvents() { if (_model == null) return; _model.NodesChanged -= Model_NodesChanged; _model.NodesInserted -= Model_NodesInserted; _model.NodesRemoved -= Model_NodesRemoved; }
		private void Model_NodesChanged(object s, TreeModelEventArgs e) { Invalidate(); }
		private void Model_NodesInserted(object s, TreeModelEventArgs e) { FullUpdate(); }
		private void Model_NodesRemoved(object s, TreeModelEventArgs e) { FullUpdate(); }

		public TreeNodeAdv FindNode(TreePath path)
		{
			if (path == null || path.IsEmpty()) return _root;
			return FindNode(_root, path, 0);
		}

		private TreeNodeAdv FindNode(TreeNodeAdv root, TreePath path, int level)
		{
			if (root.Tag != path.FullPath[level]) return null;
			if (level == path.FullPath.Length - 1) return root;
			foreach (TreeNodeAdv node in root.Nodes)
			{
				var res = FindNode(node, path, level + 1);
				if (res != null) return res;
			}
			return null;
		}

		private void ClearSelectionInternal()
		{
			foreach (var n in _selection) InvalidateNode(n);
			_selection.Clear();
		}

		private int GetContentWidth() { int w = 0; foreach (var c in Columns) if(c.IsVisible) w+=c.Width; return w; }
		public void InvalidateNode(TreeNodeAdv node) { if(node.IsVisible) Invalidate(GetNodeBounds(node)); }
		private Rectangle GetNodeBounds(TreeNodeAdv node) { int y = _header.Height + (_rowMap.IndexOf(node) - FirstVisibleRow) * RowHeight; return new Rectangle(0, y, ClientRectangle.Width, RowHeight); }
		public bool IsSelected(TreeNodeAdv node) { return _selection.Contains(node); }
		public void EnsureVisible(TreeNodeAdv node)
		{
			if (node.IsVisible)
			{
				int row = _rowMap.IndexOf(node);
				if (row < FirstVisibleRow) FirstVisibleRow = row;
				else if (row >= FirstVisibleRow + DisplayRowCount) FirstVisibleRow = row - DisplayRowCount + 1;
			}
		}

		private List<TreeNodeAdv> _rowMapList = new List<TreeNodeAdv>();
		private ReadOnlyCollection<TreeNodeAdv> _rowMap;

		private void FullUpdate()
		{
			_rowMapList.Clear();
			if (Root != null) BuildRowMap(Root);
			UpdateScrollBars();
			Invalidate();
		}

		private void BuildRowMap(TreeNodeAdv node)
		{
			_rowMapList.Add(node);
			if (node.IsExpanded) foreach (TreeNodeAdv n in node.Nodes) BuildRowMap(n);
		}

		private void UpdateScrollBars()
		{
			Rectangle clientRect = ClientRectangle;
			DisplayRowCount = clientRect.Height > _header.Height ? (clientRect.Height - _header.Height) / RowHeight : 0;
			
			bool vscrollVisible = (_rowMap.Count > DisplayRowCount);
			_vScrollBar.Visible = vscrollVisible;
			if(vscrollVisible)
			{
				_vScrollBar.Bounds = new Rectangle(clientRect.Right - _vScrollBar.Width, _header.Height, _vScrollBar.Width, clientRect.Height - _header.Height);
				_vScrollBar.Maximum = _rowMap.Count - 1;
				_vScrollBar.LargeChange = DisplayRowCount;
			}

			int contentWidth = GetContentWidth();
			bool hscrollVisible = (contentWidth > clientRect.Width);
			_hScrollBar.Visible = hscrollVisible;
			if(hscrollVisible)
			{
				_hScrollBar.Bounds = new Rectangle(0, clientRect.Bottom - _hScrollBar.Height, clientRect.Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0), _hScrollBar.Height);
				_hScrollBar.Maximum = contentWidth;
				_hScrollBar.LargeChange = clientRect.Width;
			}
		}

		protected void OnSelectionChanged(TreeViewEventArgs args) { SelectionChanged?.Invoke(this, args); }
		public event EventHandler<TreeViewEventArgs> SelectionChanged;
		public void DisplayEditor(Control editor, IEditor owner) {}
		public void HideEditor(bool cancel) {}
		#endregion
	}
}