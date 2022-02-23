using Newtonsoft.Json;

namespace WeeklyXamarin.Core.Helpers
{
    public static class JsonExtensions
    {
        public static string ToJson(this object o)
            => JsonConvert.SerializeObject(o, Formatting.Indented);
    }
}
