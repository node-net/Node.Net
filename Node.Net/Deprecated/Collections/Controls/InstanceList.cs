using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections.Controls
{
    public class InstanceList : System.Windows.Controls.Frame
    {
        public InstanceList()
        {
            DataContextChanged += _DataContextChanged;
            JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        public bool Searchable { get; set; } = true;
        public string Key { get; set; } = "Type";
        public string ValuePattern { get; set; } = "";
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private System.Windows.Controls.StackPanel stackPanel = null;
        private void Update()
        {
            Content = new System.Windows.Controls.ScrollViewer
            {
                Content = new InstanceStackPanel { DataContext = DataContext },
                VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto
            };
        }
    }
}
