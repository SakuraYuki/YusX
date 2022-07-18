using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YusX.Core.DbAccessor;
using YusX.Core.Providers;
using YusX.Core.Providers.Cache;
using YusX.Core.Services;
using YusX.Entity.Domain;

namespace YusX.Core.Dictionaries
{
    public static class DictionaryManager
    {
        public const string Key = "inernalDic";

        private static readonly object _dicObj = new object();

        private static string _dicVersionn = "";

        private static List<Sys_Dictionary> CacheDictionaries { get; set; }

        public static List<Sys_Dictionary> Dictionaries => GetAllDictionary();

        public static Sys_Dictionary GetDictionary(string dicNo)
            => GetDictionaries(new string[] { dicNo }).FirstOrDefault();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicNos"></param>
        /// <param name="executeSql">是否执行自定义sql</param>
        /// <returns></returns>
        public static IEnumerable<Sys_Dictionary> GetDictionaries(IEnumerable<string> dicNos, bool executeSql = true)
        {
            static List<Sys_DictionaryItem> query(string sql)
            {
                try
                {
                    return DbProvider.SqlDapper.QueryList<SourceKeyVaule>(sql, null).Select(s => new Sys_DictionaryItem()
                    {
                        Name = s.Value,
                        Value = s.Key.ToString()
                    }).ToList();
                }
                catch (Exception ex)
                {
                    ApiLogger.Error($"字典执行sql异常,sql:{sql},异常信息：{ex.Message + ex.StackTrace}");
                    throw ex;
                    //  Console.WriteLine(ex.Message);
                    // return null;
                }
            }
            foreach (var item in Dictionaries.Where(x => dicNos.Contains(x.DictNo)))
            {
                if (executeSql)
                {
                    //  2020.05.01增加根据用户信息加载字典数据源sql
                    string sql = DictionaryHandler.GetCustomDBSql(item.DictNo, item.DbSql);
                    if (!string.IsNullOrEmpty(item.DbSql))
                    {
                        item.DictionaryItems = query(sql);
                    }
                }
                yield return item;
            }
        }

        /// <summary>
        /// 每次变更字典配置的时候会重新拉取所有配置进行缓存(自行根据实际处理)
        /// </summary>
        /// <returns></returns>
        private static List<Sys_Dictionary> GetAllDictionary()
        {
            var cacheService = DependencyProvider.GetService<ICacheService>();
            //每次比较缓存是否更新过，如果更新则重新获取数据
            if (CacheDictionaries != null && _dicVersionn == cacheService.Get(Key))
            {
                return CacheDictionaries;
            }

            lock (_dicObj)
            {
                if (_dicVersionn != "" && CacheDictionaries != null && _dicVersionn == cacheService.Get(Key)) return CacheDictionaries;
                CacheDictionaries = DbProvider.DbContext
                    .Set<Sys_Dictionary>()
                    .Where(x => x.Enable)
                    .Include(c => c.DictionaryItems).ToList();

                string cacheVersion = cacheService.Get(Key);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    cacheService.Add(Key, cacheVersion);
                }
                else
                {
                    _dicVersionn = cacheVersion;
                }
            }
            return CacheDictionaries;
        }
    }

    public class SourceKeyVaule
    {
        public object Key { get; set; }

        public string Value { get; set; }
    }
}
