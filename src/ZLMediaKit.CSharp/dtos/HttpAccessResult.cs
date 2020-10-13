using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.dtos
{
    public class HttpAccessResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">如果为空，则代表鉴权通过，否则为错误提示,可以为null</param>
        /// <param name="path">运行或禁止访问的根目录,可以为null</param>
        /// <param name="cookieLifeSecond">鉴权cookie有效期</param>
        public HttpAccessResult(string message,string path = null,int cookieLifeSecond = 3600)
        {
            this.Message = message;
            this.AccessPath = path;
            this.CookieLifeSecond = cookieLifeSecond;
        }

        /// <summary>
        /// 如果为空，则代表鉴权通过，否则为错误提示,可以为null
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 运行或禁止访问的根目录,可以为null
        /// </summary>
        public string AccessPath { get; set; }

        /// <summary>
        /// 鉴权cookie有效期
        /// </summary>
        public int CookieLifeSecond { get; set; }
    }
}
