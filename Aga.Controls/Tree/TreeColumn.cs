using System.ComponentModel;
using System.Windows.Forms;

namespace Aga.Controls.Tree
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class TreeColumn
	{
		[DefaultValue(""), Localizable(true)] public string Header { get; set; }
		[DefaultValue(HorizontalAlignment.Left)] public HorizontalAlignment TextAlign { get; set; }
		[DefaultValue(50), Localizable(true)] public int Width { get; set; }
		[DefaultValue(true)] public bool IsVisible { get; set; }

		public TreeColumn()
		{
			Header = "";
			Width = 50;
			IsVisible = true;
			TextAlign = HorizontalAlignment.Left;
		}
		public TreeColumn(string header, int width) : this() { Header = header; Width = width; }
		public override string ToString() { return Header; }
	}
}