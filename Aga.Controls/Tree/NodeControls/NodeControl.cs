using System.ComponentModel;
using System.Drawing;

namespace Aga.Controls.Tree.NodeControls
{
	public abstract class NodeControl : Component
	{
		[Browsable(false)] public TreeColumn ParentColumn { get; internal set; }
		[Browsable(false)] public TreeViewAdv Parent { get; internal set; }

		public abstract void Draw(TreeNodeAdv node, DrawContext context);
		public abstract Size GetActualSize(TreeNodeAdv node, Font font);
		public virtual void MouseDown(TreeNodeAdvMouseEventArgs args) { }
		public virtual void MouseUp(TreeNodeAdvMouseEventArgs args) { }
		public virtual void MouseDoubleClick(TreeNodeAdvMouseEventArgs args) { }
	}
}