using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{

    /// <summary>
    /// MediaSource.close()回调事件
    /// </summary>
    /// <param name="user_data">用户数据指针，通过mk_media_set_on_close函数设置</param>
    /// <remarks>在选择关闭一个关联的MediaSource时，将会最终触发到该回调<para>你应该通过该事件调用mk_media_release函数并且释放其他资源</para><para>如果你不调用mk_media_release函数，那么MediaSource.close()操作将无效</para></remarks>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_close(IntPtr user_data);

    /// <summary>
    /// 收到客户端的seek请求时触发该回调
    /// </summary>
    /// <param name="user_data">用户数据指针,通过mk_media_set_on_seek设置</param>
    /// <param name="stamp_ms">seek至的时间轴位置，单位毫秒</param>
    /// <returns>1代表将处理seek请求，0代表忽略该请求</returns>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate int on_mk_media_seek(IntPtr user_data, uint stamp_ms);

    /// <summary>
    /// 生成的MediaSource注册或注销事件
    /// </summary>
    /// <param name="user_data">设置回调时的用户数据指针</param>
    /// <param name="sender">生成的MediaSource对象</param>
    /// <param name="regist">1为注册事件，0为注销事件</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_media_source_regist(IntPtr user_data, IntPtr sender, int regist);

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 创建一个媒体源
        /// </summary>
        /// <param name="vhost">虚拟主机名，一般为__defaultVhost__</param>
        /// <param name="app">应用名，推荐为live</param>
        /// <param name="stream">流id，例如camera</param>
        /// <param name="duration">时长(单位秒)，直播则为0</param>
        /// <param name="rtsp_enabled">是否启用rtsp协议</param>
        /// <param name="rtmp_enabled">是否启用rtmp协议</param>
        /// <param name="hls_enabled">是否生成hls</param>
        /// <param name="mp4_enabled">是否生成mp4</param>
        /// <returns>mk_media</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_create([In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                             [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                             [In()][MarshalAs(UnmanagedType.LPStr)] string stream,
                                                             float duration,
                                                             //int rtsp_enabled,
                                                             //int rtmp_enabled,
                                                             int hls_enabled,
                                                             int mp4_enabled);

        /// <summary>
        /// 销毁媒体源
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_release(IntPtr mk_media);


        /// <summary>
        /// 添加视频轨道
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="track_id">0:CodecH264/1:CodecH265</param>
        /// <param name="width">视频宽度</param>
        /// <param name="height">视频高度</param>
        /// <param name="fps">视频fps</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_init_video(IntPtr mk_media, int track_id, int width, int height, int fps);


        /// <summary>
        /// 添加音频轨道
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="track_id"> 2:CodecAAC/3:CodecG711A/4:CodecG711U/5:OPUS</param>
        /// <param name="sample_rate">通道数</param>
        /// <param name="channels">采样位数，只支持16</param>
        /// <param name="sample_bit">采样率</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_init_audio(IntPtr mk_media, int track_id, int sample_rate, int channels, int sample_bit);

        /// <summary>
        /// 初始化h264/h265/aac完毕后调用此函数，
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <remarks>在单track(只有音频或视频)时，因为ZLMediaKit不知道后续是否还要添加track，所以会多等待3秒钟
        /// <para>如果产生的流是单Track类型，请调用此函数以便加快流生成速度，当然不调用该函数，影响也不大(会多等待3秒)</para></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_init_complete(IntPtr mk_media);


        /// <summary>
        /// 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="data">单帧H264数据</param>
        /// <param name="len">单帧H264数据字节数</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_input_h264(IntPtr mk_media, IntPtr data, int len, uint dts, uint pts);

        /// <summary>
        /// 输入单帧H265视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="data">单帧H265数据</param>
        /// <param name="len">单帧H265数据字节数</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_input_h265(IntPtr mk_media, IntPtr data, int len, uint dts, uint pts);

        /// <summary>
        /// 输入单帧AAC音频(单独指定adts头)
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="data">不包含adts头的单帧AAC数据</param>
        /// <param name="len">单帧AAC数据字节数</param>
        /// <param name="dts">时间戳，毫秒</param>
        /// <param name="adts">adts头，可以为null</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_input_aac(IntPtr mk_media, IntPtr data, int len, uint dts, IntPtr adts);


        /// <summary>
        /// 输入单帧PCM音频,启用ENABLE_FAAC编译时，该函数才有效
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="data">单帧PCM数据</param>
        /// <param name="len">单帧PCM数据字节数</param>
        /// <param name="pts">时间戳，毫秒</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_input_pcm(IntPtr mk_media, IntPtr data, int len, uint pts);

        /// <summary>
        /// 输入单帧OPUS/G711音频帧
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="data">单帧音频数据</param>
        /// <param name="len">单帧音频数据字节数</param>
        /// <param name="dts">时间戳，毫秒</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_input_audio(IntPtr mk_media, IntPtr data, int len, uint dts);

        /// <summary>
        /// 监听MediaSource.close()事件
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="callback">回调指针</param>
        /// <param name="user_data">用户数据指针</param>
        /// <remarks>在选择关闭一个关联的MediaSource时，将会最终触发到该回调;
        /// <para>你应该通过该事件调用<see cref="mk_media_release"/>函数并且释放其他资源</para></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_set_on_close(IntPtr mk_media, on_mk_media_close callback, IntPtr user_data);


        /// <summary>
        /// 监听播放器seek请求事件
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="callback">回调指针</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_set_on_seek(IntPtr mk_media, on_mk_media_seek callback, IntPtr user_data);

        /// <summary>
        /// 获取总的观看人数
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <returns>观看人数</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_media_total_reader_count(IntPtr mk_media);

        /// <summary>
        /// 设置MediaSource注册或注销事件回调函数
        /// </summary>
        /// <param name="mk_media">对象指针</param>
        /// <param name="callback">回调指针</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_set_on_regist(IntPtr mk_media, on_mk_media_source_regist callback, IntPtr user_data);
    }
}
