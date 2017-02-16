using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Node.Net.Controls.Test
{
    /// <summary>
    /// Interaction logic for ControlTester.xaml
    /// </summary>
    public partial class ControlTester : UserControl
    {
        public static void ShowDialog(Assembly assembly = null)
        {
            if (assembly == null) assembly = typeof(Factory).Assembly;
            var window = new Window
            {
                Title = "ControlTester",
                Content = new ControlTester { DataContext = GlobalFixture.Projects, Assembly = assembly }
            };
            window.ShowDialog();
        }

        public static void ShowCoverage()
        {
            var projects = GlobalFixture.Projects;
            foreach (string name in projects.Keys)
            {
                var project = projects[name];
                foreach (var type in typeof(Factory).Assembly.GetTypes())
                {
                    if (typeof(FrameworkElement).IsAssignableFrom(type))
                    {
                        if (type != typeof(Node.Net.Controls.Forms.PropertyGrid))
                        {
                            var frameworkElement = Activator.CreateInstance(type) as FrameworkElement;
                            frameworkElement.DataContext = project;
                            var window = new Window { Content = frameworkElement };
                            window.Show();
                        }
                    }
                }

            }
        }
        public ControlTester()
        {
            DataContextChanged += _DataContextChanged;
            InitializeComponent();
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            dataContextComboBox.Items.Clear();
            Update();
        }

        private Assembly _assembly;
        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                _assembly = value;
                Update();
            }
        }
        private string CurrentDataContextName { get; set; }
        private string CurrentFrameworkElementName { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }

        private void Update()
        {
            UpdateDataContextComboBox();
            UpdateFrameworkElementComboBox();
            object dataContext = null;
            if(dataContextComboBox.SelectedItem != null)
            {
                CurrentDataContextName = (dataContextComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                //dataContext = (DataContext as IDictionary)[CurrentDataContextName];
                dataContext = new KeyValuePair<string, dynamic>(CurrentDataContextName, (DataContext as IDictionary)[CurrentDataContextName]);
            }
            FrameworkElement frameworkElement = null;
            if(frameworkElementComboBox.SelectedItem != null)
            {
                CurrentFrameworkElementName = (frameworkElementComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                frameworkElement = Activator.CreateInstance((frameworkElementComboBox.SelectedItem as ComboBoxItem).DataContext as Type) as FrameworkElement;
                frameworkElement.DataContext = dataContext;
            }
            frame.Content = frameworkElement;
            frame.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        private void UpdateDataContextComboBox()
        {
            if (dataContextComboBox.Items.Count == 0)
            {
                var dictionary = DataContext as IDictionary;
                if (dictionary != null)
                {
                    foreach (string name in dictionary.Keys)
                    {
                        dataContextComboBox.Items.Add(new ComboBoxItem { Content = name, DataContext = dictionary[name] });
                    }
                    if (dataContextComboBox.Items.Count > 0 && dataContextComboBox.SelectedIndex == -1)
                    {
                        dataContextComboBox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UpdateFrameworkElementComboBox()
        {
            if (frameworkElementComboBox.Items.Count == 0)
            {
                if (Assembly != null)
                {
                    foreach (var type in Assembly.GetTypes())
                    {
                        if (typeof(FrameworkElement).IsAssignableFrom(type))
                        {
                            frameworkElementComboBox.Items.Add(new ComboBoxItem { Content = type.FullName, DataContext = type });
                        }
                    }
                    if (frameworkElementComboBox.Items.Count > 0 && frameworkElementComboBox.SelectedIndex == -1)
                    {
                        frameworkElementComboBox.SelectedIndex = 0;
                    }
                }
            }
        }
        private void dataContextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = sender as ComboBoxItem;
            if(comboBoxItem !=null)
            {
                CurrentDataContextName = comboBoxItem.Content.ToString();
            }
            Update();
        }

        private void frameworkElementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = sender as ComboBoxItem;
            if (comboBoxItem != null)
            {
                CurrentFrameworkElementName = comboBoxItem.Content.ToString();
            }
            Update();
        }
    }
}
