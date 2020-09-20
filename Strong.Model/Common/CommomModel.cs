using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Strong.Model.Common
{
    /// <summary>
    ///  通用返回信息类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; }


        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode code { get; set; }

        /// <summary>
        ///     返回信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        ///     返回数据集合
        /// </summary>
        public T data { get; set; }
    }

    /// <summary>
    /// 表格数据，支持分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableModel<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 返回数据集
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 原数据集
        /// </summary>
        public T RawData { get; set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }


    }

    ///// <summary>
    ///// 分页返回类
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class PageResult<T>
    //{ /// <summary>
    //    /// 当前页标
    //    /// </summary>
    //    public int page { get; set; } = 1;
    //    /// <summary>
    //    /// 总页数
    //    /// </summary>
    //    public int pageCount { get; set; } = 6;
    //    /// <summary>
    //    /// 数据总数
    //    /// </summary>
    //    public int dataCount { get; set; } = 0;
    //    /// <summary>
    //    /// 每页大小
    //    /// </summary>
    //    public int PageSize { set; get; }
    //    /// <summary>
    //    /// 返回数据
    //    /// </summary>
    //    public T data { get; set; }
    //}

    public class SuperMapErrorResult
    {
        public error error { get; set; }

        public bool success { get; set; }
    }

    public class error
    {
        public string code { get; set; }
        public string errorMsg { get; set; }
    }
    public class HttpResult
    {
        public HttpStatusCode code { get; set; }
        public string data { get; set; }
    }
    public class JsonConfig
    {
 

        public bool isdebug { get; set; }
        public string defaulttoken { get; set; }
    }
}
