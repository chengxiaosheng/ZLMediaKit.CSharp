using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ZLMediaKit.CSharp.Helper
{
    internal class MarshalString : IDisposable
    {
        private bool disposedValue;

        internal IntPtr Value = IntPtr.Zero;

        public MarshalString(int size)
        {
            if (size <= 0) throw new InvalidDataException("申请的空间必需大于0");
            Value = Marshal.AllocHGlobal(size);
        }

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
                if(Value != IntPtr.Zero) Marshal.FreeHGlobal(Value);
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~MarshalString()
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
