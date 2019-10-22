using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Data.Security
{
    public class CredentialsList : ListBox
    {
        public CredentialsList()
        {
            DataContextChanged += _DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Items.Clear();
            var dictionary = DataContext as IDictionary;
            if (dictionary != null)
            {
                foreach(var key in dictionary.Keys)
                {
                    var credential = dictionary[key] as ICredential;
                    if(credential != null)
                    {
                        Items.Add(new ListBoxItem { DataContext = credential, Content = $"{credential.Domain} - {credential.UserName}" });
                    }
                }
            }
        }
    }
}
