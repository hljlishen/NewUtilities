using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities.Forms
{
    public class ExclusiveMenuItemManager
    {
        public string Group { get; private set; } = "default";
        private static Dictionary<string, ExclusiveMenuItemManager> groupDictionary = new Dictionary<string, ExclusiveMenuItemManager>();
        private Dictionary<ToolStripMenuItem, Action> actionMap = new Dictionary<ToolStripMenuItem, Action>();
        private List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

        private ExclusiveMenuItemManager(string group = "default")
        {
            Group = group;
        }

        public static ExclusiveMenuItemManager GetInstance(string group = "default")
        {
            if (!groupDictionary.ContainsKey(group))
                groupDictionary.Add(group, new ExclusiveMenuItemManager());
            return groupDictionary[group];
        }

        public void Add(ToolStripMenuItem c, bool checkState = false, Action action = null)
        {
            if (items.Contains(c))
                return;
            c.CheckOnClick = true;
            c.Checked = checkState;
            if (c.Checked)
                action?.Invoke();
            items.Add(c);
            actionMap.Add(c, action);
            c.Click += C_Click;
        }

        private void C_Click(object sender, EventArgs e)
        {
            var i = sender as ToolStripMenuItem;
            i.Enabled = false;
            foreach (var item in items)
            {
                if (ReferenceEquals(sender, item))
                {
                    actionMap[item]?.Invoke();  //调用回调函数
                    continue;
                }

                item.Enabled = true;
                item.Checked = false;
            }
        }
    }
}
