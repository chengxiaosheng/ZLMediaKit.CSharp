using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ZLMediaKit.CSharp.Models
{
    /// <summary>
    /// 推流结果或推流中断事件的回调
    /// </summary>
    /// <param name="code">错误代码，0为成功</param>
    /// <param name="message">错误提示</param>
    public delegate void PusherCallback(int code, string message);
    public class MediaPush
    {
        private IntPtr _id = IntPtr.Zero;
        private MediaPush() { }

        internal MediaPush(IntPtr id)
        {
            this._id = id;
        }

        private PInvoke.on_mk_push_event on_mk_push_event;
        //开辟内存空间用以区分是推流结果回调还是异常回调
        private IntPtr _result = Marshal.AllocHGlobal(1);
        //开辟内存空间用以区分是创建回调还是异常回调
        private IntPtr _shutdown = Marshal.AllocHGlobal(1);
        //定义事件等待期，用以Start中等待 推流结果回调
        private AutoResetEvent AutoResetEvent;
        //存储推流结果回调中的code
        private int _pusherCode;
        //存储推流结果回调中的错误消息
        private string _pusherMessage;

        /// <summary>
        /// 实例化一个推流器
        /// </summary>
        /// <param name="mediaInfo"></param>
        internal MediaPush(MediaInfo mediaInfo)
        {
            this._id = PInvoke.ZLMediaKitMethod.mk_pusher_create(mediaInfo.Schema, mediaInfo.VHost, mediaInfo.App, mediaInfo.StreamId);
            if (this._id == IntPtr.Zero) throw new Exception("创建推流器失败");
            on_mk_push_event = new PInvoke.on_mk_push_event(mk_push_event);
            PInvoke.ZLMediaKitMethod.mk_pusher_set_on_result(_id, on_mk_push_event, _result);
            PInvoke.ZLMediaKitMethod.mk_pusher_set_on_shutdown(_id, on_mk_push_event, _shutdown);
        }

        /// <summary>
        /// 开始推流时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 结束推流时间
        /// </summary>
        public DateTime? StopTime { get; private set; }

        /// <summary>
        /// 设置推流器配置选项
        /// </summary>
        /// <param name="key">配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms</param>
        /// <param name="value">配置项值</param>
        public void SetOption(string key,string value)
        {
            PInvoke.ZLMediaKitMethod.mk_pusher_set_option(_id, key, value);
        }

        /// <summary>
        /// 推流地址，支持rtsp[s]/rtmp[s]
        /// </summary>
        /// <param name="url"></param>
        /// <param name="waitTime">等待时长</param>
        /// <returns><inheritdoc cref="PusherCallback" /></returns>
        public (int code,string message) Start(string url,int waitTime = 5000)
        {
            this.AutoResetEvent = new AutoResetEvent(false);
            PInvoke.ZLMediaKitMethod.mk_pusher_publish(this._id, url);
            AutoResetEvent.WaitOne(waitTime);
            return (_pusherCode, _pusherMessage);
        }

        /// <summary>
        /// 停止推流
        /// </summary>
        public void Stop()
        {
            PInvoke.ZLMediaKitMethod.mk_pusher_release(this._id);
            this.StopTime = DateTime.Now;
        }

        /// <summary>
        /// 推流结果或推流中断事件的回调
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="code">错误代码，0为成功</param>
        /// <param name="message">错误提示</param>
        private void mk_push_event(IntPtr userData,int code,string message)
        {
            if (userData == this._result)
            {
                this._pusherMessage = message;
                this._pusherCode = code;
                this.StartTime = DateTime.Now;
            }
            else if (userData == this._shutdown)
            {
                this.StopTime = DateTime.Now;
                if (OnShutdown != null) OnShutdown.Invoke(code, message);
            }
            return;
        }

        /// <summary>
        /// 设置推流被异常中断的回调
        /// </summary>
        public event PusherCallback OnShutdown;

    }
}
