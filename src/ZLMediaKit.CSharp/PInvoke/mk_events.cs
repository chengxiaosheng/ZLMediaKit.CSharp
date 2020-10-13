using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{

    /// <summary>
    /// 注册或反注册MediaSource事件广播
    /// </summary>
    /// <param name="regist">注册为1，注销为0</param>
    /// <param name="mk_media_source">该MediaSource对象</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_changed(int regist, IntPtr mk_media_source);

    /// <summary>
    /// 收到rtsp/rtmp推流事件广播，通过该事件控制推流鉴权
    /// <see cref="mk_events_objects.mk_publish_auth_invoker_do"/>
    /// </summary>
    /// <param name="mk_media_info">推流url相关信息</param>
    /// <param name="mk_publish_auth_invoker">执行invoker返回鉴权结果</param>
    /// <param name="mk_sock_info">该tcp客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_publish(IntPtr mk_media_info, IntPtr mk_publish_auth_invoker, IntPtr mk_sock_info);

    /// <summary>
    /// 播放rtsp/rtmp/http-flv/hls事件广播，通过该事件控制播放鉴权
    /// <see cref="mk_events_objects.mk_auth_invoker_do"/>
    /// </summary>
    /// <param name="mk_media_info">播放url相关信息</param>
    /// <param name="mk_auth_invoker">执行invoker返回鉴权结果</param>
    /// <param name="mk_sock_info">播放客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_play(IntPtr mk_media_info, IntPtr mk_auth_invoker, IntPtr mk_sock_info);

    /// <summary>
    /// 未找到流后会广播该事件，请在监听该事件后去拉流或其他方式产生流，这样就能按需拉流了
    /// </summary>
    /// <param name="mk_media_info">播放url相关信息</param>
    /// <param name="mk_sock_info">播放客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_not_found(IntPtr mk_media_info, IntPtr mk_sock_info);

    /// <summary>
    /// 某个流无人消费时触发，目的为了实现无人观看时主动断开拉流等业务逻辑
    /// </summary>
    /// <param name="mk_media_source">该MediaSource对象</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_no_reader(IntPtr mk_media_source);

    /// <summary>
    /// 收到http api请求广播(包括GET/POST)
    /// </summary>
    /// <param name="mk_parser">http请求内容对象</param>
    /// <param name="mk_http_response_invoker">执行该invoker返回http回复</param>
    /// <param name="consumed">置1则说明我们要处理该事件</param>
    /// <param name="mk_sock_info">http客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_http_request(IntPtr mk_parser, IntPtr mk_http_response_invoker, ref int consumed, IntPtr mk_sock_info);

    /// <summary>
    /// 在http文件服务器中,收到http访问文件或目录的广播,通过该事件控制访问http目录的权限
    /// </summary>
    /// <param name="mk_parser">http请求内容对象</param>
    /// <param name="path">文件绝对路径</param>
    /// <param name="is_dir">path是否为文件夹</param>
    /// <param name="mk_http_access_path_invoker">执行invoker返回本次访问文件的结果</param>
    /// <param name="mk_sock_info">http客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_http_access(IntPtr mk_parser,
                                            [In()][MarshalAs(UnmanagedType.LPStr)] string path,
                                            int is_dir,
                                            IntPtr mk_http_access_path_invoker,
                                            IntPtr mk_sock_info);

    /// <summary>
    /// 在http文件服务器中,收到http访问文件或目录前的广播,通过该事件可以控制http url到文件路径的映射
    /// </summary>
    /// <param name="mk_parser">http请求内容对象</param>
    /// <param name="path">文件绝对路径,覆盖之可以重定向到其他文件</param>
    /// <param name="mk_sock_info">http客户端相关信息</param>
    /// <remarks>在该事件中通过自行覆盖path参数，可以做到譬如根据虚拟主机或者app选择不同http根目录的目的</remarks>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_http_before_access(IntPtr mk_parser,
                                                    [In()][Out()][MarshalAs(UnmanagedType.LPStr)] ref string path,
                                                    IntPtr mk_sock_info);

    /// <summary>
    /// 该rtsp流是否需要认证？是的话调用invoker并传入realm,否则传入空的realm
    /// </summary>
    /// <param name="mk_media_info">请求rtsp url相关信息</param>
    /// <param name="mk_rtsp_get_realm_invoker">执行invoker返回是否需要rtsp专属认证</param>
    /// <param name="mk_sock_info">rtsp客户端相关信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_rtsp_get_realm(IntPtr mk_media_info, IntPtr mk_rtsp_get_realm_invoker, IntPtr mk_sock_info);

    /// <summary>
    /// 请求认证用户密码事件，user_name为用户名，must_no_encrypt如果为true，则必须提供明文密码(因为此时是base64认证方式),否则会导致认证失败;
    /// 获取到密码后请调用invoker并输入对应类型的密码和密码类型，invoker执行时会匹配密码
    /// </summary>
    /// <param name="mk_media_info">请求rtsp url相关信息</param>
    /// <param name="realm">rtsp认证realm</param>
    /// <param name="user_name">rtsp认证用户名</param>
    /// <param name="must_no_encrypt">如果为true，则必须提供明文密码(因为此时是base64认证方式),否则会导致认证失败</param>
    /// <param name="mk_rtsp_auth_invoker">执行invoker返回rtsp专属认证的密码</param>
    /// <param name="mk_sock_info">rtsp客户端信息</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_rtsp_auth(IntPtr mk_media_info,
                                          [In()][MarshalAs(UnmanagedType.LPStr)] string realm,
                                          [In()][MarshalAs(UnmanagedType.LPStr)] string user_name,
                                          int must_no_encrypt,
                                          IntPtr mk_rtsp_auth_invoker,
                                          IntPtr mk_sock_info);

    /// <summary>
    /// 录制mp4分片文件成功后广播
    /// </summary>
    /// <param name="mk_mp4_info"></param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_record_mp4(IntPtr mk_mp4_info);

    /// <summary>
    /// shell登录鉴权
    /// </summary>
    /// <param name="user_name">用户名</param>
    /// <param name="passwd">密码</param>
    /// <param name="mk_auth_invoker"></param>
    /// <param name="mk_sock_info"></param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_shell_login([In()][MarshalAs(UnmanagedType.LPStr)] string user_name,
                                            [In()][MarshalAs(UnmanagedType.LPStr)] string passwd,
                                            IntPtr mk_auth_invoker,
                                            IntPtr mk_sock_info);

    /// <summary>
    /// 停止rtsp/rtmp/http-flv会话后流量汇报事件广播
    /// </summary>
    /// <param name="mk_media_info">播放url相关信息</param>
    /// <param name="total_bytes">耗费上下行总流量，单位字节数</param>
    /// <param name="total_seconds">本次tcp会话时长，单位秒</param>
    /// <param name="is_player">客户端是否为播放器</param>
    /// <param name="mk_sock_info"></param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_flow_report(IntPtr mk_media_info, uint total_bytes, uint total_seconds, int is_player, IntPtr mk_sock_info);


    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct mk_events
    {
        /// <summary>
        /// <inheritdoc cref="on_mk_media_changed"/>
        /// </summary>
        internal on_mk_media_changed OnMediaChanged;

        /// <summary>
        /// <inheritdoc cref="on_mk_media_publish"/>
        /// </summary>
        internal on_mk_media_publish OnMediaPublish;

        /// <summary>
        /// <inheritdoc cref="on_mk_media_play"/>
        /// </summary>
        internal on_mk_media_play OnMediaPlay;

        /// <summary>
        /// <inheritdoc cref="on_mk_media_not_found"/>
        /// </summary>
        internal on_mk_media_not_found OnMediaNotFound;

        /// <summary>
        /// <inheritdoc cref="on_mk_media_no_reader"/>
        /// </summary>
        internal on_mk_media_no_reader OnMediaNoReader;

        /// <summary>
        /// <inheritdoc cref="on_mk_http_request"/>
        /// </summary>
        internal on_mk_http_request OnHttpRequest;

        /// <summary>
        /// <inheritdoc cref="on_mk_http_access"/>
        /// </summary>
        internal on_mk_http_access OnHttpAccess;

        /// <summary>
        /// <inheritdoc cref="on_mk_http_before_access"/>
        /// </summary>
        internal on_mk_http_before_access OnHttpBeforeAccess;

        /// <summary>
        /// <inheritdoc cref="on_mk_rtsp_get_realm"/>
        /// </summary>
        internal on_mk_rtsp_get_realm OnRtspGetRealm;

        /// <summary>
        /// <inheritdoc cref="on_mk_rtsp_auth"/>
        /// </summary>
        internal on_mk_rtsp_auth OnRtspAuth;

        /// <summary>
        /// <inheritdoc cref="on_mk_record_mp4"/>
        /// </summary>
        internal on_mk_record_mp4 OnRecordMp4;

        /// <summary>
        /// <inheritdoc cref="on_mk_shell_login"/>
        /// </summary>
        internal on_mk_shell_login OnShellLogin;

        /// <summary>
        /// <inheritdoc cref="on_mk_flow_report"/>
        /// </summary>
        internal on_mk_flow_report OnFlowReport;
    }

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 监听ZLMediaKit里面的事件
        /// </summary>
        /// <param name="events">各个事件的结构体,这个对象在内部会再拷贝一次，可以设置为null以便取消监听</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_events_listen(ref mk_events events);

    }
}
