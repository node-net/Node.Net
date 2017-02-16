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
        public int IntegerValue { get; set; } = 9;
        public int IntegerCopy => IntegerValue;
        [System.ComponentModel.TypeConverter(typeof(TreeConverter))]
        public string Tree { get; set; } = "aspen";
        public System.Drawing.Color BackgroundColor { get; set; } = new System.Drawing.Color();
        public System.Drawing.Font Font { get; set; } = new System.Drawing.Font("Arial", 12);
    }
    [TestFixture,NUnit.Framework.Category(nameof(View))]
    class Properties_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Properties_Usage()
        {
            var properties = new Deprecated.Controls.PropertyControl
            {
                DataContext = new Widget()
            };
            var window = new System.Windows.Window
            {
                Content = properties,
                Title = nameof(Properties_Usage)
            };
            window.ShowDialog();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Properties_Usage_Many_Properties()
        {
            var hash = new Node.Net.Deprecated.Collections.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }
            var properties = new Deprecated.Controls.PropertyControl
            {
                DataContext = hash
            };
            var window = new System.Windows.Window
            {
                Content = properties,
                Title = nameof(Properties_Usage)
            };
            window.ShowDialog();
        }
    }
}
