using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ZLMediaKit.CSharp.Helper;
using ZLMediaKit.CSharp.PInvoke;

namespace ZLMediaKit.CSharp.ZLMediaKit
{
    /// <summary>
    /// ZLMediaKit C API mk_common.h
    /// </summary>
    /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
    public class Common
    {
        private readonly ILogger<Common> _logger;
        private readonly IOptions<ZLMediaKitSettings> _settings;

        public Common(ILogger<Common> logger, IOptions<ZLMediaKitSettings> settings)
        {
            this._logger = logger;
            this._settings = settings;
        }

        /// <summary>
        /// 初始化环境，调用该库前需要先调用此函数
        /// </summary>
        /// <remarks>根据配置文件初始化 <inheritdoc cref="ZLMediaKitComment"/></remarks>
        internal Task Init()
        {
            return Task.Run(() =>
            {
                var zlconfig = new mk_config
                {
                    ini = _settings.Value.IniPath,
                    ini_is_path = 1,
                    log_file_days = _settings.Value.LogSaveDays,
                    log_file_path = _settings.Value.LogPath,
                    log_level = (int)_settings.Value.LogLevel,
                    ssl = _settings.Value.SslPath,
                    ssl_is_path = 1,
                    ssl_pwd = _settings.Value.SslPassword,
                };
                PInvoke.ZLMediaKitMethod.mk_env_init(ref zlconfig);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 基础类型参数版本的mk_env_init，为了方便其他语言调用
        /// </summary>
        /// <param name="thread_num">线程数</param>
        /// <param name="log_level">日志级别,支持0~4</param>
        /// <param name="log_file_path">文件日志保存路径,路径可以不存在(内部可以创建文件夹)，设置为NULL关闭日志输出至文件</param>
        /// <param name="log_file_days">文件日志保存天数,设置为0关闭日志文件</param>
        /// <param name="ini_is_path">配置文件是内容还是路径</param>
        /// <param name="ini">配置文件内容或路径，可以为NULL,如果该文件不存在，那么将导出默认配置至该文件</param>
        /// <param name="ssl_is_path">ssl证书是内容还是路径</param>
        /// <param name="ssl">ssl证书内容或路径，可以为NULL</param>
        /// <param name="ssl_pwd">证书密码，可以为NULL</param>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task Init(int thread_num, int log_level, string log_file_path, int log_file_days, int ini_is_path, string ini, int ssl_is_path, string ssl, string ssl_pwd)
        {
            return Task.Run(() => PInvoke.ZLMediaKitMethod.mk_env_init1(thread_num, log_level, log_file_path, log_file_days, ini_is_path, ini, ssl_is_path, ssl, ssl_pwd))
                .ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <param name="key">配置项名</param>
        /// <param name="val">配置项值</param>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task SetOption(string key,string value)
        {
            return Task.Run(() => PInvoke.ZLMediaKitMethod.mk_set_option(key, value));
        }

        public Task<String> GetOption(string key)
        {
            return Task.Run(() =>
            {
                var intptr = PInvoke.ZLMediaKitMethod.mk_get_option(key);
                string value = Marshal.PtrToStringAnsi(intptr);
                return value;
            }).ContinueWith(GeneralTask.WriteFaultedLog,TaskContinuationOptions.OnlyOnFaulted);
        }

       
        internal Task<ushort> HttpsServerStart() => HttpServerStart(_settings.Value.HttpsPort, true);
        internal Task<ushort> HttpServerStart() => HttpServerStart(_settings.Value.HttpPort);

        /// <summary>
        /// 开启Https服务
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> HttpsServerStart(ushort port) => HttpServerStart(port, true);
        /// <summary>
        /// 开启http 服务
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        /// <returns></returns>
        public Task<ushort> HttpServerStart(ushort port = 0,bool ssl = false)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_http_server_start(port, ssl ? 1 : 0);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }


        internal Task<ushort> RtspsServerStart() => RtspServerStart(_settings.Value.HttpsPort, true);
        internal Task<ushort> RtspServerStart() => RtspServerStart(_settings.Value.HttpPort);

        /// <summary>
        /// 开启Https服务
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> RtspsServerStart(ushort port) => RtspServerStart(port, true);

        /// <summary>
        /// 开启Rtsp服务
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="ssl">是否为rtsps</param>
        /// <returns></returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> RtspServerStart(ushort port = 0,bool ssl = false)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_rtsp_server_start(port, ssl ? 1 : 0);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }



        internal Task<ushort> RtmpsServerStart() => RtmpServerStart(_settings.Value.HttpsPort, true);
        internal Task<ushort> RtmpServerStart() => RtmpServerStart(_settings.Value.HttpPort);

        /// <summary>
        /// 开启RtmpServerStarts服务
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> RtmpsServerStart(ushort port) => RtmpServerStart(port, true);

        /// <summary>
        /// 开启Rtmp服务
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="ssl">是否为rtsps</param>
        /// <returns></returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> RtmpServerStart(ushort port = 0, bool ssl = false)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_rtmp_server_start(port, ssl ? 1 : 0);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 创建rtp服务器
        /// </summary>
        /// <param name="port">rtp监听端口(包括udp/tcp)</param>
        /// <returns>0:失败,非0:端口号</returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> RtpServerStart(ushort port)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_rtp_server_start(port);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 创建shell服务器
        /// </summary>
        /// <param name="port">shell监听端口</param>
        /// <returns>0:失败,非0:端口号</returns>
        /// <remarks><inheritdoc cref="ZLMediaKitComment"/></remarks>
        public Task<ushort> ShellServerStart(ushort port)
        {
            return Task.Run(() =>
            {
                return PInvoke.ZLMediaKitMethod.mk_shell_server_start(port);
            }).ContinueWith(GeneralTask.WriteFaultedLog, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
