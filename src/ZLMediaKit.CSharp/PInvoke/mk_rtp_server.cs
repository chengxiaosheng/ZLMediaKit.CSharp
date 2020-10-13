using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    /// <summary>
    /// GB28181 RTP 服务器接收流超时时触发
    /// </summary>
    /// <param name="userdata">用户数据指针</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_rtp_server_detach(IntPtr userdata);

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 创建GB28181 RTP 服务器
        /// </summary>
        /// <param name="port">监听端口，0则为随机</param>
        /// <param name="enable_tcp">创建udp端口时是否同时监听tcp端口</param>
        /// <param name="stream_id">该端口绑定的流id</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_rtp_server_create(ushort port, int enable_tcp, [In()][MarshalAs(UnmanagedType.LPStr)] string stream_id);


        /// <summary>
        /// 销毁GB28181 RTP 服务器
        /// </summary>
        /// <param name="mk_rtp_server">服务器对象</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtp_server_release(IntPtr mk_rtp_server);

        /// <summary>
        /// 获取本地监听的端口号
        /// </summary>
        /// <param name="mk_rtp_server">服务器对象</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_rtp_server_port(IntPtr mk_rtp_server);

        /// <summary>
        /// 监听B28181 RTP 服务器接收流超时事件
        /// </summary>
        /// <param name="mk_rtp_server">服务器对象</param>
        /// <param name="callback">回调函数</param>
        /// <param name="user_data">回调函数用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtp_server_set_on_detach(IntPtr mk_rtp_server, on_mk_rtp_server_detach callback, IntPtr user_data);


    }
}
