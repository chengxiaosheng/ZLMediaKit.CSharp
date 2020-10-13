using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using ZLMediaKit.CSharp.dtos;
using ZLMediaKit.CSharp.Helper;
using ZLMediaKit.CSharp.ZLMediaKit;

namespace ZLMediaKit.CSharp.Models
{
    /// <summary>
    /// 收到客户端的seek请求时触发该回调
    /// </summary>
    /// <param name="stamp_ms"></param>
    /// <returns> True : 将处理此请求，False 忽略该请求 </returns>
    public delegate bool SeekTo(UInt32 stamp_ms);

    /// <summary>
    /// 定义此对象为 视频流的顶层对象
    /// 通过此类可实现输入帧数据生成流
    /// 外部推流时 将publish 事件转换为此对象
    /// 此对象可管理 MediaSource即转协议后的具体协议对象
    /// </summary>
    public class MediaSourceWapper : IDisposable
    {
        private bool disposedValue;
        private IntPtr _id = IntPtr.Zero;

        /// <summary>
        /// 网络套字节
        /// </summary>
        /// <remarks>只有通过推流方式注册的MediaSource才有；自行实例化创建不存在网络互动，所以没有</remarks>
        public SockInfo SockInfo { get; internal set; }

        /// <summary>
        /// 播放客户端
        /// </summary>
        /// <remarks>实验功能，不对</remarks>
        internal List<WatchClient> _watchClient { get; set; } = new List<WatchClient>();
        /// <summary>
        /// 播放客户端
        /// </summary>
        /// <remarks>实验功能，不对</remarks>
        public IReadOnlyList<WatchClient> WatchClient => _watchClient;


        internal List<MediaSource> _mediaSource { get; set; } = new List<MediaSource>();
        /// <summary>
        /// MediaSource 集合
        /// RTSP/RTMP/FLV/HLS
        /// </summary>
        public IReadOnlyList<MediaSource> MediaSource => _mediaSource;

        
        public RtpServer RtpServer { get; internal set; }


        /// <summary>
        /// 录制相关信息
        /// </summary>
        public MediaRecorder Recorder { get; internal set; }


        /// <summary>
        /// 创建录制
        /// </summary>
        /// <returns></returns>
        public virtual MediaRecorder CreateRecorder(RecorderType recorderType,string path)
        {
            //存在录制，直接返回现有录制
            if (this.Recorder is object) return this.Recorder;
            var record = new MediaRecorder(this.MediaInfo, path, recorderType);
            this.Recorder = record;
            return this.Recorder;
        }

        //禁止用户无参构造
        internal MediaSourceWapper() {
            BindEvnets();
        }
        internal MediaSourceWapper(IntPtr _id)
        {
            BindEvnets();

        }
        /// <summary>
        /// 初始化一个媒体源
        /// </summary>
        /// <param name="streamId">流id，例如camera</param>
        /// <param name="app">应用名，推荐为live</param>
        /// <param name="vhost">虚拟主机名，一般为__defaultVhost__</param>
        /// <param name="duration">时长(单位秒)，直播则为0</param>
        /// <param name="hls_enabled">是否生成hls</param>
        /// <param name="mp4_enabled">是否生成mp4</param>
        public MediaSourceWapper(string streamId, string app = "live", string vhost =  ZLMediaKitConst.DefaultVhost , float duration = 0,bool hls_enabled = false ,bool mp4_enabled = false)
        {
            this.MediaInfo = new MediaInfo
            {
                _app = app,
                _streamId = streamId,
                _vhost = vhost,
                _duration = duration,
                _enableHls = hls_enabled,
                _enableMp4 = mp4_enabled
            };
            CreateMedia(duration, hls_enabled, mp4_enabled);
        }

        /// <summary>
        /// 从 StreamNotFound 中的实例创建媒体
        /// </summary>
        /// <param name="duration">时长(单位秒)，直播则为0</param>
        /// <param name="hls_enabled">是否生成hls</param>
        /// <param name="mp4_enabled">是否生成mp4</param>
        public void CreateMedia(float duration = 0, bool hls_enabled = false, bool mp4_enabled = false)
        {
            if (this.MediaInfo == null) throw new InvalidDataException("MediaInfo对象为空");
            this.MediaInfo._duration = duration;
            this.MediaInfo._enableHls = hls_enabled;
            this.MediaInfo._enableMp4 = mp4_enabled;
            BindEvnets();
            this._id = PInvoke.ZLMediaKitMethod.mk_media_create(this.MediaInfo.VHost, this.MediaInfo.App, this.MediaInfo.StreamId, this.MediaInfo.Duration, this.MediaInfo.EnableHls ? 1 : 0, this.MediaInfo.EnableMp4 ? 1 : 0);
        }

