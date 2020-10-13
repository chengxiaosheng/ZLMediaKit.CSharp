using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.dtos
{
    public class MediaPlayInfo : IDisposable
    {
        public bool IsVideo { get; internal set; }

        public EM_Codec Codec { get; internal set; }

        public IntPtr Data { get; internal set; }

        private byte[] _data;

        private bool IsCopy = false;
        private bool disposedValue;

        /// <summary>
        /// 异步执行应该采用copy模式 
        /// 非异步执行直接从内存中获取数据
        /// </summary>
        /// <param name="copy"></param>
        internal unsafe void FillData(bool copy = false)
        {
            if(copy)
            {
                IsCopy = true;
                this._data = new byte[this.Length];
                new Span<byte>(this.Data.ToPointer(), this.Length).CopyTo(this._data);
                fixed(void * buffer = this._data)
                {
                    Data = (IntPtr)buffer;
                }
            }
            else
            {
                this._data = new Span<byte>(this.Data.ToPointer(), this.Length).ToArray();
            }
        }

        public byte[] DataBytes { 
            get {
                if (_data != null) FillData(); return _data;
            }
        }
            

        public int Length { get; set; }

        public uint Dts { get; internal set; }

        public uint Pts { get; internal set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
                if (this.IsCopy)
                {
                    Marshal.FreeHGlobal(this.Data);
                }
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~MediaPlayInfo()
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
