using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Aga.Controls.Tree
{
    public class TreePath
    {
        public object[] FullPath { get; private set; }
        public bool IsEmpty() { return FullPath == null || FullPath.Length == 0; }
        public TreePath(object node) { FullPath = new object[] { node }; }
        public TreePath(object[] path) { FullPath = path; }
    }

    public class TreeModelEventArgs : EventArgs
    {
        public TreePath Parent { get; private set; }
        public int[] Indices { get; private set; }
        public object[] Children { get; private set; }
        public TreeModelEventArgs(TreePath parent, int[] indices, object[] children)
        {
            Parent = parent;
            Indices = indices;
            Children = children;
        }
    }
    
    public enum TreeViewAction 
    { 
        Unknown, ByMouse, ByKeyboard 
    }

    public class TreeViewEventArgs : EventArgs
    {
        public TreeNodeAdv[] Nodes { get; private set; }
        public TreeViewAction Action { get; private set; }
        public TreeViewEventArgs(TreeNodeAdv[] nodes, TreeViewAction action)
        {
            Nodes = nodes;
            Action = action;
        }
    }

    public class TreeNodeAdvMouseEventArgs : MouseEventArgs
    {
        public TreeNodeAdv Node { get; private set; }
        public TreeNodeAdvMouseEventArgs(TreeNodeAdv node, MouseButtons button, int clicks, int x, int y, int delta)
            : base(button, clicks, x, y, delta)
        {
            Node = node;
        }
    }

    public interface IToolTipProvider
    {
        string GetToolTip(TreeNodeAdv node);
    }

    public class DropMarker
    {
        // Placeholder class
    }

    public class ColumnHeader : Control
    {
        private TreeViewAdv _tree;
        public ColumnHeader(TreeViewAdv tree) { _tree = tree; }
        public new void Paint(Graphics g) 
        {
            if(Visible)
            {
                g.FillRectangle(SystemBrushes.Control, this.Bounds);
                // Simple header drawing logic
                int x = 0;
                foreach(var col in _tree.Columns)
                {
                    if (col.IsVisible)
                    {
                        Rectangle rect = new Rectangle(x, 0, col.Width, this.Height);
                        g.DrawRectangle(SystemPens.ControlDark, rect);
                        TextRenderer.DrawText(g, col.Header, this.Font, rect, SystemColors.ControlText, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                        x += col.Width;
                    }
                }
            }
        }
    }
}