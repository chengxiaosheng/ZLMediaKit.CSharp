using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{

    /// <summary>
    /// 播放结果或播放中断事件的回调
    /// </summary>
    /// <param name="user_data">用户数据指针</param>
    /// <param name="err_code">错误代码，0为成功</param>
    /// <param name="err_msg">错误提示</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void on_mk_play_event(IntPtr user_data, int err_code, [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg);


    /// <summary>
    /// 收到音视频数据回调
    /// </summary>
    /// <param name="user_data">用户数据指针</param>
    /// <param name="track_type">0：视频，1：音频</param>
    /// <param name="codec_id"> 0：H264，1：H265，2：AAC 3.G711A 4.G711U 5.OPUS</param>
    /// <param name="data">数据指针</param>
    /// <param name="len">数据长度</param>
    /// <param name="dts">解码时间戳，单位毫秒</param>
    /// <param name="pts">显示时间戳，单位毫秒</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void on_mk_play_data(IntPtr user_data, int track_type, int codec_id, IntPtr data, int len, uint dts, uint pts);

    internal partial class ZLMediaKitMethod
    {



        /// <summary>
        /// 创建一个播放器,支持rtmp[s]/rtsp[s]
        /// </summary>
        /// <returns>播放器指针 mk_player</returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_player_create();


        /// <summary>
        /// 销毁播放器
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_release(IntPtr mk_player);


        /// <summary>
        /// 设置播放器配置选项
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="key">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="val">配置项值,如果是整形，需要转换成统一转换成string</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_set_option(IntPtr mk_player,
                                                         [In()][MarshalAsAttribute(UnmanagedType.LPStr)] string key,
                                                         [In()][MarshalAsAttribute(UnmanagedType.LPStr)] string val);

        /// <summary>
        /// 开始播放url
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="url">rtsp[s]/rtmp[s] url</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_play(IntPtr mk_player, [In()][MarshalAsAttribute(UnmanagedType.LPStr)] string url);


        /// <summary>
        /// 暂停或恢复播放，仅对点播有用
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="pause">1:暂停播放，0：恢复播放</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_pause(IntPtr mk_player, int pause);


        /// <summary>
        /// 设置点播进度条
        /// </summary>
        /// <param name="mk_player">对象指针</param>
        /// <param name="progress">取值范围未 0.0～1.0</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_seekto(IntPtr mk_player, float progress);


        /// <summary>
        /// 设置播放器开启播放结果回调函数
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="callback">回调函数指针,设置null立即取消回调</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_set_on_result(IntPtr mk_player, on_mk_play_event callback, IntPtr user_data);


        /// <summary>
        /// 设置播放被异常中断的回调
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="callback">回调函数指针,设置null立即取消回调</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_set_on_shutdown(IntPtr mk_player, on_mk_play_event callback, IntPtr user_data);


        /// <summary>
        /// 设置音视频数据回调函数
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <param name="callback">回调函数指针,设置null立即取消回调</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_player_set_on_data(IntPtr mk_player, on_mk_play_data callback, IntPtr user_data);


        /// <summary>
        /// 获取视频codec_id -1：不存在 0：H264，1：H265，2：AAC 3.G711A 4.G711U
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_video_codecId(IntPtr mk_player);


        /// <summary>
        /// 获取视频宽度
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_video_width(IntPtr mk_player);


        /// <summary>
        /// 获取视频高度
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_video_height(IntPtr mk_player);


        /// <summary>
        /// 获取视频帧率
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_video_fps(IntPtr mk_player);


        /// <summary>
        /// 获取音频codec_id -1：不存在 0：H264，1：H265，2：AAC 3.G711A 4.G711U
        /// </summary>
        /// <param name="mk_player">播放器指针</param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_audio_codecId(IntPtr mk_player);


        /// <summary>
        /// 获取音频采样率
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_audio_samplerate(IntPtr mk_player);


        /// <summary>
        /// 获取音频采样位数，一般为16
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_audio_bit(IntPtr mk_player);


        /// <summary>
        /// 获取音频通道数
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_player_audio_channel(IntPtr mk_player);


        /// <summary>
        /// 获取点播节目时长，如果是直播返回0，否则返回秒数
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern float mk_player_duration(IntPtr mk_player);


        /// <summary>
        /// 获取点播播放进度，取值范围未 0.0～1.0
        /// </summary>
        /// <param name="mk_player"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern float mk_player_progress(IntPtr mk_player);


        /// <summary>
        /// 获取丢包率，rtsp时有效
        /// </summary>
        /// <param name="mk_player">对象指针</param>
        /// <param name="track_type"> 0：视频，1：音频</param>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern float mk_player_loss_rate(IntPtr mk_player, int track_type);
    }
}
