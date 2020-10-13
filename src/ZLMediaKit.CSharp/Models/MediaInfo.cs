using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.Models
{
    public class MediaInfo
    {
        private IntPtr _id = IntPtr.Zero;
        internal MediaInfo(IntPtr intPtr)
        {
            this._id = intPtr;
        }
        internal MediaInfo() { }

        internal string _vhost
        {
            get
            {
                if (_vhost != null || this._id == IntPtr.Zero) return _vhost;
                _vhost = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_vhost(this._id));
                return _vhost;
            }
            set { _vhost = value; }
        }
        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VHost => _vhost;


        internal string _app
        {
            get
            {
                if (_app != null || this._id == IntPtr.Zero) return _app;
                _app = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_app(this._id));
                return _app;
            }
            set { _app = value; }
        }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string App => _app;

        internal string _streamId
        {
            get
            {
                if (_streamId != null || this._id == IntPtr.Zero) return _streamId;
                _streamId = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_stream(this._id));
                return _streamId;
            }
            set { _streamId = value; }
        }

        /// <summary>
        /// 流Id
        /// </summary>
        public string StreamId => _streamId;


        internal string _schema
        {
            get
            {
                if (_schema != null || this._id == IntPtr.Zero) return _schema;
                _schema = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_schema(this._id));
                return _schema;
            }
            set { _schema = value; }
        }

        /// <summary>
        /// 协议
        /// </summary>
        public string Schema => _schema;


        internal string _host
        {
            get
            {
                if (_host != null || this._id == IntPtr.Zero) return _host;
                _host = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_host(this._id));
                return _host;
            }
            set { _host = value; }
        }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host => _host;


        internal ushort _port
        {
            get
            {
                if (_port != 0 || this._id == IntPtr.Zero) return _port;
                _port = PInvoke.ZLMediaKitMethod.mk_media_info_get_port(this._id);
                return _port;
            }
            set { _port = value; }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port => _port;

        internal string _params
        {
            get
            {
                if (_params != null || this._id == IntPtr.Zero) return _params;
                _params = Marshal.PtrToStringAnsi(PInvoke.ZLMediaKitMethod.mk_media_info_get_params(this._id));
                return _params;
            }
            set { _params = value; }
        }
        /// <summary>
        /// Url 参数
        /// </summary>
        public string Params => _params;


        internal float _duration;
        /// <summary>
        /// 时长(单位秒)
        /// </summary>
        /// <remarks>0 则表示为直播</remarks>
        public float Duration => _duration;

        internal bool _enableHls = false;
        /// <summary>
        /// 是否生成hls
        /// </summary>
        public bool EnableHls => _enableHls;

        internal bool _enableMp4 = false;
        /// <summary>
        /// 是否生成mp4
        /// </summary>
        public bool EnableMp4 => _enableMp4;

    }
}
