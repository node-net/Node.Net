using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Node.Net.Controls
{
    public class ViewTester
    {
        public static void ShowDialog(object models, FrameworkElement view)
        {
            System.Windows.Window window = new System.Windows.Window()
            {
                Title = GetTitle(view),
                Content = view,
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        private static string GetTitle(FrameworkElement frameworkelement)
        {
            PropertyInfo titleInfo = frameworkelement.GetType().GetProperty("Title");
            if (!object.ReferenceEquals(null, titleInfo))
            {
                return titleInfo.GetValue(frameworkelement).ToString();
            }
            return frameworkelement.GetType().FullName;
        }
    }
}
