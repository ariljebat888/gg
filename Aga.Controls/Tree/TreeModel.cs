using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Aga.Controls.Tree
{
	public class TreeModel: ITreeModel
	{
		private TreeNodeAdv _root;
		public TreeNodeAdv Root
		{
			get { return _root; }
		}

		public TreeModel()
		{
			_root = new TreeNodeAdv(null, null);
			_root.IsExpanded = true;
		}

		public virtual IEnumerable GetChildren(object parent)
		{
			TreeNodeAdv node = parent as TreeNodeAdv;
			if (node == null)
				yield break;

			foreach (TreeNodeAdv n in node.Nodes)
				yield return n;
		}

		public virtual bool IsLeaf(object node)
		{
			return (node as TreeNodeAdv).IsLeaf;
		}

		public event EventHandler<TreeModelEventArgs> NodesChanged;
		protected void OnNodesChanged(TreeModelEventArgs args)
		{
			NodesChanged?.Invoke(this, args);
		}

		public event EventHandler<TreeModelEventArgs> NodesInserted;
		protected void OnNodeInserted(TreeNodeAdv parent, int index, TreeNodeAdv node)
		{
			NodesInserted?.Invoke(this, new TreeModelEventArgs(new TreePath(parent.Tag), new int[] { index }, new object[] { node.Tag }));
		}

		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		protected void OnNodeRemoved(TreeNodeAdv parent, int index, TreeNodeAdv node)
		{
			NodesRemoved?.Invoke(this, new TreeModelEventArgs(new TreePath(parent.Tag), new int[] { index }, new object[] { node.Tag }));
		}
	}
}