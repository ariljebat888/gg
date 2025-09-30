using System.Drawing;

namespace Aga.Controls.Tree.NodeControls
{
	public class DrawContext
	{
		public Graphics Graphics { get; set; }
		public Rectangle Bounds { get; set; }
		public Font Font { get; set; }
		public bool IsSelected { get; set; }
		public bool IsCurrent { get; set; }
		public bool DrawFocus { get; set; }
		public IToolTipProvider ToolTipProvider { get; set; }
	}
}