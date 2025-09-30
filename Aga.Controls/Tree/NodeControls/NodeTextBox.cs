using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Aga.Controls.Tree.NodeControls
{
	public class NodeTextBox : EditableControl
	{
		// Add this property to fix CS0103
		public string DataMember { get; set; }

		public override void Draw(TreeNodeAdv node, DrawContext context) { } // Not implemented
		public override Size GetActualSize(TreeNodeAdv node, Font font) { return Size.Empty; } // Not implemented

		protected override Control CreateEditor(TreeNodeAdv node)
		{
			var textBox = new TextBox { BorderStyle = BorderStyle.FixedSingle };
			// Set TextAlign if you have a property for it, e.g. this.TextAlign
			// textBox.TextAlign = this.TextAlign; // Uncomment if such a property exists

			textBox.KeyDown += (s, e) => {
				if (e.KeyCode == Keys.Enter) EndEdit(false);
				else if (e.KeyCode == Keys.Escape) EndEdit(true);
			};
			return textBox;
		}
		public override object GetValue(Control control) { return (control as TextBox)?.Text; }
		public override void SetValue(TreeNodeAdv node, object value)
		{
			if (!string.IsNullOrEmpty(DataMember))
			{
				PropertyInfo pi = node.Tag.GetType().GetProperty(DataMember);
				pi?.SetValue(node.Tag, value, null);
			}
		}
		public override void Update(Control control, TreeNodeAdv node)
		{
            (control as TextBox).Text = GetValue(control) != null ? GetValue(control).ToString() : string.Empty;
        }
	}
}