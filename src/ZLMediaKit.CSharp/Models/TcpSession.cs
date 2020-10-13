using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZLMediaKit.CSharp.ZLMediaKit;

namespace ZLMediaKit.CSharp.Models
{
    /// <summary>
    /// 收到客户端发送过来的数据
    /// </summary>
    /// <param name="server_port"></param>
    /// <param name="data"></param>
    public delegate void TcpSessionData(ushort server_port, string data);
    /// <summary>
    /// 一般由于客户端断开tcp触发
    /// </summary>
    /// <param name="code">错误代码</param>
    /// <param name="message">错误提示</param>
    public delegate void Disconnect(int code, string message);

    public class TcpSession : IDisposable
    {
        private TcpSession() { }

        internal TcpSession(IntPtr id)
        {
            this._id = id;
            Tcp.OnTcpSessionData += mk_tcp_session_data;
            Tcp.OnTcpSessionManager += mk_tcp_session_manager;
            Tcp.OnTcpSessionDisconnect += mk_tcp_session_disconnect;
        }

        /// <summary>
        /// Tcp Session 对象
        /// </summary>
        internal IntPtr _id;

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        internal IntPtr _userdata = IntPtr.Zero;

        /// <summary>
        /// TCP会话唯一标识
        /// </summary>
        public Int64 Id => (Int64)_id;

        public SockInfo SockInfo => new SockInfo(PInvoke.ZLMediaKitMethod.mk_tcp_session_get_sock_info(this._id));

        //收到数据时触发的事件
        public event TcpSessionData OnReceived;

        internal void Call_OnReceived(ushort serverPort, string data)
        {
            if (this.OnReceived == null) return;
            this.OnReceived.Invoke(serverPort, data);
        }

        private void mk_tcp_session_data(ushort serverPort,IntPtr mk_tcp_session,string data,int len)
        {
            if (this._id == mk_tcp_session)
            {
                Call_OnReceived(serverPort, data);
            }
            return;
        }

        private void mk_tcp_session_manager(ushort port,IntPtr mk_tcp_session)
        {
            if(this._id == mk_tcp_session)
            {
                if (this.OnManager != null) OnManager.Invoke();
            }
            return;
        }
        /// <summary>
        /// 每隔2秒的定时器，用于管理超时等任务
        /// </summary>
        public event Action OnManager;

        private void mk_tcp_session_disconnect(ushort serverPort,IntPtr mk_tcp_session,int code,string message)
        {
            if (this._id == mk_tcp_session)
            {
                if (this.OnDisconnect != null) OnDisconnect.Invoke(code,message);
                //连接断开，注销此对象
                this.Dispose();
            }
            return;
        }
        /// <summary>
        /// TCP 连接断开
        /// </summary>
        public event Disconnect OnDisconnect;

        /// <summary>
        /// 关闭此连接
        /// </summary>
        /// <param name="code">提示给对端的错误码</param>
        /// <param name="message">错误消息</param>
        public void Shutdown(int code,string message)
        {
            //todo : 后续测试关闭连接后是否会触发 on_mk_tcp_session_disconnect 事件，如果不触发，则需要手动调用 Dispose
            Tcp.TcpSessionShutdown(this, code, message).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (_userdata != IntPtr.Zero) Marshal.FreeHGlobal(_userdata);
            Tcp.OnTcpSessionData -= mk_tcp_session_data;
            Tcp.OnTcpSessionManager -= mk_tcp_session_manager;
            Tcp.OnTcpSessionDisconnect -= mk_tcp_session_disconnect;
        }
    }
}
