using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.dtos
{
    public class HttpRequest
    {
        internal HttpRequest() { }
        internal HttpRequest(IntPtr parser,IntPtr sockinfo)
        {
            this.Parser = new Parser(parser);
            this.SockInfo = new SockInfo(sockinfo);
        }

        /// <summary>
        /// http 解析器相关
        /// </summary>
        public Parser Parser { get; internal set; }

        /// <summary>
        /// 连接套字节相关
        /// </summary>
        public SockInfo SockInfo { get; internal set; }
    }
}
