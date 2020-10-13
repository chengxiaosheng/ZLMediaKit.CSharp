using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ZLMediaKit.CSharp.dtos
{
    /// <summary>
    /// Http Body
    /// </summary>
    public class HttpBody : IDisposable
    {
        internal IntPtr _id = IntPtr.Zero;
        private bool disposedValue;

        internal IntPtr headerIntptr = IntPtr.Zero;

        public string StringBody { get; set; }

        public Dictionary<string,string> MultiFormBody { get; set; }

        public string FileBody { get; set; }

        public Dictionary<string,string> HttpHeader { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        internal IntPtr GetHttpHeader()
        {
            string[] array;
            if(this.HttpHeader == null || this.HttpHeader.Count == 0 )
            {
                array = new string[1] {  null };
            }else
            {
                array = HttpHeader.Select(s => new string[2] { s.Key, s.Value }).SelectMany(s => s).Union(new string[] { null }).ToArray();
            }
            headerIntptr = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
            return this.headerIntptr;
        }

        internal IntPtr GetHttpBody()
        {
            if(MultiFormBody!= null && MultiFormBody.Count > 0)
            {
                var array = MultiFormBody.Select(s => new string[2]{ s.Key, s.Value }).ToArray();
                IntPtr body = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                this._id = PInvoke.ZLMediaKitMethod.mk_http_body_from_multi_form(ref body, FileBody);
            }else if(!string.IsNullOrEmpty(FileBody))
            {
                this._id = PInvoke.ZLMediaKitMethod.mk_http_body_from_file(FileBody);
            }else if (!string.IsNullOrEmpty(StringBody))
            {
                this._id = PInvoke.ZLMediaKitMethod.mk_http_body_from_string(StringBody, FileBody.Length);
            }
            else
            {
                throw new InvalidDataException("参数不合法");
            }
            return this._id;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                if (this._id != IntPtr.Zero) Marshal.FreeHGlobal(_id);
                if (this.headerIntptr != IntPtr.Zero) Marshal.FreeHGlobal(headerIntptr);

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~HttpBody()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
