using Newtonsoft.Json;
using YusX.Core.Enums;
using YusX.Core.Extensions;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// API响应内容
    /// </summary>
    public class ApiResponseContent
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public ApiResponseContent()
        {
        }

        /// <summary>
        /// 使用指定状态码构造
        /// </summary>
        /// <param name="status">响应的状态码</param>
        public ApiResponseContent(int status)
        {
            Status = status;
        }

        /// <summary>
        /// 响应消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 响应状态码
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 响应的业务数据
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// 设置为成功的响应
        /// </summary>
        /// <param name="msg">响应消息</param>
        /// <returns></returns>
        public ApiResponseContent OK(string msg = null)
        {
            return Set(ResponseType.OperSuccess, msg, ApiStatutsCode.OK);
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="responseType">响应消息类型</param>
        /// <param name="status">响应状态码</param>
        /// <returns></returns>
        public ApiResponseContent Set(ResponseType responseType, ApiStatutsCode? status = null)
        {
            return Set(responseType, null, status);
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="responseType">响应消息类型</param>
        /// <param name="msg">响应消息，如果传入null则取responseType的说明消息</param>
        /// <param name="status">响应状态码</param>
        public ApiResponseContent Set(ResponseType responseType, string msg = null, ApiStatutsCode? status = null)
        {
            if (status != null)
            {
                Status = (int)status;
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {
                Message = msg;
                return this;
            }

            if (!string.IsNullOrWhiteSpace(Message))
            {
                return this;
            }

            Message = responseType.GetMessage();
            return this;
        }
    }
}
