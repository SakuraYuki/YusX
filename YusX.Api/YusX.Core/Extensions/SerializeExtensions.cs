using Newtonsoft.Json;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 序列化扩展
    /// </summary>
    public static class SerializeExtensions
    {
        /// <summary>
        /// 反序列化 JSON 内容
        /// </summary>
        /// <typeparam name="T">JSON 对应的对象类型</typeparam>
        /// <param name="jsonContent">JSON 内容</param>
        /// <returns>反序列化后的对象，如果无法转换则返回对象的默认值</returns>
        public static T DeserializeObject<T>(this string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        /// <summary>
        /// 序列化内容为 JSON 格式字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="settings">序列化设置</param>
        /// <returns>序列化后的 JSON 格式字符串，如果对象为空则返回<see langword="null"/></returns>
        public static string Serialize(this object obj, JsonSerializerSettings settings = null)
        {
            if (obj == null)
            {
                return null;
            }

            settings ??= new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}

