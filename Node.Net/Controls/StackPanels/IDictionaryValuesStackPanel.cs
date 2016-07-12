using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Controls.StackPanels
{
    public class IDictionaryValuesStackPanel : LabelsStackPanel
    {
        protected override string[] GetValues()
        {
            var values = new List<string>();
            var data = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if (data != null)
            {
                foreach (var key in data.Keys)
                {
                    if(!data.IsChildKey(key))
                    {
                        values.Add(data[key].ToString());
                    }
                }
            }
            return values.ToArray();
        }
    }
}
