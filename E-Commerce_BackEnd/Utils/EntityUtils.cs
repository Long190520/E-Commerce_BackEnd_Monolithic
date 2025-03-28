using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace E_Commerce_BackEnd.Helpers
{
    public static class EntityUtils
    {
        public static void SetPropValue(this object instance, string propertyName, object value)
        {

            PropertyInfo? property = instance.GetType().GetProperty(propertyName);
            if (property != null)
            {
                property.SetValue(instance, value, null);
            }
        }

        public static object GetPropValue(this object obj, string propName)
        {
            return obj?.GetType().GetProperty(propName)?.GetValue(obj);
        }

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
