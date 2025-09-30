using System;
using System.Collections;

namespace Aga.Controls.Tree
{
	public interface ITreeModel
	{
		IEnumerable GetChildren(object parent);
		bool IsLeaf(object node);

		event EventHandler<TreeModelEventArgs> NodesChanged;
		event EventHandler<TreeModelEventArgs> NodesInserted;
		event EventHandler<TreeModelEventArgs> NodesRemoved;
	}
}