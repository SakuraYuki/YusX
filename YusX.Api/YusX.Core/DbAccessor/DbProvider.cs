using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using YusX.Core.Configuration;
using YusX.Core.Constracts;
using YusX.Core.Dapper;
using YusX.Core.EFDbContext;
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

        /// <summary>
        /// 获使用默认数据库连接初始化的Dapper操作对象
        /// </summary>
        public static ISqlDapper SqlDapper => new SqlDapper(DefaultConnectionName);

        /// <summary>
        /// 获取一个数据库上下文
        /// </summary>
        public static YusXContext DbContext => GetEFDbContext();

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
            if (DbInfo.Name.EqualsIgnoreCase(DbCurrentType.PgSql))
            {
                return new NpgsqlConnection(connString);
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
            if (dbCurrentType == DbCurrentType.PgSql)
            {
                return new NpgsqlConnection(connString);
            }
            return new SqlConnection(connString);

        }

        public static YusXContext GetEFDbContext()
        {
            return GetEFDbContext(null);
        }

        public static YusXContext GetEFDbContext(string dbName)
        {
            var beefContext = Utilities.HttpContext.Current.RequestServices.GetService(typeof(YusXContext)) as YusXContext;
            if (dbName != null)
            {
                if (!ConnectionPool.ContainsKey(dbName))
                {
                    throw new Exception("数据库连接名称错误");
                }
                beefContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
            }
            return beefContext;
        }

        public static void SetDbContextConnection(YusXContext beefContext, string dbName)
        {
            if (!ConnectionPool.ContainsKey(dbName))
            {
                throw new Exception("数据库连接名称错误");
            }
            beefContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
        }

        /// <summary>
        /// 获取实体的数据库连接
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="defaultDbContext"></param>
        /// <returns></returns>
        public static void GetDbContextConnection<TEntity>(YusXContext defaultDbContext)
        {
            //string connstr= defaultDbContext.Database.GetDbConnection().ConnectionString;
            // if (connstr != ConnectionPool[DefaultConnName])
            // {
            //     defaultDbContext.Database.GetDbConnection().ConnectionString = ConnectionPool[DefaultConnName];
            // };
        }

        public static ISqlDapper GetSqlDapper(string dbName = null)
        {
            return new SqlDapper(dbName ?? DefaultConnectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbCurrentType">指定数据库类型：MySql/MsSql/PgSql</param>
        /// <param name="dbName">指定数据连串名称</param>
        /// <returns></returns>
        public static ISqlDapper GetSqlDapper(DbCurrentType dbCurrentType, string dbName = null)
        {
            if (dbName.IsNullOrEmpty())
            {
                return new SqlDapper(dbName ?? DefaultConnectionName);
            }
            return new SqlDapper(dbName, dbCurrentType);
        }

        public static ISqlDapper GetSqlDapper<TEntity>()
        {
            //获取实体真实的数据库连接池对象名，如果不存在则用默认数据连接池名
            string dbName = typeof(TEntity).GetTypeCustomValue<DBConnectionAttribute>(x => x.DbName) ?? DefaultConnectionName;
            return GetSqlDapper(dbName);
        }
    }
}
