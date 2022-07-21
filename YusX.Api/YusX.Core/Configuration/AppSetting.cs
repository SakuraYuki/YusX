using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using YusX.Core.Configuration.Model;
using YusX.Core.Constracts;
using YusX.Core.Extensions;

namespace YusX.Core.Configuration
{
    /// <summary>
    /// 程序配置
    /// </summary>
    public static class AppSetting
    {
        /// <summary>
        /// HTTP 请求头中的授权字段名
        /// </summary>
        public const string TokenHeaderName = "Authorization";

        /// <summary>
        /// 连接信息
        /// </summary>
        private static Connection Connection;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DbConnectionString => Connection.DbConnectionString;

        /// <summary>
        /// Redis 连接字符串
        /// </summary>
        public static string RedisConnectionString => Connection.RedisConnectionString;

        /// <summary>
        /// 是否使用 Redis
        /// </summary>
        public static bool UseRedis => Connection.UseRedis;

        /// <summary>
        /// 是否使用 SignalR
        /// </summary>
        public static bool UseSignalR => Connection.UseSignalR;

        /// <summary>
        /// 下载路径
        /// </summary>
        public static string DownloadPath => Path.Combine(CurrentPath ?? "", "Download").MapPlatformPath();

        /// <summary>
        /// JWT 有效期，单位：分钟，默认2小时，即120
        /// </summary>
        public static int ExpMinutes { get; private set; } = 120;

        /// <summary>
        /// APP 端 JWT 有效期，单位：分钟，默认30天，即43200
        /// </summary>
        public static int AppExpMinutes { get; private set; } = 43200;

        /// <summary>
        /// 当前程序路径
        /// </summary>
        public static string CurrentPath { get; private set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 加密信息
        /// </summary>
        public static Secret Secret { get; private set; }

        /// <summary>
        /// 默认创建者
        /// </summary>
        public static CreateMember CreateMember { get; private set; }

        /// <summary>
        /// 默认修改者
        /// </summary>
        public static ModifyMember ModifyMember { get; private set; }

        /// <summary>
        /// Actions 权限过滤
        /// </summary>
        public static GlobalFilter GlobalFilter { get; private set; }

        /// <summary>
        /// kafka 配置
        /// </summary>
        public static Kafka Kafka { get; private set; }

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            services.Configure<Connection>(configuration.GetSection("Connection"));
            services.Configure<Secret>(configuration.GetSection("Secret"));
            services.Configure<CreateMember>(configuration.GetSection("CreateMember"));
            services.Configure<ModifyMember>(configuration.GetSection("ModifyMember"));
            services.Configure<GlobalFilter>(configuration.GetSection("GlobalFilter"));
            services.Configure<Kafka>(configuration.GetSection("Kafka"));

            var provider = services.BuildServiceProvider();
            var environment = provider.GetRequiredService<IWebHostEnvironment>();

            CurrentPath = Path.Combine(environment.ContentRootPath, "").MapPlatformPath();
            ExpMinutes = (configuration["ExpMinutes"] ?? "120").GetInt();
            AppExpMinutes = (configuration["ExpMinutes"] ?? "43200").GetInt();

            Connection = provider.GetRequiredService<IOptions<Connection>>().Value;
            Secret = provider.GetRequiredService<IOptions<Secret>>().Value;
            CreateMember = provider.GetRequiredService<IOptions<CreateMember>>().Value ?? new CreateMember();
            ModifyMember = provider.GetRequiredService<IOptions<ModifyMember>>().Value ?? new ModifyMember();
            GlobalFilter = provider.GetRequiredService<IOptions<GlobalFilter>>().Value ?? new GlobalFilter();
            GlobalFilter.Actions = GlobalFilter.Actions ?? new string[0];
            Kafka = provider.GetRequiredService<IOptions<Kafka>>().Value ?? new Kafka();

            DbInfo.Name = Connection.DbType;
            if (string.IsNullOrWhiteSpace(Connection.DbConnectionString))
            {
                throw new System.Exception("未配置好数据库默认连接");
            }

            if (Connection.DbConnectionString.TryDecryptDES(Secret.Db, out var dbConnectionString))
            {
                Connection.DbConnectionString = dbConnectionString;
            }

            if (!string.IsNullOrWhiteSpace(Connection.RedisConnectionString)
                && Connection.RedisConnectionString.TryDecryptDES(Secret.Redis, out var redisConnectionString))
            {
                Connection.RedisConnectionString = redisConnectionString;
            }
        }

        /// <summary>
        /// 获取某个节点的设置字符串
        /// </summary>
        /// <param name="key">配置键，例如：section1:key1</param>
        /// <returns></returns>
        public static string GetSettingString(string key)
        {
            return Configuration[key];
        }

        /// <summary>
        /// 获取指定 Section 节点
        /// </summary>
        /// <param name="key">Section 节点名称</param>
        /// <returns></returns>
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration.GetSection(key);
        }
    }
}
