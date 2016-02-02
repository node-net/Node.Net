

namespace Node.Net.Controls
{
    public class XmlTreeViewItem : TreeViewItem
    {
        public XmlTreeViewItem() : base(null) { }
        public XmlTreeViewItem(object value) : base(value) { }
        public XmlTreeViewItem(object model, int childDepth) : base(model, childDepth)
        {
        }
    }
}
