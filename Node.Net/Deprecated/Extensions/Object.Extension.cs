using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated
{
    public static class ObjectExtension
    {
        public static object GetPropertyValue(this object item,string propertyName)
        {
            if(item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if(propertyInfo != null)
                {
                    return propertyInfo.GetValue(item);
                }
            }
            return null;
        }
        public static T GetPropertyValue<T>(this object item,string propertyName)
        {
            return (T)GetPropertyValue(item, propertyName);
        }
        public static void SetPropertyValue(this object item,string propertyName,object propertyValue)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(item, propertyValue);
                }
            }
        }
    }
}
