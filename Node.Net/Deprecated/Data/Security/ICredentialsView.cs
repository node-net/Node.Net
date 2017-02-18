using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Data.Deprecated.Security
{
    public class ICredentialsView : Grid
    {
        public ICredentialsView()
        {
            DataContextChanged += Update;
        }


        private void Update(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
