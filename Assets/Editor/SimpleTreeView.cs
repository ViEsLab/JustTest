using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor {
    public class CustomTreeItem : TreeViewItem {
        public float height = 50;

        public CustomTreeItem() {
            height = EditorGUIUtility.singleLineHeight;
        }
    }

    class SimpleTreeView : TreeView
    {
        public SimpleTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot ()
        {
            var root = new CustomTreeItem {id = 0, depth = -1, displayName = "Root"};
            var allItems = new List<TreeViewItem>
            {
                new CustomTreeItem {id = 1, depth = 0, displayName = "A", height = 10},
                new CustomTreeItem {id = 2, depth = 1, displayName = "B", height = 20},
                new CustomTreeItem {id = 3, depth = 2, displayName = "C", height = 30},
                new CustomTreeItem {id = 4, depth = 2, displayName = "D", height = 40},
                new CustomTreeItem {id = 5, depth = 2, displayName = "E", height = 50},
                new CustomTreeItem {id = 6, depth = 2, displayName = "F", height = 60},
                new CustomTreeItem {id = 7, depth = 1, displayName = "G", height = 70},
                new CustomTreeItem {id = 8, depth = 2, displayName = "H", height = 80},
                new CustomTreeItem {id = 9, depth = 2, displayName = "I", height = 90},
            };

            SetupParentsAndChildrenFromDepths (root, allItems);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args) {
            CustomTreeItem item = (CustomTreeItem)args.item;

            base.RowGUI(args);
        }
    }
}