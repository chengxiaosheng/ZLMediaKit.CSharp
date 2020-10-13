using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void on_mk_media_source_find_cb(IntPtr user_data, IntPtr ctx);

    internal partial class ZLMediaKitMethod
    {

        /// <summary>
        /// 开始时间
        /// GMT 标准时间，单位秒
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern uint mk_mp4_info_get_start_time(IntPtr mk_mp4_info);


        /// <summary>
        /// 录像长度，单位秒
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern float mk_mp4_info_get_time_len(IntPtr mk_mp4_info);


        /// <summary>
        /// 文件大小，单位 BYTE
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern uint mk_mp4_info_get_file_size(IntPtr mk_mp4_info);


        /// <summary>
        /// 文件路径
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_file_path(IntPtr mk_mp4_info);


        /// <summary>
        /// 文件名称
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_file_name(IntPtr mk_mp4_info);


        /// <summary>
        /// 文件夹路径
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns>strFolder</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_folder(IntPtr mk_mp4_info);

        /// <summary>
        /// 播放路径 
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns>strUrl</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_url(IntPtr mk_mp4_info);


        /// <summary>
        /// 虚拟主机
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_vhost(IntPtr mk_mp4_info);

        /// <summary>
        /// 应用名称
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_app(IntPtr mk_mp4_info);

        /// <summary>
        /// 流Id
        /// </summary>
        /// <param name="mk_mp4_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_mp4_info_get_stream(IntPtr mk_mp4_info);


        /// <summary>
        /// 获取命令字，譬如GET/POST
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_method(IntPtr mk_parser);


        /// <summary>
        /// 获取HTTP的访问url(不包括?后面的参数)
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_url(IntPtr mk_parser);

        /// <summary>
        /// FullUrl(),包括?后面的参数
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_full_url(IntPtr mk_parser);

        /// <summary>
        /// Params(),?后面的参数字符串
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_url_params(IntPtr mk_parser);


        /// <summary>
        /// getUrlArgs()["key"],获取?后面的参数中的特定参数
        /// </summary>
        /// <param name="ctmk_parserx"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_url_param(IntPtr ctmk_parserx,
                                                              [In()][MarshalAs(UnmanagedType.LPStr)] string key);

        /// <summary>
        /// ，获取协议相关信息，譬如 HTTP/1.1
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_tail(IntPtr mk_parser);


        /// <summary>
        /// getValues()["key"],获取HTTP头中特定字段
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_header(IntPtr mk_parser,
                                                           [In()][MarshalAs(UnmanagedType.LPStr)] string key);

        /// <summary>
        /// 获取HTTP头中所有字段
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_parser_get_headers(IntPtr mk_parser);


        /// <summary>
        /// 获取HTTP body
        /// </summary>
        /// <param name="mk_parser"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal unsafe static extern char* mk_parser_get_content(IntPtr mk_parser, ref int length);


        /// <summary>
        /// _param_strs
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_params(IntPtr mk_media_info);


        /// <summary>
        /// _schema
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_schema(IntPtr mk_media_info);

        /// <summary>
        /// _vhost
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_vhost(IntPtr mk_media_info);

        /// <summary>
        /// _app
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_app(IntPtr mk_media_info);


        /// <summary>
        /// _streamid
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_stream(IntPtr mk_media_info);


        /// <summary>
        /// _host
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_info_get_host(IntPtr mk_media_info);

        /// <summary>
        /// _port
        /// </summary>
        /// <param name="mk_media_info"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_media_info_get_port(IntPtr mk_media_info);


        /// <summary>
        /// getSchema
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_source_get_schema(IntPtr mk_media_source);

        /// <summary>
        /// getVhost
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_source_get_vhost(IntPtr mk_media_source);

        /// <summary>
        /// getApp
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_source_get_app(IntPtr mk_media_source);

        /// <summary>
        /// getStreamId
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_media_source_get_stream(IntPtr mk_media_source);


        /// <summary>
        /// readerCount
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_media_source_get_reader_count(IntPtr mk_media_source);

        /// <summary>
        /// totalReaderCount
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_media_source_get_total_reader_count(IntPtr mk_media_source);

        /// <summary>
        /// 直播源在ZLMediaKit中被称作为MediaSource，
        /// </summary>
        /// <param name="mk_media_source">对象</param>
        /// <param name="force">是否强制关闭，如果强制关闭，在有人观看的情况下也会关闭</param>
        /// <returns>0代表失败，1代表成功</returns>
        /// <remarks>
        /// <list type="number">
        /// <item>目前支持3种，分别是RtmpMediaSource、RtspMediaSource、HlsMediaSource</item>
        /// <item>源的产生有被动和主动方式:</item>
        /// <item>被动方式分别是rtsp/rtmp/rtp推流、mp4点播</item>
        /// <item>主动方式包括mk_media_create创建的对象(DevChannel)、mk_proxy_player_create创建的对象(PlayerProxy)</item>
        /// <item>被动方式你不用做任何处理，ZLMediaKit已经默认适配了MediaSource::close()事件，都会关闭直播流</item>
        /// <item>主动方式你要设置这个事件的回调，你要自己选择删除对象</item>
        /// <item>通过<seealso cref="mk_proxyplayer.mk_proxy_player_set_on_close"/>、<seealso cref="mk_media_set_on_close"/>函数可以设置回调</item>
        /// <item>请在回调中删除对象来完成媒体的关闭，否则又为什么要调用mk_media_source_close函数？</item>
        /// </list></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_media_source_close(IntPtr mk_media_source, int force);

        /// <summary>
        /// seekTo
        /// </summary>
        /// <param name="mk_media_source"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern int mk_media_source_seek_to(IntPtr mk_media_source, uint stamp);

        /// <summary>
        /// 查找MediaSource
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <param name="user_data"></param>
        /// <param name="callback"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_source_find([In()][MarshalAs(UnmanagedType.LPStr)] string schema,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string stream,
                                                         IntPtr user_data,
                                                         on_mk_media_source_find_cb callback);


        /// <summary>
        /// 遍历所有MediaSource
        /// </summary>
        /// <param name="user_data"></param>
        /// <param name="callback"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_media_source_for_each(IntPtr user_data, on_mk_media_source_find_cb callback);


        /// <summary>
        /// 生成HttpStringBody
        /// </summary>
        /// <param name="str">字符串指针</param>
        /// <param name="len">字符串长度，为0则用strlen获取</param>
        /// <returns>mk_http_body</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_body_from_string([In()][MarshalAs(UnmanagedType.LPStr)] string str,
                                                               int len);


        /// <summary>
        /// 生成HttpFileBody
        /// </summary>
        /// <param name="file_path">文件完整路径</param>
        /// <returns>mk_http_body</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_body_from_file([In()][MarshalAs(UnmanagedType.LPStr)] string file_path);


        /// <summary>
        /// 生成HttpMultiFormBody
        /// </summary>
        /// <param name="key_val">参数key-value</param>
        /// <param name="file_path">文件完整路径</param>
        /// <returns>mk_http_body</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_body_from_multi_form(ref IntPtr key_val,
                                                                   [In()][MarshalAs(UnmanagedType.LPStr)] string file_path);


        /// <summary>
        /// 销毁HttpBody
        /// </summary>
        /// <param name="mk_http_body"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_body_release(IntPtr mk_http_body);


        /// <summary>
        /// HttpSession::HttpResponseInvoker(const string &codeOut, const StrCaseMap &headerOut, const HttpBody::Ptr &body);
        /// </summary>
        /// <param name="mk_http_response_invoker"></param>
        /// <param name="response_code">譬如200 OK</param>
        /// <param name="response_header">返回的http头，譬如 {"Content-Type","text/html",NULL} 必须以NULL结尾</param>
        /// <param name="mk_http_body">body对象</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_response_invoker_do(IntPtr mk_http_response_invoker,
                                                                [In()][MarshalAs(UnmanagedType.LPStr)] string response_code,
                                                                ref IntPtr response_header,
                                                                IntPtr mk_http_body);


        /// <summary>
        /// HttpSession::HttpResponseInvoker(const string &codeOut, const StrCaseMap &headerOut, const string &body);
        /// </summary>
        /// <param name="mk_http_response_invoker"></param>
        /// <param name="response_code">譬如200 OK</param>
        /// <param name="response_header">返回的http头，譬如 {"Content-Type","text/html",NULL} 必须以NULL结尾</param>
        /// <param name="response_content">返回的content部分，譬如一个网页内容</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_response_invoker_do_string(IntPtr mk_http_response_invoker,
                                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string response_code,
                                                                       ref IntPtr response_header,
                                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string response_content);


        /// <summary>
        /// HttpSession::HttpResponseInvoker(const StrCaseMap &requestHeader,const StrCaseMap &responseHeader,const string &filePath);
        /// </summary>
        /// <param name="mk_http_response_invoker"></param>
        /// <param name="mk_parser">请求事件中的mk_parser对象，用于提取其中http头中的Range字段，通过该字段先fseek然后再发送文件部分片段</param>
        /// <param name="response_header">返回的http头，譬如 {"Content-Type","text/html",NULL} 必须以NULL结尾</param>
        /// <param name="response_file_path">返回的content部分，譬如/path/to/html/file</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_response_invoker_do_file(IntPtr mk_http_response_invoker,
                                                                     IntPtr mk_parser,
                                                                     ref IntPtr response_header,
                                                                     [In()][MarshalAs(UnmanagedType.LPStr)] string response_file_path);


        /// <summary>
        /// 克隆mk_http_response_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_http_response_invoker_do"/>
        /// </summary>
        /// <param name="mk_http_response_invoker"></param>
        /// <returns>mk_http_response_invoker</returns>
        /// <remarks>如果是同步执行mk_http_response_invoker_do，那么没必要克隆对象</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_response_invoker_clone(IntPtr mk_http_response_invoker);

        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_http_response_invoker"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_response_invoker_clone_release(IntPtr mk_http_response_invoker);


        /// <summary>
        /// HttpSession::HttpAccessPathInvoker(const string &errMsg,const string &accessPath, int cookieLifeSecond);
        /// </summary>
        /// <param name="mk_http_access_path_invoker"></param>
        /// <param name="err_msg">如果为空，则代表鉴权通过，否则为错误提示,可以为null</param>
        /// <param name="access_path">运行或禁止访问的根目录,可以为null</param>
        /// <param name="cookie_life_second">鉴权cookie有效期</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_access_path_invoker_do(IntPtr mk_http_access_path_invoker,
                                                                   [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg,
                                                                   [In()][MarshalAs(UnmanagedType.LPStr)] string access_path,
                                                                   int cookie_life_second);


        /// <summary>
        /// 克隆mk_http_access_path_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_http_access_path_invoker_do"/>
        /// </summary>
        /// <param name="mk_http_access_path_invoker"></param>
        /// <returns>mk_http_access_path_invoker</returns>
        /// <remarks>如果是同步执行<seealso cref="mk_http_access_path_invoker_do"/>，那么没必要克隆对象</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_access_path_invoker_clone(IntPtr mk_http_access_path_invoker);


        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_http_access_path_invokerctx"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_access_path_invoker_clone_release(IntPtr mk_http_access_path_invokerctx);


        /// <summary>
        /// 执行RtspSession::onGetRealm
        /// </summary>
        /// <param name="mk_rtsp_get_realm_invoker"></param>
        /// <param name="realm">该rtsp流是否需要开启rtsp专属鉴权，至null或空字符串则不鉴权</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtsp_get_realm_invoker_do(IntPtr mk_rtsp_get_realm_invoker,
                                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string realm);

        /// <summary>
        /// 克隆mk_rtsp_get_realm_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_rtsp_get_realm_invoker_do"/>
        /// </summary>
        /// <param name="mk_rtsp_get_realm_invoker"></param>
        /// <returns>mk_rtsp_get_realm_invoker</returns>
        /// <remarks>如果是同步执行<seealso cref="mk_rtsp_get_realm_invoker_do"/>，那么没必要克隆对象</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_rtsp_get_realm_invoker_clone(IntPtr mk_rtsp_get_realm_invoker);


        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_rtsp_get_realm_invoker"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtsp_get_realm_invoker_clone_release(IntPtr mk_rtsp_get_realm_invoker);

        /// <summary>
        /// 执行RtspSession::onAuth
        /// </summary>
        /// <param name="mk_rtsp_auth_invoker"></param>
        /// <param name="encrypted">为true是则表明是md5加密的密码，否则是明文密码, 在请求明文密码时如果提供md5密码者则会导致认证失败</param>
        /// <param name="pwd_or_md5">明文密码或者md5加密的密码</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtsp_auth_invoker_do(IntPtr mk_rtsp_auth_invoker,
                                                            int encrypted,
                                                            [In()][MarshalAs(UnmanagedType.LPStr)] string pwd_or_md5);


        /// <summary>
        /// /克隆mk_rtsp_auth_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_rtsp_auth_invoker_do"/>
        /// </summary>
        /// <param name="mk_rtsp_auth_invoker"></param>
        /// <returns>mk_rtsp_auth_invoker</returns>
        /// <remarks>克隆mk_rtsp_auth_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_rtsp_auth_invoker_do"/></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_rtsp_auth_invoker_clone(IntPtr mk_rtsp_auth_invoker);

        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_rtsp_auth_invoker"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_rtsp_auth_invoker_clone_release(IntPtr mk_rtsp_auth_invoker);


        /// <summary>
        /// 执行Broadcast::PublishAuthInvoker
        /// </summary>
        /// <param name="mk_publish_auth_invoker"></param>
        /// <param name="err_msg">为空或null则代表鉴权成功</param>
        /// <param name="enable_hls">是否允许转换hls</param>
        /// <param name="enable_mp4">是否运行MP4录制</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_publish_auth_invoker_do(IntPtr mk_publish_auth_invoker,
                                                               [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg,
                                                               int enable_hls,
                                                               int enable_mp4);


        /// <summary>
        /// 克隆mk_publish_auth_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_publish_auth_invoker_do"/>
        /// </summary>
        /// <param name="mk_publish_auth_invoker"></param>
        /// <returns>mk_publish_auth_invoker</returns>
        /// <remarks>克隆mk_publish_auth_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_publish_auth_invoker_do"/></remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_publish_auth_invoker_clone(IntPtr mk_publish_auth_invoker);


        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_publish_auth_invoker"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_publish_auth_invoker_clone_release(IntPtr mk_publish_auth_invoker);

        /// <summary>
        /// 执行Broadcast::AuthInvoker
        /// </summary>
        /// <param name="mk_auth_invoker"></param>
        /// <param name="err_msg">为空或null则代表鉴权成功</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_auth_invoker_do(IntPtr mk_auth_invoker,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg);

        /// <summary>
        /// 克隆mk_auth_invoker对象，通过克隆对象为堆对象，可以实现跨线程异步执行<seealso cref="mk_auth_invoker_do"/>
        /// </summary>
        /// <param name="mk_auth_invoker"></param>
        /// <returns>mk_auth_invoker</returns>
        /// <remarks>如果是同步执行<seealso cref="mk_auth_invoker_do"/>，那么没必要克隆对象</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_auth_invoker_clone(IntPtr mk_auth_invoker);

        /// <summary>
        /// 销毁堆上的克隆对象
        /// </summary>
        /// <param name="mk_auth_invoker"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_auth_invoker_clone_release(IntPtr mk_auth_invoker);
    }
}
