using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ZLMediaKit.CSharp.Helper;

namespace ZLMediaKit.CSharp.Models
{
    public class Parser
    {
        internal IntPtr _id = IntPtr.Zero;

        private Parser() { }
        internal Parser(IntPtr id)
        {
            _id = id;
        }

        /// <summary>
        /// 获取命令字，譬如GET/POST
        /// </summary>
        public string Method => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_method(this._id));
        /// <summary>
        /// 获取HTTP的访问url(不包括?后面的参数)
        /// </summary>
        public string Url => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_url(this._id));
        /// <summary>
        /// 包括?后面的参数
        /// </summary>
        public string FullUrl => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_full_url(this._id));
        /// <summary>
        /// 后面的参数字符串
        /// </summary>
        public string Params => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_url_params(this._id));

        /// <summary>
        /// 后面的参数字符串
        /// </summary>
        public NameValueCollection ParamsCollenction => QueryString.GetQueryString(Params,null,false);


        /// <summary>
        /// ，获取协议相关信息，譬如 HTTP/1.1
        /// </summary>
        public string Tail => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_tail(this._id));
        /// <summary>
        /// 获取HTTP body
        /// </summary>
        /// <returns></returns>
        /// <remarks>实验性写法，可能会有问题</remarks>
        public unsafe string GetConetentString()
        {
            int length = 0;
            var arr = PInvoke.ZLMediaKitMethod.mk_parser_get_content(this._id,ref length);
            Span<string> span = new Span<string>(arr, length);
            return span.GetPinnableReference();
        }

        /// <summary>
        /// 获取HTTP body
        /// </summary>
        /// <returns></returns>
        /// <remarks>实验性写法，可能会有问题</remarks>
        public unsafe byte[] GetContentBytes()
        {
            int length = 0;
            var arr = PInvoke.ZLMediaKitMethod.mk_parser_get_content(this._id, ref length);
            Span<byte[]> span = new Span<byte[]>(arr, length);
            return span.GetPinnableReference();
        }

        /// <summary>
        /// 获取HTTP头中特定字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetHeader(string key) => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_header(this._id,key));

        /// <summary>
        /// 获取?后面的参数中的特定参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetParam(string key) => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_parser_get_url_param(this._id, key));



    }
}
