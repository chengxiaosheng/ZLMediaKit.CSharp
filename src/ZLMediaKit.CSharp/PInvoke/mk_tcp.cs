using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.PInvoke
{
    /// <summary>
    /// 收到mk_tcp_session创建对象
    /// </summary>
    /// <param name="server_port">服务器端口号</param>
    /// <param name="mk_tcp_session">会话处理对象</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_session_create(ushort server_port, IntPtr mk_tcp_session);

    /// <summary>
    /// 收到客户端发过来的数据
    /// </summary>
    /// <param name="server_port">服务器端口号</param>
    /// <param name="mk_tcp_session">会话处理对象</param>
    /// <param name="data">数据指针</param>
    /// <param name="len">数据长度</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_session_data(ushort server_port,
                                                   IntPtr mk_tcp_session,
                                                   [In()][MarshalAs(UnmanagedType.LPStr)] string data,
                                                   int len);
    /// <summary>
    /// 每隔2秒的定时器，用于管理超时等任务
    /// </summary>
    /// <param name="server_port">服务器端口号</param>
    /// <param name="mk_tcp_session">会话处理对象</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_session_manager(ushort server_port, IntPtr mk_tcp_session);

    /// <summary>
    /// 一般由于客户端断开tcp触发
    /// </summary>
    /// <param name="server_port">服务器端口号</param>
    /// <param name="mk_tcp_session">会话处理对象</param>
    /// <param name="code">错误代码</param>
    /// <param name="msg">错误提示</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_session_disconnect(ushort server_port,
                                                       IntPtr mk_tcp_session,
                                                       int code,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string msg);


    /// <summary>
    /// tcp客户端连接服务器成功或失败回调
    /// </summary>
    /// <param name="mk_tcp_client">tcp客户端</param>
    /// <param name="code">0为连接成功，否则为失败原因</param>
    /// <param name="msg">连接失败错误提示</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_client_connect(IntPtr mk_tcp_client,
                                                   int code,
                                                   [In()][MarshalAs(UnmanagedType.LPStr)] string msg);


    /// <summary>
    /// tcp客户端与tcp服务器之间断开回调
    /// </summary>
    /// <param name="mk_tcp_client">tcp客户端</param>
    /// <param name="code">错误代码</param>
    /// <param name="msg">错误提示</param>
    /// <remarks>一般是eof事件导致</remarks>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_client_disconnect(IntPtr mk_tcp_client,
                                                        int code,
                                                        [In()][MarshalAs(UnmanagedType.LPStr)] string msg);

    /// <summary>
    /// 收到tcp服务器发来的数据
    /// </summary>
    /// <param name="mk_tcp_client">tcp客户端</param>
    /// <param name="data">数据指针</param>
    /// <param name="len">数据长度</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_tcp_client_data(IntPtr mk_tcp_client,
                                                  [In()][MarshalAs(UnmanagedType.LPStr)] string data,
                                                  int len);

    /// <summary>
    /// 每隔2秒的定时器，用于管理超时等任务
    /// </summary>
    /// <param name="mk_tcp_client"></param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    public delegate void on_mk_tcp_client_manager(System.IntPtr mk_tcp_client);

    internal enum mk_tcp_type
    {
        /// <summary>
        /// 普通的tcp
        /// </summary>
        [Description("普通的tcp")]
        mk_type_tcp = 0,
        /// <summary>
        /// ssl类型的tcp
        /// </summary>
        [Description("ssl类型的tcp")]
        mk_type_ssl = 1,
        /// <summary>
        /// 基于webSock的连接
        /// </summary>
        [Description("基于webSock的连接")]
        mk_type_ws = 2,
        /// <summary>
        /// 基于ssl webSock的连接
        /// </summary>
        [Description("基于ssl webSock的连接")]
        mk_type_wss = 3
    }
    /// <summary>
    /// TCP Sessions事件
    /// </summary>
    [StructLayout(ZLMediaKitMethod.LayoutKind)]
    internal struct mk_tcp_session_events
    {
        /// <summary>
        /// <inheritdoc cref="on_mk_tcp_session_create"/>
        /// <para>详情请看<seealso cref="on_mk_tcp_session_create"/> </para>
        /// </summary>
        internal on_mk_tcp_session_create OnCreate;

        /// <summary>
        /// <inheritdoc cref="on_mk_tcp_session_data"/>
        /// <para>详情请看<seealso cref="on_mk_tcp_session_data"/></para>
        /// </summary>
        internal on_mk_tcp_session_data OnData;

        /// <summary>
        /// <inheritdoc cref="on_mk_tcp_session_manager"/>
        /// <para>详情请看 <seealso cref="on_mk_tcp_session_manager"/></para>
        /// </summary>
        internal on_mk_tcp_session_manager OnManager;

        /// <summary>
        /// <inheritdoc cref="on_mk_tcp_session_disconnect"/>
        /// <para>详情请看<seealso cref="on_mk_tcp_session_disconnect"/></para>
        /// </summary>
        internal on_mk_tcp_session_disconnect OnDisconnect;
    }

    /// <summary>
    /// TCP客户端事件
    /// </summary>
    [StructLayout(ZLMediaKitMethod.LayoutKind)]
    internal struct mk_tcp_client_events
    {

        /// <summary>
        /// <inheritdoc cref="_on_mk_tcp_client_connect"/>
        /// <para>详情请看<seealso cref="_on_mk_tcp_client_connect"/></para>
        /// </summary>
        internal on_mk_tcp_client_connect OnConnect;

        /// <summary>
        /// <inheritdoc cref="_on_mk_tcp_client_disconnect"/>
        /// <para>详情请看<seealso cref="_on_mk_tcp_client_disconnect"/></para>
        /// </summary>
        internal on_mk_tcp_client_disconnect OnDisconnect;

        /// <summary>
        /// <inheritdoc cref="_on_mk_tcp_client_data"/>
        /// <para>详情请看<seealso cref="_on_mk_tcp_client_data"/></para>
        /// </summary>
        internal on_mk_tcp_client_data OnData;

        /// <summary>
        /// <inheritdoc cref="_on_mk_tcp_client_manager"/>
        /// <para>详情请看 <seealso cref="_on_mk_tcp_client_manager"/></para>
        /// </summary>
        internal on_mk_tcp_client_manager OnManager;
    }

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 获取TCP Sock客户端IP
        /// </summary>
        /// <param name="mk_sock_info">tcp Sock对象指针</param>
        /// <param name="buf"></param>
        /// <returns>IP信息</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_sock_info_peer_ip(IntPtr mk_sock_info, IntPtr buf);

        /// <summary>
        /// 获取TCP Sock 本地IP
        /// </summary>
        /// <param name="mk_sock_info">Tcp Sock对象指针</param>
        /// <param name="buf"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_sock_info_local_ip(IntPtr mk_sock_info, IntPtr buf);

        /// <summary>
        /// 获取TCP Sock 客户端端口
        /// </summary>
        /// <param name="mk_sock_info">Tcp Sock对象指针</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_sock_info_peer_port(IntPtr mk_sock_info);

        /// <summary>
        /// 获取TCP Sock 本地端口
        /// </summary>
        /// <param name="mk_sock_info">Tcp Sock对象指针</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_sock_info_local_port(IntPtr mk_sock_info);

        /// <summary>
        /// 获取TCP Session 客户端IP
        /// </summary>
        /// <param name="mk_tcp_session">TCP Session对象</param>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static IntPtr mk_tcp_session_peer_ip(IntPtr mk_tcp_session, IntPtr buf)
            => mk_sock_info_peer_ip(mk_tcp_session_get_sock_info(mk_tcp_session), buf);

        /// <summary>
        /// 获取TCP Session 本地IP
        /// </summary>
        /// <param name="mk_tcp_session">Session对象</param>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static IntPtr mk_tcp_session_local_ip(IntPtr mk_tcp_session, IntPtr buf)
            => mk_sock_info_local_ip(mk_tcp_session_get_sock_info(mk_tcp_session), buf);

        /// <summary>
        /// 获取TCP Session 客户端端口
        /// </summary>
        /// <param name="mk_tcp_session">Session对象</param>
        /// <returns></returns>
        internal static ushort mk_tcp_session_peer_port(IntPtr mk_tcp_session)
            => mk_sock_info_peer_port(mk_tcp_session_get_sock_info(mk_tcp_session));

        /// <summary>
        /// 获取TCP Session本地端口
        /// </summary>
        /// <param name="mk_tcp_session">Session对象</param>
        /// <returns></returns>
        internal static ushort mk_tcp_session_local_port(IntPtr mk_tcp_session)
            => mk_sock_info_local_port(mk_tcp_session_get_sock_info(mk_tcp_session));

        /// <summary>
        /// 获取TCP 客户端 对端IP
        /// </summary>
        /// <param name="mk_tcp_client">TCP客户端对象</param>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static IntPtr mk_tcp_client_peer_ip(IntPtr mk_tcp_client, IntPtr buf)
            => mk_sock_info_peer_ip(mk_tcp_client_get_sock_info(mk_tcp_client), buf);

        /// <summary>
        /// 获取TCP客户端本地IP
        /// </summary>
        /// <param name="mk_tcp_client">TCP客户端对象</param>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static IntPtr mk_tcp_client_local_ip(IntPtr mk_tcp_client, IntPtr buf)
           => mk_sock_info_local_ip(mk_tcp_client_get_sock_info(mk_tcp_client), buf);

        /// <summary>
        /// 获取TCP客户端对端端口
        /// </summary>
        /// <param name="mk_tcp_client">TCP对象</param>
        /// <returns></returns>
        internal static ushort mk_tcp_client_peer_port(IntPtr mk_tcp_client)
           => mk_sock_info_peer_port(mk_tcp_client_get_sock_info(mk_tcp_client));

        /// <summary>
        /// 获取TCP客户端本地端口
        /// </summary>
        /// <param name="mk_tcp_client">TCP对象</param>
        /// <returns></returns>
        internal static ushort mk_tcp_client_local_port(IntPtr mk_tcp_client)
          => mk_sock_info_local_port(mk_tcp_client_get_sock_info(mk_tcp_client));

        /// <summary>
        /// 获取TCP Sock 对象指针
        /// </summary>
        /// <param name="mk_tcp_session">TCP Session 对象指针</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern System.IntPtr mk_tcp_session_get_sock_info(IntPtr mk_tcp_session);


       /// <summary>
       /// 关闭一个TCP Session连接
       /// </summary>
       /// <param name="mk_tcp_session">TCP Session 指针</param>
       /// <param name="err">错误码</param>
       /// <param name="err_msg">错误消息</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_session_shutdown(IntPtr mk_tcp_session,
                                                            int err,
                                                            [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg);


       /// <summary>
       /// 发送数据
       /// </summary>
       /// <param name="mk_tcp_session">Tcp Session对象指针</param>
       /// <param name="data">数据</param>
       /// <param name="len">数据长度</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_session_send(IntPtr mk_tcp_session, [In()][MarshalAs(UnmanagedType.LPStr)] string data, int len);



        /// <summary>
        /// 切换到该对象所在线程后再TcpSession::send()
        /// </summary>
        /// <param name="mk_tcp_session">tcp session 对象指针</param>
        /// <param name="data">待发送的数据</param>
        /// <param name="len">待发送的数据长度</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_session_send_safe(IntPtr mk_tcp_session,
                                                             [In()][MarshalAs(UnmanagedType.LPStr)] string data,
                                                             int len);


        /// <summary>
        /// 为TCP session 对象设置标记数据
        /// </summary>
        /// <param name="mk_tcp_session"></param>
        /// <param name="user_data">自定义标记数据</param>
        /// <remarks>该函数只对 <see cref="mk_tcp_server_start"/> 启动的服务类型有效</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_session_set_user_data(IntPtr mk_tcp_session, IntPtr user_data);


        /// <summary>
        /// 获取  mk_tcp_session_set_user_data 中设置的标记数据
        /// </summary>
        /// <param name="mk_tcp_session">Tcp session对象</param>
        /// <returns></returns>
        /// <remarks>该函数只对 <see cref="mk_tcp_server_start"/> 启动的服务类型有效</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern System.IntPtr mk_tcp_session_get_user_data(IntPtr mk_tcp_session);


        /// <summary>
        /// 开启tcp服务器
        /// </summary>
        /// <param name="port">监听端口号，0则为随机</param>
        /// <param name="type">服务器类型</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_tcp_server_start(ushort port, TcpType type);


        /// <summary>
        /// 监听tcp服务器事件
        /// </summary>
        /// <param name="events"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_server_events_listen(ref mk_tcp_session_events events);


        /// <summary>
        /// 获取基类指针以便获取其网络相关信息
        /// </summary>
        /// <param name="mk_tcp_client"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern System.IntPtr mk_tcp_client_get_sock_info(IntPtr mk_tcp_client);


        /// <summary>
        /// 创建tcp客户端
        /// </summary>
        /// <param name="events">回调函数结构体</param>
        /// <param name="type">客户端类型</param>
        /// <returns>客户端对象</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern System.IntPtr mk_tcp_client_create(ref mk_tcp_client_events events, TcpType type);


        /// <summary>
        /// 释放tcp客户端
        /// </summary>
        /// <param name="mk_tcp_client"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_client_release(IntPtr mk_tcp_client);



        /// <summary>
        /// 发起连接
        /// </summary>
        /// <param name="mk_tcp_client">客户端对象</param>
        /// <param name="host">服务器ip或域名</param>
        /// <param name="port">服务器端口号</param>
        /// <param name="time_out_sec">超时时间</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_client_connect(IntPtr mk_tcp_client,
                                                          [In()][MarshalAs(UnmanagedType.LPStr)] string host,
                                                          ushort port,
                                                          float time_out_sec);


        /// <summary>
        /// 非线程安全的发送数据
        /// </summary>
        /// <param name="mk_tcp_client">客户端对象</param>
        /// <param name="data">数据指针</param>
        /// <param name="len">数据长度，等于0时，内部通过strlen获取</param>
        /// <remarks>开发者如果能确保在本对象网络线程内，可以调用此此函数</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_client_send(IntPtr mk_tcp_client,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string data,
                                                       int len);

        /// <summary>
        /// 切换到本对象的网络线程后再发送数据
        /// </summary>
        /// <param name="mk_tcp_client">客户端对象</param>
        /// <param name="data">数据指针</param>
        /// <param name="len">数据长度，等于0时，内部通过strlen获取</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_client_send_safe(System.IntPtr mk_tcp_client,
                                                            [In()][MarshalAsAttribute(UnmanagedType.LPStr)] string data,
                                                            int len);


        /// <summary>
        /// 客户端附着用户数据
        /// </summary>
        /// <param name="mk_tcp_client">客户端对象</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_tcp_client_set_user_data(IntPtr mk_tcp_client, System.IntPtr user_data);


        /// <summary>
        /// 获取客户端对象上附着的用户数据
        /// </summary>
        /// <param name="mk_tcp_client">客户端对象</param>
        /// <returns>用户数据指针</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_tcp_client_get_user_data(IntPtr mk_tcp_client);


    }
}