        /// <summary>
        /// 开启一个RTP端口等待推流生成媒体源
        /// </summary>
        /// <param name="streamId">流名</param>
        /// <param name="rtpPort">rtp端口号，填0则随机获取</param>
        /// <param name="enableTcp">是否启用tcp,为true则同时开启UPD与TCP</param>
        public MediaSourceWapper(string streamId,ushort rtpPort,bool enableTcp = false)
        {
            if (this.RtpServer != null) throw new InvalidOperationException("操作不合法：当前对象已存在RtpServer实例");
            this.MediaInfo = new MediaInfo
            {
                _app = "rtp",
                _vhost = ZLMediaKitConst.DefaultVhost,
                _streamId = streamId
            };
            CreateMedia(rtpPort, enableTcp);
        }

        /// <summary>
        /// 从 StreamNotFound 中的实例创建媒体
        /// </summary>
        /// <param name="rtpPort"></param>
        /// <param name="enableTcp"></param>
        public void CreateMedia(ushort rtpPort, bool enableTcp = false)
        {
            if (this.MediaInfo == null) throw new InvalidDataException("MediaInfo对象为空");
            if (this.MediaInfo.App != "rtp") throw new InvalidDataException("App必须是rtp");
            BindEvnets();
            this.RtpServer = new RtpServer(this.MediaInfo.StreamId, rtpPort, enableTcp);
        }

        /// <summary>
        /// 代理拉流实例
        /// </summary>
        public ProxyPlayer ProxyPlayer { get; private set; }

        /// <summary>
        /// 通过代理的方式创建媒体源
        /// </summary>
        /// <param name="url">播放url,支持rtsp/rtmp</param>
        /// <param name="vhost">虚拟主机名，一般为__defaultVhost__</param>
        /// <param name="app">应用名</param>
        /// <param name="streamId">流名</param>
        /// <param name="proxyOptions">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="enableHls">是否生成hls</param>
        /// <param name="enableMp4">是否生成mp4</param>
        public MediaSourceWapper(string url,string vhost,string app ,string streamId,Dictionary<string,string> proxyOptions , bool enableHls = false,bool enableMp4 = false)
        {
            this.MediaInfo = new MediaInfo
            {
                _app = app,
                _vhost = vhost,
                _streamId = streamId
            };
            CreateMedia(url, proxyOptions, enableHls, enableMp4);
        }

        /// <summary>
        /// 从 StreamNotFound 中的实例创建媒体
        /// </summary>
        /// <param name="url">播放url,支持rtsp/rtmp/hls</param>
        /// <param name="proxyOptions">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="enableHls">是否生成hls</param>
        /// <param name="enableMp4">是否生成mp4</param>
        public void CreateMedia(string url, Dictionary<string, string> proxyOptions, bool enableHls = false, bool enableMp4 = false)
        {
            if (this.MediaInfo == null) throw new InvalidDataException("MediaInfo对象为空");
            this.ProxyPlayer = new ProxyPlayer(url, this.MediaInfo, proxyOptions, enableHls, enableMp4);
            //绑定关闭事件
            this.ProxyPlayer.OnClose += () =>
            {
                this.OnClose?.Invoke();
            };
        }

        /// <summary>
        /// 创建对象时绑定一些事件
        /// </summary>
        private void BindEvnets()
        {
            if(this._id != IntPtr.Zero)
            {
                _mk_media_close = new PInvoke.on_mk_media_close(mk_media_close);
                PInvoke.ZLMediaKitMethod.mk_media_set_on_close(_id, _mk_media_close, IntPtr.Zero);
                _mk_media_seek = new PInvoke.on_mk_media_seek(mk_media_seek);
                PInvoke.ZLMediaKitMethod.mk_media_set_on_seek(_id, _mk_media_seek, IntPtr.Zero);
            }

            ZLMEvents.OnMediaChanged += MediaChanged;
            ZLMEvents.OnInternalMediaPlay += GetMediaSourceWapper;
            ZLMEvents.OnRtspGetRealm += RtspGetRealm;
            ZLMEvents.OnRtspAuth += RtspAuth;
        }


