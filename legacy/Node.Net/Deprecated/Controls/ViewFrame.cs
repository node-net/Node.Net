using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Node.Net.Deprecated.Controls
{
    public class ViewFrame : Frame
    {
        public ViewFrame()
        {
            DataContextChanged += ViewFrame_DataContextChanged;
            JournalOwnership = JournalOwnership.UsesParentJournal;
        }

        public Dictionary<string, FrameworkElement> Views = new Dictionary<string, FrameworkElement>();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        private void ViewFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();
        private void Update()
        {
            if (Views.Count == 0) Content = null;
            else
            {
                if(Views.Count == 1)
                {
                    var viewKey = Views.Keys.First();
                    Content = Views[viewKey];
                }
                if(Views.Count > 1)
                {
                    var tabControl = new TabControl();
                    foreach(var key in Views.Keys)
                    {
                        var view = Views[key];
                        tabControl.Items.Add(new TabItem { Header = key.ToString(), Content = view });
                    }
                    Content = tabControl;
                }
            }

            foreach(var key in Views.Keys)
            {
                Views[key].DataContext = DataContext;
            }
        }
    }
}
