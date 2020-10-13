using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ZLMediaKit.CSharp.dtos;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.ZLMediaKit
{
    public class Player : IDisposable
    {
        internal IntPtr _id = IntPtr.Zero;
        private bool disposedValue;

        public Player()
        {
            this._id = PInvoke.ZLMediaKitMethod.mk_player_create();
            PInvoke.ZLMediaKitMethod.mk_player_set_on_result(_id, mk_play_event, _resultData);
            PInvoke.ZLMediaKitMethod.mk_player_set_on_shutdown(_id, mk_play_event, _shutdownData);
            PInvoke.ZLMediaKitMethod.mk_player_set_on_data(_id, mk_play_data, IntPtr.Zero);
        }

        internal Dictionary<string, string> _options = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> Options => _options;

        public void SetOptions(Dictionary<string,string> options)
        {
            foreach(var item in options)
            {
                this._options[item.Key] = item.Value;
                PInvoke.ZLMediaKitMethod.mk_player_set_option(_id, item.Key, item.Value);
            }
        }

        /// <summary>
        /// 播放状态
        /// </summary>
        public EM_PlayStatus Status { get; private set; } =  EM_PlayStatus.Init;

        private IntPtr _resultData = Marshal.AllocHGlobal(1);
        private IntPtr _shutdownData = Marshal.AllocHGlobal(1);

        public void Play(string url)
        {
            this.Status = EM_PlayStatus.Init;
            PInvoke.ZLMediaKitMethod.mk_player_play(_id, url);
        }

        public void Pause(bool pause= true)
        {
            if(this.Status == EM_PlayStatus.Playing && pause)
            {
                this.Status = EM_PlayStatus.Paused;
                PInvoke.ZLMediaKitMethod.mk_player_pause(_id, 1);
            }
            else if(!pause && this.Status == EM_PlayStatus.Paused )
            {
                this.Status = EM_PlayStatus.Playing;
                PInvoke.ZLMediaKitMethod.mk_player_pause(_id, 0);
            }

        }

        public void Seek(float progress)
        {
            PInvoke.ZLMediaKitMethod.mk_player_seekto(_id, progress);
        }

        public EM_Codec VideoCodec => (EM_Codec)PInvoke.ZLMediaKitMethod.mk_player_video_codecId(_id);

        public int Width => PInvoke.ZLMediaKitMethod.mk_player_video_width(_id);
        public int Height => PInvoke.ZLMediaKitMethod.mk_player_video_height(_id);
        public int Fps => PInvoke.ZLMediaKitMethod.mk_player_video_fps(_id);

        public EM_Codec AudioCodec =>(EM_Codec)PInvoke.ZLMediaKitMethod.mk_player_audio_codecId(_id);

        public int SampleRate => PInvoke.ZLMediaKitMethod.mk_player_audio_samplerate(_id);
        public int AudioBit => PInvoke.ZLMediaKitMethod.mk_player_audio_bit(_id);
        public int AudioChannel => PInvoke.ZLMediaKitMethod.mk_player_audio_channel(_id);
        public float Duration => PInvoke.ZLMediaKitMethod.mk_player_duration(_id);
        public float Progress => PInvoke.ZLMediaKitMethod.mk_player_progress(_id);

        private void mk_play_event(IntPtr userdata,int err_code,string err_msg)
        {
            if(userdata == this._resultData)
            {
                if (err_code == 0) this.Status = EM_PlayStatus.Playing;
                OnPlayResult?.Invoke(err_code, err_msg);
            }
            else if(userdata == this._shutdownData)
            {
                this.Status = EM_PlayStatus.Closed;
                OnShutdown?.Invoke(err_code, err_msg);
            }
        }

        public event Action<int, string> OnPlayResult;
        public event Action<int, string> OnShutdown;


        private void mk_play_data(IntPtr userdata,int track_type,int codec_id,IntPtr data,int len,uint dts,uint pts)
        {
            unsafe
            {
                var a  = new Span<byte>(data.ToPointer(), len).ToArray() ;
            }
            var mediaInfo = new MediaPlayInfo
            {
                Codec = (EM_Codec)codec_id,
                Data = data,
                Dts = dts,
                IsVideo = track_type == 0,
                Length = len,
                Pts = pts
            };
            if (this.OnDataAsync != null)
            {
                mediaInfo.FillData(true);
                _ = Task.Run(() => OnDataAsync.Invoke(mediaInfo));

            } else if(this.OnData != null)
            {
                OnData.Invoke(mediaInfo);
            }
        }

        /// <summary>
        /// 同步方法
        /// </summary>
        /// <remarks>不要在此事件中执行业务逻辑</remarks>
        public event Action<MediaPlayInfo> OnData;

        /// <summary>
        /// 异步方法，如果你处理不够快，可能会乱序
        /// 异步方法中取数据应该直接使用 DataBytes，Data可能已经被释放
        /// </summary>
        public event Action<MediaPlayInfo> OnDataAsync;

        public void Close()
        {
            PInvoke.ZLMediaKitMethod.mk_player_release(_id);
            this.Status = EM_PlayStatus.Closed;
        }



        public float LossRate(bool video = true)
        {
            return PInvoke.ZLMediaKitMethod.mk_player_loss_rate(_id, video ? 0 : 1);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                PInvoke.ZLMediaKitMethod.mk_player_release(_id);
                this.Status = EM_PlayStatus.Closed;
                Marshal.FreeHGlobal(this._resultData);
                Marshal.FreeHGlobal(this._shutdownData);
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~Player()
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