        private void UnBindEvents()
        {
            ZLMEvents.OnMediaChanged -= MediaChanged;
            ZLMEvents.OnInternalMediaPlay -= GetMediaSourceWapper;
            ZLMEvents.OnRtspGetRealm -= RtspGetRealm;
            ZLMEvents.OnRtspAuth -= RtspAuth;
        }

        private MediaSourceWapper GetMediaSourceWapper(MediaInfo arg)
        {
            if (IsCurrentMedia(arg)) return this;
            return null;
        }

        private void MediaChanged(int regist,MediaSource mediaSource)
        {
            if(IsCurrentMedia(mediaSource))
            {
                if(regist == 1)
                {
                    this._mediaSource.Add(mediaSource);
                }
                //如果zlm对对象的指针不会发生改变，则不用处理结束时间等信息
            }
        }

        private bool IsCurrentMedia(MediaSource mediaSource) => this.MediaInfo.App == mediaSource.App && this.MediaInfo.VHost == mediaSource.VHost && this.MediaInfo.StreamId == mediaSource.StreamId;
        private bool IsCurrentMedia(MediaInfo mediaInfo) => this.MediaInfo.App == mediaInfo.App && this.MediaInfo.VHost == mediaInfo.VHost && this.MediaInfo.StreamId == mediaInfo.StreamId;


        /// <summary>
        /// 媒体信息
        /// </summary>
        public MediaInfo MediaInfo { get; internal set; }

        private PInvoke.on_mk_media_close _mk_media_close { get; set; }
        private PInvoke.on_mk_media_seek _mk_media_seek { get; set; }


        private void bindEvent()
        {
          
        }

        private void ValidInputOperation()
        {
            if (this._id == IntPtr.Zero)
                throw new InvalidOperationException("代理或主动拉流或推流模式下无法使用此功能");
        }
       

        /// <summary>
        /// 添加视频轨道
        /// </summary>
        /// <param name="track_id"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fps"></param>
        public void InitVideo(EM_Codec track_id, int width, int height, int fps)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_init_video(this._id, (int)track_id, width, height, fps);
        }

