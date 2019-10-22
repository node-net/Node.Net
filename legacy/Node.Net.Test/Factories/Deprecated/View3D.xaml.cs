using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Node.Net.Factories.Deprecated;

namespace Node.Net.Factories.Test
{
    public class View3DProperties
    {

    }
    /// <summary>
    /// Interaction logic for View3D.xaml
    /// </summary>
    public partial class View3D : UserControl
    {
        public View3D()
        {
            DataContextChanged += View3D_DataContextChanged;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            SetDataContext(dataContextComboBox, GetDataContexts());
            SetDataContext(factoryComboBox, GetFactories());
        }

        private void View3D_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();

        private void Update()
        {
            viewport.Children.Clear();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());

            var cbi = factoryComboBox.SelectedItem as ComboBoxItem;
            if (cbi != null)
            {
                var factory = cbi.DataContext as IFactory;
                if (factory != null)
                {
                    cbi = dataContextComboBox.SelectedItem as ComboBoxItem;
                    if (cbi != null)
                    {
                        var v3d = factory.Create(typeof(Visual3D),cbi.DataContext) as Visual3D;
                        if (v3d != null) viewport.Children.Add(v3d);
                    }
                }
            }
        }

        private void dataContextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();
        private void factoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();
        private static IDictionary GetDataContexts() => GlobalFixture.GetDictionaryModels();

        private static IDictionary GetFactories()
        {
            var factories = new Dictionary<string, IFactory>();

            /*
            var minecraft = new Factory();
            minecraft.Add("Default", new Node.Net.Factories.Factories.DefaultFactory());
            var resources = new Node.Net.Factories.Factories.ManifestResourceFactory(typeof(View3D).Assembly);
            resources.ExcludePatterns.Add(".json");
            minecraft.Add("Resources", resources);
            factories.Add("Minecraft", minecraft);*/
            return factories;
        }

        public static object ReadFunction(Stream stream)
        {
            if (stream.GetName().Contains(".json"))
            {
                var reader = new Deprecated.Internal.JsonReader();
                return reader.Read(stream);
            }
            return XamlReader.Load(stream);
        }
        private static void SetDataContext(ComboBox comboBox,object dataContext)
        {
            comboBox.Items.Clear();
            var dictionary = dataContext as IDictionary;
            foreach(string key in dictionary.Keys)
            {
                comboBox.Items.Add(new ComboBoxItem { Content = key, DataContext = dictionary[key] });
            }
            if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
        }

        private void PlanViewButton_Click(object sender, RoutedEventArgs e)
        {
            viewport.Camera = new OrthographicCamera
            {
                LookDirection = new Vector3D(0, 0, -1),
                Position = new Point3D(0, 0, 50),
                UpDirection = new Vector3D(0, 1, 0)
            };
            viewport.ZoomExtents();
        }
    }
}
