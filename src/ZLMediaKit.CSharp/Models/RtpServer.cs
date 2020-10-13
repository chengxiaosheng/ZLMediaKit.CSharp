using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public class RtpServer : IDisposable
    {
        private IntPtr _id = IntPtr.Zero;
        private bool disposedValue;

        /// <summary>
        /// Rtp端口
        /// </summary>
        public ushort Port { get; private set; }
        /// <summary>
        /// 是否启用TCP
        /// </summary>
        public bool EnableTcp { get; private set; }

        /// <summary>
        /// 流Id
        /// </summary>
        public string StreamId { get; private set; }

        private RtpServer() { }
        internal RtpServer(string streamId, ushort port = 0,bool enableRtp = false)
        {
            this.Port = port;
            this.StreamId = streamId;
            this.EnableTcp = enableRtp;
            this._id = PInvoke.ZLMediaKitMethod.mk_rtp_server_create(this.Port, this.EnableTcp ? 1 : 0, this.StreamId);
            PInvoke.ZLMediaKitMethod.mk_rtp_server_set_on_detach(this._id, mk_rtp_server_detach, IntPtr.Zero);
            if (this.Port == 0)
            {
                this.Port = PInvoke.ZLMediaKitMethod.mk_rtp_server_port(_id);
            }
        }
        /// <summary>
        /// 监听B28181 RTP 服务器接收流超时事件
        /// </summary>
        public event Action OnTimeout;

        private void mk_rtp_server_detach(IntPtr userdata)
        {
            if (OnTimeout != null) OnTimeout.Invoke();
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
                PInvoke.ZLMediaKitMethod.mk_rtp_server_release(this._id);
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~RtpServer()
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
