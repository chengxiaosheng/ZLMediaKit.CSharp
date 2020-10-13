using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ZLMediaKit.CSharp.Models;
using ZLMediaKit.CSharp.ZLMediaKit;

namespace ZLMediaKit.CSharp.ZLMediaKit
{
    /// <summary>
    /// 收到tcp服务器发来的数据
    /// </summary>
    /// <param name="mk_tcp_client">tcp客户端</param>
    /// <param name="data">数据指针</param>
    /// <param name="len">数据长度</param>
    public delegate void mk_tcp_client_data(string data, int len);
    delegate void TcpSessionManager(ushort serverPort, IntPtr mk_tcp_session);
    public class TcpClient : IDisposable
    {

        private PInvoke.mk_tcp_client_events _clientEvents;

        private void mk_tcp_client_connect(IntPtr mk_tcp_client,int code ,string message)
        {
            if (this._id != mk_tcp_client) return;
            if (code == 0)
            {
                this.IsConnected = true;
            }
            this.message = message;
            this._autoResetEvent.Set();
        }

        private  void mk_tcp_client_disconnect(IntPtr mk_tcp_client,int code,string message)
        {
            if (this._id != mk_tcp_client) return;
            this.IsConnected = false;
        }

        private void mk_tcp_client_data(IntPtr mk_tcp_client,string data,int datalne)
        {
            if (this._id != mk_tcp_client) return;
            if (this.OnReviced != null) this.OnReviced.Invoke(data,datalne); 
        }

        private  void on_mk_tcp_client_manager(IntPtr mk_tcp_client)
        {
            if (mk_tcp_client != this._id) return;
            if (this.OnManager != null) this.OnManager.Invoke();
        }


        public TcpClient(TcpType tcpType)
        {
            _clientEvents = new PInvoke.mk_tcp_client_events
            {
                OnConnect = new PInvoke.on_mk_tcp_client_connect(mk_tcp_client_connect),
                OnData = new PInvoke.on_mk_tcp_client_data(mk_tcp_client_data),
                OnDisconnect = new PInvoke.on_mk_tcp_client_disconnect(mk_tcp_client_disconnect),
                OnManager = new PInvoke.on_mk_tcp_client_manager(on_mk_tcp_client_manager)
            };
            this._id = PInvoke.ZLMediaKitMethod.mk_tcp_client_create(ref _clientEvents, tcpType);
        }

        /// <summary>
        /// Tcp Client 对象
        /// </summary>
        internal IntPtr _id = IntPtr.Zero;
        private bool disposedValue;


        /// <summary>
        /// TCP会话唯一标识
        /// </summary>
        public Int64 Id => (Int64)_id;

        /// <summary>
        /// 套字节连接信息
        /// </summary>
        public SockInfo SockInfo => new SockInfo(PInvoke.ZLMediaKitMethod.mk_tcp_client_get_sock_info(this._id));

        /// <summary>
        /// TCP是否连接
        /// </summary>
        public bool IsConnected { get; set; } = false;
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private string message;

        /// <summary>
        /// 发起连接
        /// </summary>
        /// <param name="host">服务器ip或域名</param>
        /// <param name="port">服务器端口号</param>
        /// <param name="timeout">超时时间</param>
        public (bool,string) Connect(string host,ushort port,int timeout)
        {
            PInvoke.ZLMediaKitMethod.mk_tcp_client_connect(this._id, host, port, timeout);
            if( _autoResetEvent.WaitOne(timeout + 10))
            {
                return (this.IsConnected, this.message);
            }
            return (false, "等待ZLMediaKit处理超时");
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len">数据长度，等于0时，内部通过strlen获取</param>
        public void Send(string data,int len = 0)
        {
            PInvoke.ZLMediaKitMethod.mk_tcp_client_send(this._id, data, len);
        }
        /// <summary>
        /// 切换到本对象的网络线程后再发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len">数据长度，等于0时，内部通过strlen获取</param>
        public void SendSafe(string data,int len=0)
        {
            PInvoke.ZLMediaKitMethod.mk_tcp_client_send_safe(this._id, data, len);
        }

        /// <summary>
        /// 收到数据时触发
        /// </summary>
        public event mk_tcp_client_data OnReviced;

        public event Action OnManager;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    this._clientEvents = default;
                }
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                this.IsConnected = false;
                PInvoke.ZLMediaKitMethod.mk_tcp_client_release(_id);
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~TcpClient()
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
