using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ZLMediaKit.CSharp.Helper;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.ZLMediaKit
{

    /// <summary>
    /// 创建TCPSession 的委托
    /// </summary>
    /// <param name="tcpSession"></param>
    public delegate void TcpSessionCreate(TcpSession tcpSession);
    /// <summary>
    /// 收到客户端发过来的数据委托
    /// </summary>
    /// <param name="serverPort"></param>
    /// <param name="mk_tcp_session"></param>
    /// <param name="data"></param>
    /// <param name="datalen"></param>
    delegate void TcpSessionData(ushort serverPort, IntPtr mk_tcp_session, string data, int datalen);

    delegate void TcpSessionDisconnect(ushort serverPort, IntPtr mk_tcp_session, int code, string message);

    public static class Tcp
    {
        private static PInvoke.mk_tcp_session_events _mk_tcp_session_events;
        static Tcp()
        {
            _mk_tcp_session_events = new PInvoke.mk_tcp_session_events
            {
                OnCreate = new PInvoke.on_mk_tcp_session_create(TcpSessionCreate),
                OnData = new PInvoke.on_mk_tcp_session_data(mk_tcp_session_data_event),
                OnManager = new PInvoke.on_mk_tcp_session_manager(mk_tcp_session_manager),
                OnDisconnect = new PInvoke.on_mk_tcp_session_disconnect(mk_tcp_session_disconnect)
                
            };
            PInvoke.ZLMediaKitMethod.mk_tcp_server_events_listen(ref _mk_tcp_session_events);
        }

        /// <summary>
        /// 获取基础连接信息的对端IP
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetSockPeerIp(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_sock_info_peer_ip(mk_sock_info, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog,TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取基础连接信息的对端端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetSockPeerPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_sock_info_peer_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog,TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取基础连接信息的本地IP地址
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetSockLocalIp(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_sock_info_local_ip(mk_sock_info, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 获取基础连接信息的本地端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetSockLocalPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_sock_info_local_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }


        /// <summary>
        /// 获取Tcp Session连接信息的对端IP
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetTcpSessionPeerIp(IntPtr mk_tcp_session)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_tcp_session_peer_ip(mk_tcp_session, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取Tcp Session连接信息的对端端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetTcpSessionPeerPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_tcp_session_peer_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取Tcp Session连接信息的本地IP地址
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetTcpSessionLocalIp(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_tcp_session_local_ip(mk_sock_info, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 获取Tcp Session连接信息的本地端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetTcpSessionLocalPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_tcp_session_local_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }


        /// <summary>
        /// 获取Tcp Client连接信息的对端IP
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetTcpClientPeerIp(IntPtr mk_tcp_Client)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_tcp_client_peer_ip(mk_tcp_Client, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取Tcp Client连接信息的对端端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetTcpClientPeerPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_tcp_client_peer_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取Tcp Client连接信息的本地IP地址
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<string> GetTcpClientLocalIp(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                using (var buffer = new MarshalString(ZLMediaKitConst.IpStringLength))
                {
                    PInvoke.ZLMediaKitMethod.mk_tcp_client_local_ip(mk_sock_info, buffer.Value);
                    return Marshal.PtrToStringAnsi(buffer.Value);
                }
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 获取Tcp Client连接信息的本地端口号
        /// </summary>
        /// <param name="mk_sock_info"></param>
        /// <returns></returns>
        internal static Task<ushort> GetTcpClientLocalPort(IntPtr mk_sock_info)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_tcp_client_local_port(mk_sock_info);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }



        /// <summary>
        /// 关闭一个TCP Session连接
        /// </summary>
        /// <param name="tcpSession">Tcp Session对象</param>
        /// <param name="err">错误码</param>
        /// <param name="err_msg">错误消息</param>
        /// <returns></returns>
        public static Task TcpSessionShutdown(TcpSession tcpSession,int err,string err_msg)
        {
            return Task.Run(() =>
            {
                PInvoke.ZLMediaKitMethod.mk_tcp_session_shutdown(tcpSession._id,err,err_msg);
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task TcpSessionSend(TcpSession tcpSession,string message)
        {
            return Task.Run(() =>
            {
                PInvoke.ZLMediaKitMethod.mk_tcp_session_send(tcpSession._id, message, message.Length);
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task TcpSessionSendSafe(TcpSession tcpSession, string message)
        {
            return Task.Run(() =>
            {
                PInvoke.ZLMediaKitMethod.mk_tcp_session_send_safe(tcpSession._id, message, message.Length);
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 设置TCPSession用户数据
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="userdata"></param>
        /// <returns></returns>
        public static Task TcpSessionSetUserdata(TcpSession tcpSession, Guid userdata)
        {
            return Task.Run(() =>
            {
                tcpSession._userdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid)));
                Marshal.StructureToPtr(userdata, tcpSession._userdata,true);
                PInvoke.ZLMediaKitMethod.mk_tcp_session_set_user_data(tcpSession._id, tcpSession._userdata);
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <returns></returns>
        internal static Task<Guid> TcpSessionGetUserdata(TcpSession tcpSession)
        {
            return Task.Run(() =>
            {
                IntPtr buffer = PInvoke.ZLMediaKitMethod.mk_tcp_session_get_user_data(tcpSession._id);
                return (Guid)Marshal.PtrToStructure(buffer, typeof(Guid));
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }

        private static void TcpSessionCreate(ushort server_port,IntPtr mk_tcp_session)
        {
            var autoEvent = _autoWaitEventDict.FirstOrDefault(f => f.ServerPort == server_port);
            if (autoEvent != null)
            {
                autoEvent.TcpSession = mk_tcp_session;
                autoEvent.AutoResetEvent.Set();
            }
            if (OnTcpSessionCreate == null) return;
            OnTcpSessionCreate.Invoke(new TcpSession(mk_tcp_session));
        }

        /// <summary>
        /// 创建TCP Session 的事件
        /// </summary>
        public static event TcpSessionCreate OnTcpSessionCreate;

        /// <summary>
        /// 收到ZLMediaKit 的 on_mk_tcp_session_data_event 事件
        /// 收到客户端发过来的数据
        /// </summary>
        /// <param name="serverPort"></param>
        /// <param name="mk_tcp_session"></param>
        /// <param name="data"></param>
        /// <param name="datalen"></param>
        private static void mk_tcp_session_data_event(ushort serverPort,IntPtr mk_tcp_session,string data,int datalen)
        {
            if (OnTcpSessionData == null) return;
            OnTcpSessionData.Invoke(serverPort, mk_tcp_session, data, datalen);
        }
        internal static event TcpSessionData OnTcpSessionData;

        private static void mk_tcp_session_manager(ushort serverPort,IntPtr mk_tcp_session)
        {
            if (OnTcpSessionManager == null) return;
            OnTcpSessionManager.Invoke(serverPort, mk_tcp_session);
        }

        internal static event TcpSessionManager OnTcpSessionManager;

        /// <summary>
        /// 一般由于客户端断开tcp触发
        /// </summary>
        /// <param name="serverPort">服务器端口号</param>
        /// <param name="mk_tcp_session">会话处理对象</param>
        /// <param name="code">错误代码</param>
        /// <param name="msg">错误提示</param>
        private static void mk_tcp_session_disconnect(ushort serverPort,IntPtr mk_tcp_session, int code,string msg)
        {
            if (OnTcpSessionDisconnect == null) return;
            OnTcpSessionDisconnect.Invoke(serverPort, mk_tcp_session, code, msg);
        }
        internal static event TcpSessionDisconnect OnTcpSessionDisconnect;

        private static List<TcpSessionAutoResetEvent> _autoWaitEventDict = new List<TcpSessionAutoResetEvent>();

        /// <summary>
        /// 开启tcp服务器
        /// </summary>
        /// <param name="port">监听端口号，0则为随机</param>
        /// <param name="tcpServerType">服务器类型</param>
        /// <returns></returns>
        internal static Task<TcpSession> TcpServerStart(ushort port, TcpType tcpServerType)
        {
            return Task.Run(() =>
            {
                ushort serverPort = PInvoke.ZLMediaKitMethod.mk_tcp_server_start(port, tcpServerType);
                var autoResetEvent = new TcpSessionAutoResetEvent(serverPort);
                _autoWaitEventDict.Add(autoResetEvent);
                if( autoResetEvent.AutoResetEvent.WaitOne(AppConst.WaitTcpCreateMillisecond))
                {
                    _autoWaitEventDict.Remove(autoResetEvent);
                    return new TcpSession(autoResetEvent.TcpSession);
                }
                _autoWaitEventDict.Remove(autoResetEvent);
                throw new TimeoutException("等待 ZLMediaKit on_mk_tcp_session_create回调超时");
            }).ContinueWith(GeneralTask.WriteFaultedLog);
        }





    }

    internal class TcpSessionAutoResetEvent 
    {
        internal TcpSessionAutoResetEvent(ushort serverPort)
        {
            this.ServerPort = serverPort;
        }
        internal AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
        internal ushort ServerPort { get; set; }
        internal IntPtr TcpSession { get; set; }
    }
}
