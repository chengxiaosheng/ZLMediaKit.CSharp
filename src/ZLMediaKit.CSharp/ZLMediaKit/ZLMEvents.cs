using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZLMediaKit.CSharp.dtos;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.ZLMediaKit
{
    public static class ZLMEvents
    {
        private static PInvoke.mk_events mkEvnets;
        static ZLMEvents()
        {
            mkEvnets = new PInvoke.mk_events
            {
                OnFlowReport = new PInvoke.on_mk_flow_report(mk_flow_report) ,
                OnHttpAccess = new PInvoke.on_mk_http_access(mk_http_access),
                OnHttpBeforeAccess = new PInvoke.on_mk_http_before_access(mk_http_before_access),
                OnHttpRequest = new PInvoke.on_mk_http_request(mk_http_request),
                OnMediaChanged = new PInvoke.on_mk_media_changed(mk_media_changed),
                OnMediaNoReader = new PInvoke.on_mk_media_no_reader(mk_media_no_reader),
                OnMediaNotFound = new PInvoke.on_mk_media_not_found(mk_media_not_found),
                OnMediaPlay = new PInvoke.on_mk_media_play(mk_media_play),
                OnMediaPublish = new PInvoke.on_mk_media_publish(mk_media_publish),
                OnRecordMp4 = new PInvoke.on_mk_record_mp4(mk_record_mp4),
                OnRtspAuth = new PInvoke.on_mk_rtsp_auth(mk_rtsp_auth),
                OnRtspGetRealm = new PInvoke.on_mk_rtsp_get_realm(mk_rtsp_get_realm),
                OnShellLogin = new PInvoke.on_mk_shell_login(mk_shell_login)
            };
        }

        internal static void Run()
        {
            //监听ZLM的所有事件
            PInvoke.ZLMediaKitMethod.mk_events_listen(ref mkEvnets);
        }

        private static void mk_media_changed(int regist,IntPtr mk_media_source)
        {
            OnMediaChanged?.Invoke(regist, new MediaSource(mk_media_source));
        }
        /// <summary>
        /// 媒体改变事件，
        /// </summary>
        internal static event Action<int, MediaSource> OnMediaChanged;

        private static void mk_media_publish(IntPtr mk_media_info,IntPtr mk_publish_auth_invoker ,IntPtr mk_sock_info)
        {
            var response = new dtos.PublishInvokerDto { Message = "不允许推流", EnableHls = false, EnableMp4 = false };
            if (OnPublish  != null)
            {
                var mediaWapper = new MediaSourceWapper();
                mediaWapper.MediaInfo = new MediaInfo(mk_media_info);
                mediaWapper.SockInfo = new SockInfo(mk_sock_info);
                var result = OnPublish.Invoke(mediaWapper) ?? response;
                PInvoke.ZLMediaKitMethod.mk_publish_auth_invoker_do(mk_publish_auth_invoker, result.Message, result.EnableHls ? 1 : 0, result.EnableMp4 ? 1 : 0);
                return;
            }
            PInvoke.ZLMediaKitMethod.mk_publish_auth_invoker_do(mk_publish_auth_invoker, response.Message, response.EnableHls ? 1 : 0, response.EnableMp4 ? 1 : 0);
        }

        /// <summary>
        /// 收到rtsp/rtmp/rtp推流事件广播，通过该事件控制推流鉴权
        /// </summary>
        public static event Func<MediaSourceWapper, dtos.PublishInvokerDto> OnPublish;

        private static void mk_media_play(IntPtr mk_media_info,IntPtr auth_invoker ,IntPtr mk_sock_info)
        {
            if(OnMediaPlay == null)
            {
                PInvoke.ZLMediaKitMethod.mk_auth_invoker_do(auth_invoker, null);
                return;
            }
            var mediaInfo = new MediaInfo(mk_media_info);
            var list = new List<Task<MediaSourceWapper>>();
            foreach(Func<MediaInfo, MediaSourceWapper> item in OnInternalMediaPlay.GetInvocationList())
            {
                list.Add(Task<MediaSourceWapper>.Run(() => item(mediaInfo)));
            }
            Task.WaitAll(list.ToArray());
            var mediasourceWapper = list.FirstOrDefault(w => w.Result != null)?.Result;
            var sockinfo = new SockInfo(mk_sock_info);
            bool notMedia = mediasourceWapper == null;
            mediasourceWapper = new MediaSourceWapper { MediaInfo = new MediaInfo(mk_media_info)};
            var result = OnMediaPlay.Invoke(mediasourceWapper, sockinfo);
            if(string.IsNullOrEmpty(result))
            {
                mediasourceWapper._watchClient.Add(new WatchClient(new SockInfo(mk_sock_info)));
            }
            PInvoke.ZLMediaKitMethod.mk_auth_invoker_do(auth_invoker, result);
        }
        //内部消化，获取播放
        internal static event Func<MediaInfo,MediaSourceWapper> OnInternalMediaPlay;
        /// <summary>
        /// 播放rtsp/rtmp/http-flv/hls事件广播，通过该事件控制播放鉴权
        /// </summary>
        /// <remarks>如果运行播放则返回null,否则返回不允许的原因</remarks>
        public static event Func<MediaSourceWapper, SockInfo, string> OnMediaPlay;


        private static void mk_media_not_found(IntPtr mk_media_info,IntPtr mk_sock_info)
        {
            if(OnMediaNotFound != null)
            {
                var mediaInfo = new MediaInfo(mk_media_info);
                var list = new List<Task<MediaSourceWapper>>();
                foreach (Func<MediaInfo, MediaSourceWapper> item in OnInternalMediaPlay.GetInvocationList())
                {
                    list.Add(Task<MediaSourceWapper>.Run(() => item(mediaInfo)));
                }
                Task.WaitAll(list.ToArray());
                var mediasourceWapper = list.FirstOrDefault(w => w.Result != null).Result ?? new MediaSourceWapper { MediaInfo = new MediaInfo(mk_media_info)};
                OnMediaNotFound.Invoke(mediasourceWapper);
            }
        }

        /// <summary>
        /// 未找到流后会广播该事件，请在监听该事件后去拉流或其他方式产生流，这样就能按需拉流了
        /// </summary>
        public static event Action<MediaSourceWapper> OnMediaNotFound;

        private static void mk_media_no_reader(IntPtr mk_media_source)
        {
            OnMediaNoReader.Invoke(new MediaSource(mk_media_source));
        }

        internal static event Action<MediaSource> OnMediaNoReader;

        private static void mk_http_request(IntPtr mk_parser,IntPtr mk_http_response_invoker,ref int consumed,IntPtr mk_sock_info)
        {
            if (OnHttpRequest == null) return;
            consumed = 1;
            var httpBody = OnHttpRequest.Invoke(new HttpRequest(mk_parser,mk_sock_info));
            if(httpBody == null)
            {
                consumed = 0;
                return;
            }
            httpBody.GetHttpHeader();
            PInvoke.ZLMediaKitMethod.mk_http_response_invoker_do(mk_http_response_invoker, $"{(int)httpBody.HttpStatusCode} {httpBody.HttpStatusCode}", ref httpBody.headerIntptr, httpBody.GetHttpBody());
        }
        /// <summary>
        /// 
        /// </summary>
        public static event Func<HttpRequest, HttpBody> OnHttpRequest;

        private static void mk_http_access(IntPtr mk_parser,string path,int is_dir,IntPtr mk_http_access_path_invoker,IntPtr mk_sock_info)
        {
            if (OnHttpAccess == null) return;
            var result = OnHttpAccess.Invoke(new HttpAccess(mk_parser, mk_sock_info, is_dir == 1 ? true : false, path));
            if (result == null) return;
            PInvoke.ZLMediaKitMethod.mk_http_access_path_invoker_do(mk_http_access_path_invoker, result.Message, result.AccessPath, result.CookieLifeSecond);
        }
        /// <summary>
        /// 
        /// </summary>
        public static event Func<HttpAccess,HttpAccessResult> OnHttpAccess;

        private unsafe static void mk_http_before_access(IntPtr mk_parser,ref string path,IntPtr mk_sock_info)
        {
            if (OnHttpAccessBefore == null) return;
            var result = OnHttpAccessBefore.Invoke(new HttpAccessBefore(mk_parser, mk_sock_info, path));
            if (string.IsNullOrEmpty(result)) return;
            path = result;
        }
        /// <summary>
        /// 在http文件服务器中,收到http访问文件或目录前的广播,通过该事件可以控制http url到文件路径的映射;
        /// <para>在该事件中通过自行覆盖path参数，可以做到譬如根据虚拟主机或者app选择不同http根目录的目的</para>
        /// </summary>
        public static event Func<HttpAccessBefore, string> OnHttpAccessBefore;

        private static void mk_rtsp_get_realm(IntPtr mk_media_info, IntPtr mk_rtsp_get_realm_invoker, IntPtr mk_sock_info)
        {
            if(OnRtspGetRealm == null)
            {
                PInvoke.ZLMediaKitMethod.mk_rtsp_get_realm_invoker_do(mk_rtsp_get_realm_invoker, null);
            }
            var mediaInfo = new MediaInfo(mk_media_info);
            var list = new List<Task<string>>();
            foreach (Func<MediaInfo, SockInfo, string> item in OnRtspGetRealm.GetInvocationList())
            {
                list.Add(Task<string>.Run(() => item(new MediaInfo(mk_media_info),new SockInfo(mk_sock_info))));
            }
            Task.WaitAll(list.ToArray());
            var mediasourceWapper = list.FirstOrDefault(w => w.Result != null)?.Result;
            PInvoke.ZLMediaKitMethod.mk_rtsp_get_realm_invoker_do(mk_rtsp_get_realm_invoker, mediasourceWapper);
        }

        internal static event Func<MediaInfo, SockInfo,string> OnRtspGetRealm;

        private static void mk_rtsp_auth(IntPtr mk_media_info, string realm, string user_name, int must_no_encrypt, IntPtr mk_rtsp_auth_invoker, IntPtr mk_sock_info)
        {
            var mediaInfo = new MediaInfo(mk_media_info);
            var list = new List<Task<string>>();
            var rtspAuth = new RtspAuth() { MediaInfo = mediaInfo, Realm = realm, Username = user_name, MustNoEncrypt = must_no_encrypt == 1, SockInfo = new SockInfo(mk_sock_info) };
            foreach (Func<RtspAuth, string> item in OnRtspGetRealm.GetInvocationList())
            {
                list.Add(Task<string>.Run(() => item(rtspAuth)));
            }
            Task.WaitAll(list.ToArray());
            var password = list.FirstOrDefault(w => w.Result != null)?.Result;
            PInvoke.ZLMediaKitMethod.mk_rtsp_auth_invoker_do(mk_rtsp_auth_invoker, rtspAuth.MustNoEncrypt ? 0 : 1, password);
        }

        internal static event Func<RtspAuth, string> OnRtspAuth;


        private static void mk_record_mp4(IntPtr mk_mp4_info)
        {

        }

        private static void mk_shell_login(string user_name,string passwd,IntPtr mk_auth_invoker,IntPtr mk_sock_info)
        {
            var result = string.Empty;
            if (OnShellLogin != null)
            {
                result = OnShellLogin.Invoke(new ShellLoginDto(user_name,passwd,new SockInfo(mk_sock_info)));
            }
            PInvoke.ZLMediaKitMethod.mk_auth_invoker_do(mk_auth_invoker, result);
        }
        /// <summary>
        /// shell登录鉴权 
        /// </summary>
        public static event Func<ShellLoginDto, string> OnShellLogin;

        private static void mk_flow_report(IntPtr mk_media_info,uint total_bytes,uint total_seconds,int is_player,IntPtr mk_sock_info)
        {
           if(OnFlowReport!= null)
            {
                OnFlowReport.Invoke((new MediaInfo(mk_media_info), total_bytes, total_seconds, is_player, new SockInfo(mk_sock_info)));
            }
        }
        internal static event Action<(MediaInfo mk_media_info, uint total_bytes, uint total_seconds, int is_player, SockInfo mk_sock_info)> OnFlowReport;

    }
}
