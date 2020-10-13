using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace ZLMediaKit.CSharp
{
    public class ZLMediaKitSettings : IOptions<ZLMediaKitSettings>
    {

        public ZLMediaKitSettings Value => this;

        public string IniPath { get; set; }


        private LogLevel _logLevel = LogLevel.Information;
        /// <summary>
        /// 日志等级
        /// </summary>
        /// <remarks>默认：Information </remarks>
        public LogLevel LogLevel { get => this._logLevel; set =>  _logLevel = (int)value > 4 ?  LogLevel.Error : value; }
        /// <summary>
        /// 日志目录
        /// </summary>
        /// <remarks>Default : ./logs/zlmediakit/</remarks>
        public string LogPath { get; set; } = Path.Combine("logs", "zlmediakit");
        /// <summary>
        /// 日志保存周期
        /// </summary>
        /// <remarks>Default : 7 天 </remarks>
        public int LogSaveDays { get; set; } = 7;

        /// <summary>
        /// SSL证书路径
        /// </summary>
        /// <remarks>Default : Null</remarks>
        public string SslPath { get; set; }
        /// <summary>
        /// SSL证书密码
        /// </summary>
        /// <remarks>Default : Null</remarks>
        public string SslPassword { get; set; }

        /// <summary>
        /// 预定于http zlmediakit时事件回调中需要提取的http header
        /// </summary>
        public string[] HttpHeaderNames { get; set; } = { "Host", "Origin", "Referer", "User-Agent", "Accept", "Accept-Encoding", "Accept-Language" };


        /// <summary>
        /// 是否启用Web Api
        /// </summary>
        /// <remarks>默认： False</remarks>
        public bool EableHttpApi { get; set; } = false;

        /// <summary>
        /// Http端口, 0标识禁用
        /// </summary>
        /// <remarks>默认：80</remarks>
        public ushort HttpPort { get; set; } = 80;

        /// <summary>
        /// Https端口，0表示禁用
        /// </summary>
        /// <remarks>默认：443</remarks>
        public ushort HttpsPort { get; set; } = 443;

        /// <summary>
        /// Rtsp端口，0标识禁用
        /// </summary>
        /// <remarks>默认：554</remarks>
        public ushort RtspPort { get; set; } = 554;

        /// <summary>
        /// Rtsps端口，0标识禁用
        /// </summary>
        /// <remarks>默认：322</remarks>
        public ushort RtspsPort { get; set; } = 322;

        /// <summary>
        /// Rtmp端口 ，0表示禁用
        /// </summary>
        /// <remarks>默认：1935</remarks>
        public ushort RtmpPort { get; set; } = 1935;

        /// <summary>
        /// Rtmps 端口，0表示禁用
        /// </summary>
        /// <remarks>默认：19350</remarks>
        public ushort RtmpsPort { get; set; } = 19350;
        /// <summary>
        /// Rtp端口，0表示禁用
        /// </summary>
        /// <remarks>默认：10000</remarks>
        public ushort RtpPort { get; set; } = 10000;

        /// <summary>
        /// Shell管理端口，0表示禁用
        /// </summary>
        /// <remarks>默认：9000</remarks>
        public ushort ShellPort { get; set; } = 9000;

    }
}
