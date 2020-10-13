using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    /// <summary>
    /// MediaSource.close()回调事件
    /// </summary>
    /// <remarks>在选择关闭一个关联的MediaSource时，将会最终触发到该回调
    /// <para>你应该通过该事件调用<seealso cref="mk_proxy_player_release"/>函数并且释放其他资源</para>
    /// <para>如果你不调用<seealso cref="mk_proxy_player_release"/>函数，那么MediaSource.close()操作将无效</para></remarks>
    /// <param name="user_data">用户数据指针，通过<seealso cref="mk_proxy_player_set_on_close"/>函数设置</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    public delegate void on_mk_proxy_player_close(IntPtr user_data);

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 创建一个代理播放器
        /// </summary>
        /// <param name="vhost">虚拟主机名，一般为__defaultVhost__</param>
        /// <param name="app">应用名</param>
        /// <param name="stream">流名</param>
        /// <param name="hls_enabled">是否生成hls</param>
        /// <param name="mp4_enabled">是否生成mp4</param>
        /// <returns>对象指针 mk_proxy_player</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_proxy_player_create([In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                                    [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                                    [In()][MarshalAs(UnmanagedType.LPStr)] string stream,
                                                                    int hls_enabled,
                                                                    int mp4_enabled);


        /// <summary>
        /// 销毁代理播放器
        /// </summary>
        /// <param name="mk_proxy_player">对象指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_proxy_player_release(IntPtr mk_proxy_player);


        /// <summary>
        /// 设置代理播放器配置选项
        /// </summary>
        /// <param name="mk_proxy_player">代理播放器指针</param>
        /// <param name="key">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="val">配置项值,如果是整形，需要转换成统一转换成string</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_proxy_player_set_option(IntPtr mk_proxy_player,
                                                               [In()][MarshalAs(UnmanagedType.LPStr)] string key,
                                                               [In()][MarshalAs(UnmanagedType.LPStr)] string val);


        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="mk_proxy_player">对象指针</param>
        /// <param name="url">播放url,支持rtsp/rtmp</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_proxy_player_play(IntPtr mk_proxy_player,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string url);


        /// <summary>
        /// 监听MediaSource.close()事件
        /// </summary>
        /// <param name="mk_proxy_player">对象指针</param>
        /// <param name="callback">回调指针</param>
        /// <param name="user_data">用户数据指针</param>
        /// <remarks><para>在选择关闭一个关联的MediaSource时，将会最终触发到该回调</para><para>你应该通过该事件调用mk_proxy_player_release函数并且释放其他资源</para></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_proxy_player_set_on_close(IntPtr mk_proxy_player,
                                                                 on_mk_proxy_player_close callback,
                                                                 IntPtr user_data);


        /// <summary>
        /// 获取总的观看人数
        /// </summary>
        /// <param name="mk_proxy_player">对象指针</param>
        /// <returns>观看人数</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_proxy_player_total_reader_count(IntPtr mk_proxy_player);

    }
}
