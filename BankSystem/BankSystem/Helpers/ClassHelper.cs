using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BankSystem.Helpers
{
    public class ClassHelper
    {
        public static object GetProperty(IDictionary<string, object> dictionary, string propertyName)
        {
            foreach (var item in dictionary)
            {
                if (item.Key.ToLower() == propertyName)
                {
                    return item.Value;
                }
                else if (item.Value.GetType().GetTypeInfo().IsClass)
                {
                    var properties = GetProperties(item.Value);

                    foreach (var prop in properties)
                    {
                        if (prop.Name.ToLower() == propertyName)
                        {
                            return prop.GetValue(item.Value, null);
                        }
                    }
                }
            }

            return null;
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
