using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Filters;
using YusX.Core.Services;
using YusX.Core.Utilities;
using YusX.Entity.System;

namespace YusX.Core.Controllers.Basic
{
    [JWTAuthorize, ApiController]
    public class ApiBaseController<IServiceBase> : Controller
    {
        public ApiBaseController()
        {
        }

        public ApiBaseController(IServiceBase service)
        {
            this.service = service;
        }

        public ApiBaseController(string projectName, string folder, string tablename, IServiceBase service)
        {
            this.service = service;
        }

        protected IServiceBase service;

        private WebResponseContent ResponseContent { get; set; }

        /// <summary>
        /// 2020.11.21增加json原格式返回数据(默认是驼峰格式)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="serializerSettings"></param>
        /// <returns></returns>
        protected JsonResult JsonNormal(object data, JsonSerializerSettings serializerSettings = null, bool formateDate = true)
        {
            serializerSettings ??= new JsonSerializerSettings();
            serializerSettings.ContractResolver = null;

            if (formateDate)
            {
                serializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }

            return Json(data, serializerSettings);
        }

        [ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpPost, Route("GetPageData")]
        public virtual ActionResult GetPageData([FromBody] PaginationOptions loadData)
        {
            return JsonNormal(InvokeService("GetPageData", new object[] { loadData }));
        }

        /// <summary>
        /// 获取明细grid分页数据
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpPost, Route("GetDetailPage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult GetDetailPage([FromBody] PaginationOptions loadData)
        {
            return Content(InvokeService("GetDetailPage", new object[] { loadData }).Serialize());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("Upload")]
        [ApiActionPermission(ActionPermissionOptions.Upload)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Upload(IEnumerable<IFormFile> fileInput)
        {
            return Json(InvokeService("Upload", new object[] { fileInput }));
        }

        /// <summary>
        /// 下载导入Excel模板
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownLoadTemplate")]
        [ApiActionPermission(ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult DownLoadTemplate()
        {
            ResponseContent = InvokeService("DownLoadTemplate", new object[] { }) as WebResponseContent;
            if (!ResponseContent.Status) return Json(ResponseContent);
            byte[] fileBytes = System.IO.File.ReadAllBytes(ResponseContent.Data.ToString());
            return File(
                    fileBytes,
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    Path.GetFileName(ResponseContent.Data.ToString())
                );
        }

        /// <summary>
        /// 导入表数据Excel
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("Import")]
        [ApiActionPermission(ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Import(List<IFormFile> fileInput)
        {
            return Json(InvokeService("Import", new object[] { fileInput }));
        }

        /// <summary>
        /// 导出文件，返回日期+文件名
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Export)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost, Route("Export")]
        public virtual ActionResult Export([FromBody] PaginationOptions loadData)
        {
            var result = InvokeService("Export", new object[] { loadData }) as WebResponseContent;
            return File(
                   System.IO.File.ReadAllBytes(result.Data.ToString().MapPath()),
                   System.Net.Mime.MediaTypeNames.Application.Octet,
                   Path.GetFileName(result.Data.ToString())
               );
        }

        /// <summary>
        /// 通过key删除文件
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Delete)]
        [HttpPost, Route("Del")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Del([FromBody] object[] keys)
        {
            ResponseContent = InvokeService("Del", new object[] { keys, true }) as WebResponseContent;
            LogProvider.Info(ApiLogType.Del, keys.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return Json(ResponseContent);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Audit)]
        [HttpPost, Route("Audit")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Audit([FromBody] object[] id, int? auditStatus, string auditReason)
        {
            ResponseContent = InvokeService("Audit", new object[] { id, auditStatus, auditReason }) as WebResponseContent;
            LogProvider.Info(ApiLogType.Del, id?.Serialize() + "," + (auditStatus ?? -1) + "," + auditReason, ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return Json(ResponseContent);
        }

        /// <summary>
        /// 新增支持主子表
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Add)]
        [HttpPost, Route("Add")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Add([FromBody] SaveDataModel saveModel)
        {
            ResponseContent = InvokeService("Add",
                new Type[] { typeof(SaveDataModel) },
                new object[] { saveModel }) as WebResponseContent;
            LogProvider.Info(ApiLogType.Add, saveModel.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            ResponseContent.Data = ResponseContent.Data?.Serialize();
            return Json(ResponseContent);
        }

        /// <summary>
        /// 编辑支持主子表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// [ModelBinder(BinderType =(typeof(ModelBinder.BaseModelBinder)))]可指定绑定modelbinder
        /// </remarks>
        [ApiActionPermission(ActionPermissionOptions.Update)]
        [HttpPost, Route("Update")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Update([FromBody] SaveDataModel saveModel)
        {
            ResponseContent = InvokeService("Update", new object[] { saveModel }) as WebResponseContent;
            LogProvider.Info(ApiLogType.Edit, saveModel.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            ResponseContent.Data = ResponseContent.Data?.Serialize();
            return Json(ResponseContent);
        }

        /// <summary>
        /// 调用service方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, object[] parameters)
        {
            return service.GetType().GetMethod(methodName).Invoke(service, parameters);
        }

        /// <summary>
        /// 调用service方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="types">为要调用重载的方法参数类型：new Type[] { typeof(SaveDataModel)</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, Type[] types, object[] parameters)
        {
            return service.GetType().GetMethod(methodName, types).Invoke(service, parameters);
        }
    }
}
