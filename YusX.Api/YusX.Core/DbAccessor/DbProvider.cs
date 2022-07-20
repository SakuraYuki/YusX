using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using YusX.Core.Configuration;
using YusX.Core.Constracts;
using YusX.Core.Enums;
using YusX.Core.Extensions;

namespace YusX.Core.DbAccessor
{
    /// <summary>
    /// 数据库服务提供者
    /// </summary>
    public class DbProvider
    {
        /// <summary>
        /// 为<see cref="DbProvider"/>初始化
        /// </summary>
        static DbProvider()
        {
            SetConnection(DefaultConnectionName, AppSetting.DbConnectionString);
        }

        /// <summary>
        /// 连接池
        /// </summary>
        private static readonly Dictionary<string, string> ConnectionPool = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 默认连接配置名
        /// </summary>
        private static readonly string DefaultConnectionName = "defalut";

        public static void SetConnection(string key, string val)
        {
            if (ConnectionPool.ContainsKey(key))
            {
                ConnectionPool[key] = val;
                return;
            }
            ConnectionPool.Add(key, val);
        }

        /// <summary>
        /// 设置默认数据库连接
        /// </summary>
        /// <param name="val"></param>
        public static void SetDefaultConnection(string val)
        {
            SetConnection(DefaultConnectionName, val);
        }

        public static string GetConnectionString(string key)
        {
            key = key ?? DefaultConnectionName;
            if (ConnectionPool.ContainsKey(key))
            {
                return ConnectionPool[key];
            }
            return key;
        }

        /// <summary>
        /// 获取默认数据库连接
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(DefaultConnectionName);
        }

        public static System.Data.IDbConnection GetDbConnection(string connString = null)
        {
            if (connString == null)
            {
                connString = ConnectionPool[DefaultConnectionName];
            }
            if (DbInfo.Name.EqualsIgnoreCase(DbCurrentType.MySql))
            {
                return new MySql.Data.MySqlClient.MySqlConnection(connString);
            }
            return new SqlConnection(connString);
        }

        /// <summary>
        /// 扩展dapper 获取MSSQL数据库DbConnection，默认系统获取配置文件的DBType数据库类型，
        /// </summary>
        /// <param name="connString">如果connString为null 执行重载GetDbConnection(string connString = null)</param>
        /// <param name="dapperType">指定连接数据库的类型：MySql/MsSql/PgSql</param>
        /// <returns></returns>
        public static System.Data.IDbConnection GetDbConnection(string connString = null, DbCurrentType dbCurrentType = DbCurrentType.Default)
        {
            //默认获取DbConnection
            if (connString.IsNullOrEmpty() || DbCurrentType.Default == dbCurrentType)
            {
                return GetDbConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.MySql)
            {
                return new MySql.Data.MySqlClient.MySqlConnection(connString);
            }
            return new SqlConnection(connString);

        }
    }
}
