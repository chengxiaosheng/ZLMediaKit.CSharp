using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public class HttpAccess : HttpAccessBefore
    {
        internal HttpAccess(IntPtr parser,IntPtr sockinfo,bool isDir,string path) : base (parser, sockinfo,path)
        {
            this.IsDir = IsDir;
        }
        public bool IsDir { get; internal set; }

    }

    public class HttpAccessBefore
    {
        public HttpAccessBefore(IntPtr parser, IntPtr sockinfo,string path)
        {
            this.Parser = new Parser(parser);
            this.SockInfo = new SockInfo(sockinfo);
            this.Path = path;
        }
        public Parser Parser { get; internal set; }
        public string Path { get; internal set; }
        public SockInfo SockInfo { get; internal set; }
    }
}
