using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZLMediaKit.CSharp.PInvoke;

namespace ZLMediaKit.CSharp.HostedService
{
    public class ZLMediaKitInitService : IHostedService
    {
        private readonly ILogger<ZLMediaKitInitService> _logger;
        private readonly IOptions<ZLMediaKitSettings> _settings;
        private mk_config _Config;

        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public ZLMediaKitInitService(ILogger<ZLMediaKitInitService> logger, IOptions<ZLMediaKitSettings> settings)
        {
            this._logger = logger;
            this._settings = settings;
            _Config = new mk_config
            {
                ini_is_path = 1,
                ini = _settings.Value.IniPath,
                log_file_days = _settings.Value.LogSaveDays,
                log_file_path = _settings.Value.LogPath,
                log_level = (int)_settings.Value.LogLevel,
                ssl = _settings.Value.SslPath,
                ssl_is_path = 1,
                ssl_pwd = _settings.Value.SslPassword,
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token, cancellationToken);
            return StartZLMediaKit();
        }


        public Task StartZLMediaKit()
        {
            return Task.Run(() =>
            {
                PInvoke.ZLMediaKitMethod.mk_env_init(ref _Config);
                if(_settings.Value.HttpPort > 0)
                {
                    PInvoke.ZLMediaKitMethod.mk_http_server_start(_settings.Value.HttpPort, 0);
                }
                if(_settings.Value.HttpsPort > 0) PInvoke.ZLMediaKitMethod.mk_http_server_start(_settings.Value.HttpsPort, 1);
                if(_settings.Value.RtspPort > 0) PInvoke.ZLMediaKitMethod.mk_rtsp_server_start(_settings.Value.RtspPort, 0);
                if(_settings.Value.RtspsPort > 0) PInvoke.ZLMediaKitMethod.mk_rtsp_server_start(_settings.Value.RtspsPort, 0);
                if(_settings.Value.RtmpPort > 0) PInvoke.ZLMediaKitMethod.mk_rtmp_server_start(_settings.Value.RtmpPort, 0);
                if(_settings.Value.RtmpsPort > 0) PInvoke.ZLMediaKitMethod.mk_rtmp_server_start(_settings.Value.RtmpsPort, 0);
                if(_settings.Value.RtpPort > 0) PInvoke.ZLMediaKitMethod.mk_rtp_server_start(_settings.Value.RtpPort);
                if(_settings.Value.ShellPort > 0) PInvoke.ZLMediaKitMethod.mk_shell_server_start(_settings.Value.ShellPort);

            },this.CancellationTokenSource.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.CancellationTokenSource.Cancel(false);
            return Task.Run(() =>
            {
                PInvoke.ZLMediaKitMethod.mk_stop_all_server();

            }, cancellationToken);
        }
    }
}
