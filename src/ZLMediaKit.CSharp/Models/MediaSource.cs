using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLMediaKit.CSharp.Helper;

namespace ZLMediaKit.CSharp.Models
{
    /// <summary>
    /// 媒体源
    /// </summary>
    public class MediaSource
    {
        private IntPtr _id = IntPtr.Zero;

        private MediaSource() { }
        internal MediaSource(IntPtr id)
        {
            this._id = id;
        }

        /* 监听全局 OnPlay事件，获取到当前MediaSource的播放并填充到 WatchClient
         * 监听全局 OnFlow事件，获取当前MediaSource的播放流量报告事件 并填充WatchClient
         * OnPlay 事件触发在 OnMediaNotFound之前，所有还需要一个钩子在MediaSouce的上层 
         */

        /// <summary>
        /// 内部实现事件绑定
        /// </summary>
        /// <remarks>主要为了监听播放着</remarks>
        private void BindEvents()
        {

        }

        /// <summary>
        /// 协议
        /// </summary>
        public string Schema => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_source_get_schema(this._id));

        public MediaInfo MediaInfo { get; private set;}

        internal List<MediaPush> _mediaPushers = new List<MediaPush>();

        /// <summary>
        /// 推流器集合
        /// </summary>
        public IReadOnlyList<MediaPush> MediaPushers => _mediaPushers;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegistTime { get; internal set; }
        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime? UnRegistTime { get; internal set; }

        /// <summary>
        /// 创建一个推流器
        /// </summary>
        /// <returns></returns>
        public virtual MediaPush CreateMediaPusher()
        {
            
            if(!ZLMediaKitConst.PusherSechames.Any(a=>a == this.Schema))
            {
                throw new NotSupportedException("此协议不支持推流");
            }
            var pusher = new MediaPush(this.MediaInfo);
            this._mediaPushers.Add(pusher);
            return pusher;
        }

        /// <summary>
        /// 应用名称
        /// </summary>
        /// <remarks>此数据实际无意义，可从上层对象 MediaWapper中获取</remarks>
        internal string App => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_source_get_app(this._id));
        /// <summary>
        /// 虚拟主机
        /// </summary>
        /// <remarks>此数据实际无意义，可从上层对象 MediaWapper中获取</remarks>
        internal string VHost => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_source_get_vhost(this._id));
        /// <summary>
        /// 流Id
        /// </summary>
        /// <remarks>此数据实际无意义，可从上层对象 MediaWapper中获取</remarks>
        internal string StreamId => Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_source_get_stream(this._id));

        /// <summary>
        /// 当前观看人数
        /// </summary>
        public int ReaderCount => PInvoke.ZLMediaKitMethod.mk_media_source_get_reader_count(this._id);
        /// <summary>
        /// 总观看人数
        /// </summary>
        public int TotalReaderCount => PInvoke.ZLMediaKitMethod.mk_media_source_get_total_reader_count(this._id);

        /// <summary>
        /// 关闭该MediaSource
        /// </summary>
        /// <param name="force">是否强制关闭</param>
        public bool Close(bool force)
        {
            return PInvoke.ZLMediaKitMethod.mk_media_source_close(this._id, force ? 1: 0) == 1 ? true : false;
        }

        /// <summary>
        /// seek到指定时间 单位毫秒
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public bool SeekTo(uint stamp)
        {
            return PInvoke.ZLMediaKitMethod.mk_media_source_seek_to(this._id, stamp) == 1 ? true : false;
        }

       


    }
}
