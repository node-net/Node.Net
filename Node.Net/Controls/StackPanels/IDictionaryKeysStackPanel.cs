using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Controls.StackPanels
{
    public class IDictionaryKeysStackPanel : LabelsStackPanel
    {
        protected override string[] GetValues()
        {
            var values = new List<string>();
            var data = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if (data != null)
            {
                foreach (var key in data.Keys)
                {
                    values.Add(key.ToString());
                }
            }
            return values.ToArray();
        }
    }
}
