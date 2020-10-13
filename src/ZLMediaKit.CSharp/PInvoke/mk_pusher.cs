using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    /// <summary>
    /// 推流结果或推流中断事件的回调
    /// </summary>
    /// <param name="user_data">用户数据指针</param>
    /// <param name="err_code">错误代码，0为成功</param>
    /// <param name="err_msg">错误提示</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    public delegate void on_mk_push_event(IntPtr user_data, int err_code, [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg);

    internal partial class ZLMediaKitMethod
    {

        /// <summary>
        /// 绑定的MediaSource对象并创建rtmp[s]/rtsp[s]推流器
        /// </summary>
        /// <param name="schema">绑定的MediaSource对象所属协议，支持rtsp/rtmp</param>
        /// <param name="vhost">绑定的MediaSource对象的虚拟主机，一般为__defaultVhost__</param>
        /// <param name="app">绑定的MediaSource对象的应用名，一般为live</param>
        /// <param name="stream">绑定的MediaSource对象的流id</param>
        /// <returns>对象指针 mk_pusher </returns>
        /// <remarks>MediaSource通过mk_media_create或mk_proxy_player_create生成 ; <para>该MediaSource对象必须已注册</para></remarks>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_pusher_create([In()][MarshalAs(UnmanagedType.LPStr)] string schema,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string stream);


        /// <summary>
        /// 释放推流器
        /// </summary>
        /// <param name="mk_pusher">推流器指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_pusher_release(IntPtr mk_pusher);


        /// <summary>
        /// 设置推流器配置选项
        /// </summary>
        /// <param name="mk_pusher">推流器指针</param>
        /// <param name="key">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="val">配置项值,如果是整形，需要转换成统一转换成string</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_pusher_set_option(IntPtr mk_pusher,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string key,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string val);


        /// <summary>
        /// 开始推流
        /// </summary>
        /// <param name="mk_pusher">推流器指针</param>
        /// <param name="url">推流地址，支持rtsp[s]/rtmp[s]</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_pusher_publish(IntPtr mk_pusher,
                                                      [In()][MarshalAs(UnmanagedType.LPStr)] string url);


        /// <summary>
        /// 设置推流器推流结果回调函数
        /// </summary>
        /// <param name="mk_pusher">推流器指针</param>
        /// <param name="cb">回调函数指针,不得为null</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_pusher_set_on_result(IntPtr mk_pusher, on_mk_push_event callback, IntPtr user_data);


        /// <summary>
        /// 设置推流被异常中断的回调
        /// </summary>
        /// <param name="mk_pusher">推流器指针</param>
        /// <param name="callback">回调函数指针,不得为null</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_pusher_set_on_shutdown(IntPtr mk_pusher, on_mk_push_event callback, IntPtr user_data);
    }
}
