using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Controls
{
    public class TitleControl : System.Windows.Controls.UserControl
    {
        public TitleControl()
        {
            DataContextChanged += Title_DataContextChanged;
        }

        void Title_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (!ReferenceEquals(null, Node.Net.Collections.KeyValuePair.GetValue(DataContext)))
            {
                var label = new System.Windows.Controls.Label
                {
                    Content = Node.Net.Collections.KeyValuePair.GetValue(DataContext).GetType().Name
                };
                Content = label;
            }
        }
    }
}
