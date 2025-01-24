using Newtonsoft.Json;

namespace AttendanceSystem.Domain.Extensions
{
    public static class Helper
    {
        public static T DeepCopy<T>(T source)
        {
            // Serialize the source object to JSON
            string json = JsonConvert.SerializeObject(source, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            // Deserialize the JSON back to a new object
            T copy = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return copy;
        }

        public static string GetWeek(DateTime start, DateTime end)
        {
            return string.Empty;
        }
    }
}