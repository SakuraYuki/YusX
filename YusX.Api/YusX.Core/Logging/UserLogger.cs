using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using YusX.Core.Configuration;
using YusX.Core.DbAccessor;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Services;
using YusX.Core.Utilities;
using YusX.Entity.Domain;

namespace YusX.Core.Providers.Logger
{
    /// <summary>
    /// 用户日志记录器
    /// </summary>
    public static class UserLogger
    {
        /// <summary>
        /// 静态构造器，初次使用时初始化
        /// </summary>
        static UserLogger()
        {
        }

        /// <summary>
        /// 日志路径
        /// </summary>
        private static readonly string _logPath = System.IO.Path.Combine(AppSetting.DownloadPath, "Logger", "Queue").MapPlatformPath();

        /// <summary>
        /// 日志队列
        /// </summary>
        private static readonly ConcurrentQueue<Sys_ApiLog> _logQueue = new ConcurrentQueue<Sys_ApiLog>();

        /// <summary>
        /// 上次日志清理的时间
        /// </summary>
        private static DateTime _lastClearTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Info(string message)
            => Info(ApiLogType.Info, message);

        /// <summary>
        /// 记录指定类型的信息日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志消息</param>
        public static void Info(ApiLogType type, string message = null)
            => Info(type, message, null);

        /// <summary>
        /// 记录指定类型的信息日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="reqParam">请求参数</param>
        /// <param name="respData">响应数据</param>
        /// <param name="ex">异常信息</param>
        public static void Info(ApiLogType type, string reqParam, string respData, Exception ex = null)
            => LogEnqueue(type, reqParam, respData, ex: ex, status: ApiResponseStatus.Info);

        /// <summary>
        /// 记录成功日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void OK(string message)
            => OK(ApiLogType.Success, message);

        /// <summary>
        /// 记录指定类型的成功日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志消息</param>
        public static void OK(ApiLogType type, string message = null)
            => OK(type, message, null);

        /// <summary>
        /// 记录指定类型的成功日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="reqParam">请求参数</param>
        /// <param name="respData">响应数据</param>
        public static void OK(ApiLogType type, string reqParam, string respData)
            => LogEnqueue(type, reqParam, respData, status: ApiResponseStatus.Success);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Error(string message)
            => Error(ApiLogType.Error, message);

        /// <summary>
        /// 记录指定类型的错误日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志消息</param>
        public static void Error(ApiLogType type, string message, Exception ex = null)
            => Error(type, message, null, ex: ex);

        /// <summary>
        /// 记录指定类型的错误日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="reqParam">请求参数</param>
        /// <param name="respData">响应数据</param>
        /// <param name="ex">异常信息</param>
        public static void Error(ApiLogType type, string reqParam, string respData, Exception ex = null)
            => LogEnqueue(type, reqParam, respData, status: ApiResponseStatus.Error, ex: ex);

        /// <summary>
        /// 启动并持续写入队列中的日志
        /// </summary>
        private static void Start()
        {
            var queueTable = CreateEmptyTable();
            while (true)
            {
                try
                {
                    if (_logQueue.Count() > 0 && queueTable.Rows.Count < 500)
                    {
                        DequeueToTable(queueTable);
                        continue;
                    }
                    //每5秒写一次数据
                    Thread.Sleep(1000);
                    if (queueTable.Rows.Count == 0)
                    {
                        continue;
                    }

                    DbProvider.SqlDapper.BulkInsert(
                        table: queueTable,
                        tableName: "Sys_ApiLog",
                        sqlBulkCopyOptions: SqlBulkCopyOptions.KeepIdentity,
                        fileName: null,
                        tmpPath: _logPath);

                    queueTable.Clear();

                    if ((DateTime.Now - _lastClearTime).TotalDays > 1)
                    {
                        FileHelper.DeleteFolder(_logPath);
                        _lastClearTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"日志批量写入数据时出错：{ex.Message}");
                    RecordErrorLog(ex.Message + ex.StackTrace + ex.Source);
                    queueTable.Clear();
                }
            }
        }

        /// <summary>
        /// 创建一个以 <see cref="Sys_ApiLog"/> 为骨架的空数据表
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateEmptyTable()
        {
            var queueTable = new DataTable();
            queueTable.Columns.Add(nameof(_m.Type), typeof(string));
            queueTable.Columns.Add(nameof(_m.Request), typeof(string));
            queueTable.Columns.Add(nameof(_m.Response), typeof(string));
            queueTable.Columns.Add(nameof(_m.ExceptionInfo), typeof(string));
            queueTable.Columns.Add(nameof(_m.ResponseStatus), Type.GetType("System.Int32"));
            queueTable.Columns.Add(nameof(_m.BeginDate), Type.GetType("System.DateTime"));
            queueTable.Columns.Add(nameof(_m.EndDate), Type.GetType("System.DateTime"));
            queueTable.Columns.Add(nameof(_m.ElapsedTime), Type.GetType("System.Int32"));
            queueTable.Columns.Add(nameof(_m.UserAddress), typeof(string));
            queueTable.Columns.Add(nameof(_m.UserAgent), typeof(string));
            queueTable.Columns.Add(nameof(_m.Url), typeof(string));
            queueTable.Columns.Add(nameof(_m.UserId), Type.GetType("System.Int32"));
            queueTable.Columns.Add(nameof(_m.UserName), typeof(string));
            return queueTable;
        }

