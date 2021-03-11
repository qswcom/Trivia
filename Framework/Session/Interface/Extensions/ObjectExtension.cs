using Newtonsoft.Json;

namespace Com.Qsw.Framework.Session.Interface
{
    public static class ObjectExtension
    {
        public static T Clone<T>(this T original) where T : class
        {
            if (original == null)
            {
                return default;
            }

            string json = JsonConvert.SerializeObject(original);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}