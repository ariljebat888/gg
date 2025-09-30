using System;
using System.Collections.ObjectModel;

namespace Aga.Controls.Tree
{
	public class TreeNodeAdv
	{
		public object Tag { get; private set; }
		public TreeViewAdv Tree { get; private set; }
		public TreeNodeAdv Parent { get; internal set; }
		public Collection<TreeNodeAdv> Nodes { get; private set; }
		public bool IsExpanded { get; set; }
		public bool IsLeaf { get; internal set; }
		public int Row { get; internal set; }
		public bool IsChildrenLoaded { get; internal set; }

		public bool IsVisible
		{
			get
			{
				TreeNodeAdv node = Parent;
				while (node != null)
				{
					if (!node.IsExpanded) return false;
					node = node.Parent;
				}
				return true;
			}
		}

		public TreeNodeAdv(TreeViewAdv tree, object tag)
		{
			Tree = tree;
			Tag = tag;
			Nodes = new Collection<TreeNodeAdv>();
			if (tree?.Model != null)
				IsLeaf = tree.Model.IsLeaf(Tag);
		}
	}
}