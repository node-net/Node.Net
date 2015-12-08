using NUnit.Framework;

namespace Node.Net.View
{
    class TreeConverter : System.ComponentModel.StringConverter
    {
        public override StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) => new StandardValuesCollection(new string[]{"aspen",
                                                     "maple",
                                                     "oak"});
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) => true;
    }
    class Widget
    {
        private int integerValue = 9;
        public int IntegerValue
        {
            get { return integerValue; }
            set { integerValue = value; }
        }
        public int IntegerCopy => integerValue;

        private string tree = "aspen";
        [System.ComponentModel.TypeConverter(typeof(TreeConverter))]
        public string Tree
        {
            get { return tree; }
            set { tree = value; }
        }
        private System.Drawing.Color backgroundColor = new System.Drawing.Color();
        public System.Drawing.Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
        public System.Drawing.Font Font
        {
            get { return font; }
            set { font = value; }
        }
    }
    [TestFixture,NUnit.Framework.Category("View")]
    class Properties_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Properties_Usage()
        {
            Properties properties = new Properties();
            properties.DataContext = new Widget();
            System.Windows.Window window = new System.Windows.Window();
            window.Content = properties;
            window.Title = "Properties_Usage";
            window.ShowDialog();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Properties_Usage_Many_Properties()
        {
            Node.Net.Json.Hash hash = new Node.Net.Json.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }
            Properties properties = new Properties();
            properties.DataContext = hash;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = properties;
            window.Title = "Properties_Usage";
            window.ShowDialog();
        }
    }
}
