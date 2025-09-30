using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Aga.Controls.Tree.NodeControls
{
	public abstract class BaseTextControl : NodeControl
	{
		[Category("Appearance")] public Font Font { get; set; }
		[DefaultValue(HorizontalAlignment.Left)] public HorizontalAlignment TextAlign { get; set; }
		[DefaultValue(StringTrimming.None)] public StringTrimming Trimming { get; set; }
		[DefaultValue(null)] public string DataMember { get; set; }

		protected virtual object GetValue(TreeNodeAdv node)
		{
			if (!string.IsNullOrEmpty(DataMember))
			{
				PropertyInfo pi = node.Tag.GetType().GetProperty(DataMember);
				if (pi != null) return pi.GetValue(node.Tag, null);
			}
			return node.Tag;
		}

		public override Size GetActualSize(TreeNodeAdv node, Font font)
		{
			string text = GetValue(node).ToString();
			return TextRenderer.MeasureText(text, font);
		}

		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			string text = GetValue(node).ToString();
			Rectangle bounds = context.Bounds;
			Font font = Font ?? context.Font;
			
			TextFormatFlags flags = TextFormatFlags.VerticalCenter;
			switch (TextAlign)
			{
				case HorizontalAlignment.Center: flags |= TextFormatFlags.HorizontalCenter; break;
				case HorizontalAlignment.Right: flags |= TextFormatFlags.Right; break;
			}
			if (Trimming != StringTrimming.None)
				flags |= TextFormatFlags.EndEllipsis;

			Color color = (context.IsSelected && context.DrawFocus) ? SystemColors.HighlightText : SystemColors.WindowText;
			TextRenderer.DrawText(context.Graphics, text, font, bounds, color, flags);
		}
	}
}