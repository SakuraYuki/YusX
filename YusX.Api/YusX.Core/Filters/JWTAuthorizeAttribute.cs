using Microsoft.AspNetCore.Authorization;

namespace YusX.Core.Filters
{
    /// <summary>
    /// JWT 授权标识，标识指定控制器需要验证 JWT 授权
    /// </summary>
    public class JWTAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 空构造器
        /// </summary>
        public JWTAuthorizeAttribute() : base()
        {

        }
    }
}
