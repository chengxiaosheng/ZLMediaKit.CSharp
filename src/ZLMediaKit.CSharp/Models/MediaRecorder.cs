using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public enum RecorderType : byte
    {
        Hls = 0,
        Mp4,
        Flv

    }
    public class MediaRecorder : IDisposable
    {
        private bool disposedValue;
        private IntPtr _id = IntPtr.Zero;

        internal MediaInfo MediaInfo { get; set; }

        /// <summary>
        /// 录制类型
        /// </summary>
        public RecorderType RecorderType { get; internal set; }

        /// <summary>
        /// 文件存放路径
        /// </summary>
        public string FilePath { get; internal set; }

        /// <summary>
        /// 开始录制时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 结束录制时间
        /// </summary>
        public DateTime StopTime { get; private set; }

        private bool _flvRecorder = false;

        /// <summary>
        /// 录制状态
        /// </summary>
        public bool IsRecording => this.RecorderType == RecorderType.Flv ? _flvRecorder :
            PInvoke.ZLMediaKitMethod.mk_recorder_is_recording((int)this.RecorderType, this.MediaInfo.VHost, this.MediaInfo.App, this.MediaInfo.StreamId) == 0 ? false : true;

        private MediaRecorder() { }
        internal MediaRecorder(MediaInfo mediaInfo,string filePath, RecorderType type = RecorderType.Flv)
        {
            this.MediaInfo = mediaInfo;
            this.FilePath = filePath;
            this.RecorderType = type;
            switch (type)
            {
                case RecorderType.Flv:
                    {
                        this._id = PInvoke.ZLMediaKitMethod.mk_flv_recorder_create();
                        if(this._id == IntPtr.Zero) throw new Exception("创建录制失败");
                        var result = PInvoke.ZLMediaKitMethod.mk_flv_recorder_start(_id, mediaInfo.VHost, mediaInfo.App, mediaInfo.StreamId, this.FilePath);
                        //录制开始
                        if(result == 0)
                        {
                            this.StartTime = DateTime.Now;
                            this._flvRecorder = true;
                        }
                        //录制失败，打开文件失败或该RtmpMediaSource不存在
                        else if(result == -1)
                        {
                            throw new Exception("录制失败，打开文件失败或该RtmpMediaSource不存在");
                        }
                    }
                    break;
                case RecorderType.Mp4:
                case RecorderType.Hls:
                    {
                        var result = PInvoke.ZLMediaKitMethod.mk_recorder_start((int)this.RecorderType, this.MediaInfo.VHost, this.MediaInfo.App, this.MediaInfo.StreamId, this.FilePath);
                        if(result == 1)
                        {
                            this.StartTime = DateTime.Now;

                        }
                    }
                    break;
                default:
                    throw new NotSupportedException("不支持的录制类型");
            }
        }

        /// <summary>
        /// 停止录制
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            if (this.RecorderType == RecorderType.Flv)
            {
                if (this._id == IntPtr.Zero) throw new NullReferenceException("Flv录制指针不存在");
                PInvoke.ZLMediaKitMethod.mk_flv_recorder_release(this._id);
                this.StopTime = DateTime.Now;
                return true;
            }
            else
            {
                var result = PInvoke.ZLMediaKitMethod.mk_recorder_stop((int)this.RecorderType, this.MediaInfo.VHost, this.MediaInfo.App, this.MediaInfo.StreamId) == 1;
                if (result) this.StopTime = DateTime.Now;
                return result;
            }
        }


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
                //销毁此实例时关闭Flv录制
                if (this._id != null) this.Stop();
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~FlvRecorder()
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