        public void InitAudio(EM_Codec track_id, int sample_rate, int channels, int sample_bit)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_init_audio(this._id, (int)track_id, sample_rate, channels, sample_bit);
        }

        public void InitComplete()
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_init_complete(this._id);
        }

        /// <summary>
        /// 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="data">单帧H264数据</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        public void InputH264(byte[] data,uint dts,uint pts)
        {
            ValidInputOperation();
            unsafe
            {
                fixed(void* buffer = data)
                {
                    InputH264((IntPtr)buffer, data.Length, dts, pts);
                }
            }
        }

        /// <summary>
        /// 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="data">单帧H264数据</param>
        /// <param name="len">单帧H264数据字节数</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        public unsafe void InputH264(IntPtr data,int len, uint dts, uint pts)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_input_h264(this._id, data, len, dts, pts);
        }

        /// <summary>
        /// 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="data">单帧H264数据</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        public void InputH265(byte[] data, uint dts, uint pts)
        {
            ValidInputOperation();
            unsafe
            {
                fixed(void * buffer = data)
                {
                    InputH265((IntPtr)buffer, data.Length,  dts, pts);
                }
            }
        }

        /// <summary>
        /// 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
        /// </summary>
        /// <param name="data">单帧H264数据</param>
        /// <param name="len">单帧H264数据字节数</param>
        /// <param name="dts">解码时间戳，单位毫秒</param>
        /// <param name="pts">播放时间戳，单位毫秒</param>
        public void InputH265(IntPtr data, int len, uint dts, uint pts)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_input_h265(this._id, data, len, dts, pts);
        }

        /// <summary>
        /// 输入单帧AAC音频(单独指定adts头)
        /// </summary>
        /// <param name="data">不包含adts头的单帧AAC数据</param>
        /// <param name="dts">时间戳，毫秒</param>
        /// <param name="adts">adts头，可以为null</param>
        public void InputAac(byte[] data,uint dts ,byte[] adts)
        {
            ValidInputOperation();
            unsafe
            {
                fixed(void * buffer = data)
                fixed(void * adtsBuffer = adts)
                {
                    InputAac((IntPtr)buffer, data.Length, dts, (IntPtr)adtsBuffer);
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">不包含adts头的单帧AAC数据</param>
        /// <param name="len">单帧AAC数据字节数</param>
        /// <param name="dts">时间戳，毫秒</param>
        /// <param name="adts">adts头，可以为null</param>
        public void InputAac(IntPtr data, int len, uint dts, IntPtr adts)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_input_aac(this._id, data, len, dts, adts);
        }

        /// <summary>
        /// 输入单帧PCM音频,启用ENABLE_FAAC编译时，该函数才有效
        /// </summary>
        /// <param name="data">单帧PCM数据</param>
        /// <param name="pts">时间戳，毫秒</param>
        public void InputPcm(byte [] data,uint pts)
        {
            ValidInputOperation();
            unsafe
            {
                fixed(void * buffer = data)
                {
                    InputPcm((IntPtr)buffer, data.Length, pts);
                }
            }
        }
        /// <summary>
        /// 输入单帧PCM音频,启用ENABLE_FAAC编译时，该函数才有效
        /// </summary>
        /// <param name="data">单帧PCM数据</param>
        /// <param name="len">单帧PCM数据字节数</param>
        /// <param name="pts">时间戳，毫秒</param>
        public void InputPcm(IntPtr data,int len, uint pts)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_input_pcm(this._id, data, len, pts);
        }

        /// <summary>
        /// 输入单帧OPUS/G711音频帧
        /// </summary>
        /// <param name="data">单帧音频数据</param>
        /// <param name="pts">时间戳，毫秒</param>
        public void InputAudio(byte[] data, uint pts)
        {
            ValidInputOperation();
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, buffer, data.Length);
                PInvoke.ZLMediaKitMethod.mk_media_input_audio(this._id, buffer, data.Length, pts);
            }
            finally
            {
                if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// 输入单帧OPUS/G711音频帧
        /// </summary>
        /// <param name="data">单帧音频数据</param>
        /// <param name="len">单帧音频数据字节数</param>
        /// <param name="pts">时间戳，毫秒</param>
        public void InputAudio(IntPtr data, int len, uint pts)
        {
            ValidInputOperation();
            PInvoke.ZLMediaKitMethod.mk_media_input_audio(this._id, data, len, pts);
        }

        /// <summary>
        /// 是否关闭此MediaSource
        /// </summary>
        public event Func<bool> OnClose;

        private void mk_media_close(IntPtr userdata)
        {
            if (OnClose == null) this.Dispose();
            else if(OnClose.Invoke())
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userdata"></param>
        /// <param name="stamp_ms"></param>
        /// <returns></returns>
        private int mk_media_seek(IntPtr userdata,uint stamp_ms)
        {
            //我也没有办法
            if (this.OnSeekTo == null) return 0;
            return OnSeekTo.Invoke(stamp_ms) ? 1:0;
        }

        /// <summary>
        /// 客户端请求 seek至的时间轴位置
        /// </summary>
        public event SeekTo OnSeekTo;


        public event Action<MediaSourceWapper> OnNoReader;

        /// <summary>
        /// 该rtsp流是否需要认证？是的话调用invoker并传入realm,否则传入空的realm
        /// </summary>
        public event Func<MediaSourceWapper, SockInfo, string> OnRtspRealm;

        private string RtspGetRealm(MediaInfo mediaInfo, SockInfo sockInfo)
        {
            if (!this.IsCurrentMedia(mediaInfo) || this.OnRtspRealm == null) return null;
            //没有密码验证就不要使用realm了 
            if (OnRtspAuth == null) return null;
            return OnRtspRealm.Invoke(this, sockInfo);
        }

        private string RtspAuth(RtspAuth rtspAuth)
        {
            if (!this.IsCurrentMedia(rtspAuth.MediaInfo)) return null;
            rtspAuth.MediaSourceWapper = this;
            return OnRtspAuth?.Invoke(rtspAuth);
        }

        public event Func<RtspAuth, string> OnRtspAuth;




        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                UnBindEvents();
                if(this._id != IntPtr.Zero)  PInvoke.ZLMediaKitMethod.mk_media_release(_id);
                if (this.RtpServer != null) this.RtpServer.Dispose();
                if (this.ProxyPlayer != null) this.ProxyPlayer.Dispose();
                this._mediaSource = null;
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~MediaSource()
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
