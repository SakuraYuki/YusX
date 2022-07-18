using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YusX.Core.Configuration;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Services;
using YusX.Core.Utilities;
using YusX.Entity.System;

namespace YusX.Core.Controllers.Basic
{
    public class BaseController<IServiceBase> : Controller
    {
        public BaseController()
        {

        }

        public BaseController(IServiceBase service)
        {
            this.service = service;
        }

        public BaseController(string projectName, string folder, string tablename, IServiceBase service)
        {
            this.service = service;
        }

        protected IServiceBase service;

        private WebResponseContent ResponseContent { get; set; }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Manager()
        {
            return View();
            //if (System.IO.File.Exists(($"Views\\PageExtension\\{projectName }\\{TableName}Extension.cshtml").MapPath()))
            //{
            //    ViewBag.UrlExtension = $"~/Views/PageExtension/{projectName}/{TableName}Extension.cshtml";
            //}
            //  return View("~/Views/" + projectName + "/" + folder + "/" + TableName + ".cshtml");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> GetPageData(PaginationOptions loadData)
        {
            string pageData = await Task.FromResult(InvokeService("GetPageData", new object[] { loadData }).Serialize());
            return Content(pageData);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> GetDetailPage(PaginationOptions loadData)
        {
            string pageData = await Task.FromResult(InvokeService("GetDetailPage", new object[] { loadData }).Serialize());
            return Content(pageData);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> Upload(List<IFormFile> fileInput)
        {
            object result = await Task.FromResult(InvokeService("Upload", new object[] { fileInput }));
            return Json(result);
        }

        /// <summary>
        /// 导入表数据Excel
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ActionResult> Import(List<IFormFile> fileInput)
        {
            object result = await Task.FromResult(InvokeService("Import", new object[] { fileInput }));
            return Json(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> Export(PaginationOptions loadData)
        {
            return Json(await Task.FromResult(InvokeService("Export", new object[] { loadData })));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult DownLoadFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return Content("未找到文件");
            try
            {
                if (path.IndexOf("/") == -1 && path.IndexOf("\\") == -1)
                {
                    path = path.DecryptDES(AppSetting.Secret.ExportFile);
                }
                else
                {
                    path = path.MapPath();
                }
                string fileName = Path.GetFileName(path);
                return File(System.IO.File.ReadAllBytes(path), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                ApiLogger.Error($"文件下载出错:{path}{ex.Message}");
            }
            return Content("");
        }

        /// <summary>
        /// 下载Excel导入的模板
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async virtual Task<ActionResult> DownLoadTemplate()
        {
            ResponseContent = await Task.FromResult(InvokeService("DownLoadTemplate", new object[] { })) as WebResponseContent;
            if (!ResponseContent.Status)
            {
                return Json(ResponseContent);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(ResponseContent.Data.ToString());
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(ResponseContent.Data.ToString()));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> Del(object[] keys)
        {
            ResponseContent = await Task.FromResult(InvokeService("Del", new object[] { keys, true })) as WebResponseContent;
            ApiLogger.Info(ApiLogType.Del, keys.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return Json(ResponseContent);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> Audit(object[] id, int? auditStatus, string auditReason)
        {
            ResponseContent = await Task.FromResult(InvokeService("Audit", new object[] { id, auditStatus, auditReason })) as WebResponseContent;
            ApiLogger.Info(ApiLogType.Del, id?.Serialize() + "," + (auditStatus ?? -1) + "," + auditReason, ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return Json(ResponseContent);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<WebResponseContent> Add(SaveDataModel saveModel)
        {
            ResponseContent = await Task.FromResult(InvokeService("Add", new Type[] { typeof(SaveDataModel) }, new object[] { saveModel })) as WebResponseContent;
            ApiLogger.Info(ApiLogType.Add, saveModel.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return ResponseContent;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<WebResponseContent> Update(SaveDataModel saveModel)
        {
            ResponseContent = await Task.FromResult(InvokeService("Update", new object[] { saveModel })) as WebResponseContent;
            ApiLogger.Info(ApiLogType.Edit, saveModel.Serialize(), ResponseContent.Status ? "Ok" : ResponseContent.Message);
            return ResponseContent;
        }

        /// <summary>
        /// 反射调用service方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, object[] parameters)
        {
            return service.GetType().GetMethod(methodName).Invoke(service, parameters);
        }

        /// <summary>
        /// 反射调用service方法
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
