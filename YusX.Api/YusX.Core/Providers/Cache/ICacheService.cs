using System;
using System.Collections.Generic;

namespace YusX.Core.Providers.Cache
{
    /// <summary>
    /// 缓存服务 接口
    /// </summary>
    public interface ICacheService : IDisposable
    {
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 将值插入到列表头部
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="val">缓存值</param>
        void LPush(string key, string val);

        /// <summary>
        /// 在列表中添加值
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="val">缓存值</param>
        void RPush(string key, string val);

        /// <summary>
        /// 移除并获取列表最后一个元素并转换为指定类型
        /// </summary>
        /// <typeparam name="T">要转换的制定类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        T ListDequeue<T>(string key) where T : class;

        /// <summary>
        /// 移除并获取列表最后一个元素
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        object ListDequeue(string key);

        /// <summary>
        /// 移除列表中开始位置指定数量的元素
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="keepIndex">指定数量元素，如果要移除开头的3个元素，应该传4</param>
        void ListRemove(string key, int keepIndex);

        /// <summary>
        /// 添加对象缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns>是否添加成功</returns>
        bool AddObject(string key, object value, int expireSeconds = -1, bool isSliding = false);

        /// <summary>
        /// 添加字符串缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns>是否添加成功</returns>
        bool Add(string key, string value, int expireSeconds = -1, bool isSliding = false);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>是否删除成功</returns>
        bool Remove(string key);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        void RemoveAll(IEnumerable<string> keys);

        /// <summary>
        /// 获取对象缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>获取到的对象</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 获取字符串缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>获取到的字符串</returns>
        string Get(string key);
    }
}
