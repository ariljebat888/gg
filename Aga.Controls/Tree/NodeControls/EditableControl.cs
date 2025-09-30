using System.Windows.Forms;

namespace Aga.Controls.Tree.NodeControls
{
	public interface IEditor
	{
		Control Editor { get; }
		void Update(Control control, TreeNodeAdv node);
		void SetValue(TreeNodeAdv node, object value);
		object GetValue(Control control);
		void EndEdit(bool cancel);
	}

	public abstract class EditableControl : NodeControl, IEditor
	{
		private Control _editControl;
		public virtual Control Editor { get { return _editControl; } }
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args) { BeginEdit(args.Node); }
		public bool BeginEdit(TreeNodeAdv node)
		{
			_editControl = CreateEditor(node);
			if (_editControl != null)
			{
				Update(_editControl, node);
				Parent.DisplayEditor(_editControl, this);
				return true;
			}
			return false;
		}
		public void EndEdit(bool cancel)
		{
			if (!cancel && Parent.CurrentNode != null)
			{
				SetValue(Parent.CurrentNode, GetValue(_editControl));
			}
			Parent.HideEditor(false);
		}
		
		protected abstract Control CreateEditor(TreeNodeAdv node);
		public abstract void SetValue(TreeNodeAdv node, object value);
		public abstract void Update(Control control, TreeNodeAdv node);
		public abstract object GetValue(Control control);
	}
}