        /// <summary>
        /// 新增一个日志到队列中
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="reqParam">请求参数</param>
        /// <param name="respData">响应数据</param>
        /// <param name="status">响应状态</param>
        /// <param name="ex">异常信息，一般 <paramref name="status"/> 为 <see cref="ApiResponseStatus.Error">Error</see> 或 <see cref="ApiResponseStatus.Info">Info</see> 时可传入</param>
        private static void LogEnqueue(ApiLogType type, string reqParam, string respData, ApiResponseStatus status = ApiResponseStatus.Success, Exception ex = null)
        {
            Sys_ApiLog log = null;
            try
            {
                var context = HttpContext.Current;

                // 不存在上下文则记录到本地日志文件
                if (context == null)
                {
                    var content = $"未获取到请求上下文，[Type]{type}，[Request]{reqParam}，[Response]{respData}，[ResponseStatus]{status}";
                    if (ex != null)
                    {
                        content += $"，[Ex]{ex}";
                    }
                    RecordErrorLog(content);
                    return;
                }

                // 不处理 OPTIONS 请求
                if (context.Request.Method == "OPTIONS")
                {
                    return;
                }

                var observer = context.RequestServices.GetService(typeof(ActionObserver)) as ActionObserver;
                var userInfo = ManageUser.UserManager.Current.UserInfo;
                log = new Sys_ApiLog()
                {
                    BeginDate = observer.RequestTime,
                    EndDate = DateTime.Now,
                    UserId = userInfo.UserId,
                    UserName = userInfo.Username,
                    Type = (int)type,
                    ExceptionInfo = ex?.Message,
                    Request = reqParam,
                    Response = respData,
                    ResponseStatus = (int)status
                };
                ApplyRequestInfo(log, context);
            }
            catch (Exception exception)
            {
                if (log == null)
                {
                    log = new Sys_ApiLog()
                    {
                        BeginDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Type = (int)type,
                        Request = reqParam,
                        Response = respData,
                        ResponseStatus = (int)status,
                        ExceptionInfo = exception.Format()
                    };
                }
            }
            _logQueue.Enqueue(log);
        }

        /// <summary>
        /// 将错误内容记录到本地文件中
        /// </summary>
        /// <param name="message">错误内容</param>
        private static void RecordErrorLog(string message)
        {
            try
            {
                FileHelper.WriteFile(
                    path: Path.Combine(_logPath, "Errors"),
                    fileName: $"{DateTime.Now:yyyyMMdd}.log",
                    content: message + "\r\n",
                    append: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API错误日志写入文件时出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 取出队列中的一个日志对象并插入数据表中
        /// </summary>
        /// <param name="queueTable">要插入的数据表</param>
        private static void DequeueToTable(DataTable queueTable)
        {
            _logQueue.TryDequeue(out Sys_ApiLog log);
            var row = queueTable.NewRow();
            if (log.BeginDate == null || log.BeginDate?.Year < 2010)
            {
                log.BeginDate = DateTime.Now;
            }
            if (log.EndDate == null)
            {
                log.EndDate = DateTime.Now;
            }
            row[nameof(_m.Type)] = log.Type;
            row[nameof(_m.Request)] = log.Request?.Replace("\r\n", "");
            row[nameof(_m.Response)] = log.Response?.Replace("\r\n", "");
            row[nameof(_m.ExceptionInfo)] = log.ExceptionInfo;
            row[nameof(_m.ResponseStatus)] = log.ResponseStatus ?? -1;
            row[nameof(_m.BeginDate)] = log.BeginDate;
            row[nameof(_m.EndDate)] = log.EndDate;
            row[nameof(_m.ElapsedTime)] = (log.EndDate.Value - log.BeginDate.Value).TotalMilliseconds;
            row[nameof(_m.UserAddress)] = log.UserAddress;
            row[nameof(_m.UserAgent)] = log.UserAgent;
            row[nameof(_m.Url)] = log.Url;
            row[nameof(_m.UserId)] = log.UserId ?? -1;
            row[nameof(_m.UserName)] = log.UserName;
            queueTable.Rows.Add(row);
        }
    }
}
