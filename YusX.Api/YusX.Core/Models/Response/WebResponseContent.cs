using YusX.Core.Enums;
using YusX.Core.Extensions;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// Web响应内容
    /// </summary>
    public class WebResponseContent
    {
        public WebResponseContent()
        {
        }

        public WebResponseContent(bool status)
        {
            Status = status;
        }

        public static WebResponseContent Instance => new WebResponseContent();

        public bool Status { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public WebResponseContent OK()
        {
            Status = true;
            return this;
        }

        public WebResponseContent OK(string message = null, object data = null)
        {
            Status = true;
            Message = message;
            Data = data;
            return this;
        }

        public WebResponseContent OK(ResponseType responseType)
        {
            return Set(responseType, true);
        }

        public WebResponseContent Error(string message = null)
        {
            Status = false;
            Message = message;
            return this;
        }

        public WebResponseContent Error(ResponseType responseType)
        {
            return Set(responseType, false);
        }

        public WebResponseContent Set(ResponseType responseType)
        {
            bool? b = null;
            return Set(responseType, b);
        }

        public WebResponseContent Set(ResponseType responseType, bool? status)
        {
            return Set(responseType, null, status);
        }

        public WebResponseContent Set(ResponseType responseType, string msg)
        {
            bool? b = null;
            return Set(responseType, msg, b);
        }

        public WebResponseContent Set(ResponseType responseType, string msg, bool? status)
        {
            if (status != null)
            {
                Status = (bool)status;
            }
            Code = ((int)responseType).ToString();
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Message = msg;
                return this;
            }
            Message = responseType.GetMessage();
            return this;
        }
    }
}
