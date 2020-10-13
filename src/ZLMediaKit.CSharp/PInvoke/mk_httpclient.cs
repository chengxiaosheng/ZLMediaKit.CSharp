using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user_data">用户数据指针</param>
    /// <param name="code">错误代码，0代表成功</param>
    /// <param name="err_msg">错误提示</param>
    /// <param name="file_path">文件保存路径</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    public delegate void on_mk_download_complete(IntPtr user_data,
                                                 int code,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string file_path);

    /// <summary>
    /// http请求结果回调
    /// </summary>
    /// <param name="user_data">用户数据指针</param>
    /// <param name="code">错误代码，0代表成功</param>
    /// <param name="err_msg">错误提示</param>
    /// <remarks>
    /// <list type="number">
    /// <item>在code == 0时代表本次http会话是完整的（收到了http回复）</item>
    /// <item>用户应该通过user_data获取到mk_http_requester对象</item>
    /// <item>然后通过mk_http_requester_get_response等函数获取相关回复数据</item>
    /// <item>在回调结束时，应该通过mk_http_requester_release函数销毁该对象</item>
    /// <item>或者调用mk_http_requester_clear函数后再复用该对象</item>
    /// </list>
    /// </remarks>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    public delegate void on_mk_http_requester_complete(IntPtr user_data,
                                                       int code,
                                                       [In()][MarshalAs(UnmanagedType.LPStr)] string err_msg);

    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 创建http[s]下载器
        /// </summary>
        /// <returns>下载器指针 mk_http_downloader</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_downloader_create();

        /// <summary>
        /// 销毁http[s]下载器
        /// </summary>
        /// <param name="mk_http_downloader">下载器指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_downloader_release(IntPtr mk_http_downloader);


        /// <summary>
        /// 开始http[s]下载
        /// </summary>
        /// <param name="mk_http_downloader">下载器指针</param>
        /// <param name="url">http[s]下载url</param>
        /// <param name="file">文件保存路径</param>
        /// <param name="callback">回调函数</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_downloader_start(IntPtr mk_http_downloader,
                                                             [In()][MarshalAs(UnmanagedType.LPStr)] string url,
                                                             [In()][MarshalAs(UnmanagedType.LPStr)] string file,
                                                             on_mk_download_complete callback,
                                                             IntPtr user_data);


        /// <summary>
        /// 创建HttpRequester
        /// </summary>
        /// <returns>mk_http_requester</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_requester_create();


        /// <summary>
        /// 在复用mk_http_requester对象时才需要用到此方法
        /// </summary>
        /// <param name="mk_http_requester"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_clear(IntPtr mk_http_requester);

        /// <summary>
        /// 销毁HttpRequester
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <remarks>如果调用了<see cref="mk_http_requester_start"/>函数且正在等待http回复; 
        /// 也可以调用<see cref="mk_http_requester_release"/>方法取消本次http请求</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_release(IntPtr mk_http_requester);

        /// <summary>
        /// 设置HTTP方法，譬如GET/POST
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="method"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_set_method(IntPtr mk_http_requester, [In()][MarshalAs(UnmanagedType.LPStr)] string method);

        /// <summary>
        /// 批量设置设置HTTP头
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="header">譬如 {"Content-Type","text/html",NULL} 必须以NULL结尾</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_set_header(IntPtr mk_http_requester, ref IntPtr header);


        /// <summary>
        /// 添加HTTP头
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="key">譬如Content-Type</param>
        /// <param name="value">譬如 text/html</param>
        /// <param name="force">如果已经存在该key，是否强制替换</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_add_header(IntPtr mk_http_requester, [In()][MarshalAs(UnmanagedType.LPStr)] string key, [In()][MarshalAs(UnmanagedType.LPStr)] string value, int force);

        /// <summary>
        /// 设置消息体，
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="mk_http_body">mk_http_body对象，通过<see cref="mk_http_body_from_string"/>等函数生成，使用完毕后请调用mk_http_body_release释放之</param>

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_set_body(IntPtr mk_http_requester, IntPtr mk_http_body);

        /// <summary>
        /// 在收到HTTP回复后可调用该方法获取状态码
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <returns>譬如 200 OK</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_requester_get_response_status(IntPtr mk_http_requester);


        /// <summary>
        /// 在收到HTTP回复后可调用该方法获取响应HTTP头
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="key">HTTP头键名</param>
        /// <returns>HTTP头键值</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_requester_get_response_header(IntPtr mk_http_requester, [In()][MarshalAs(UnmanagedType.LPStr)] string key);


        /// <summary>
        /// 在收到HTTP回复后可调用该方法获取响应HTTP body
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="length">返回body长度,可以为null</param>
        /// <returns>body指针</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_requester_get_response_body(IntPtr mk_http_requester, ref int length);

        /// <summary>
        /// 在收到HTTP回复后可调用该方法获取响应
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <returns>mk_parser</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_http_requester_get_response(IntPtr mk_http_requester);

        /// <summary>
        /// 设置回调函数
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="callback">回调函数，不能为空</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_set_cb(IntPtr mk_http_requester, on_mk_http_requester_complete callback, IntPtr user_data);

        /// <summary>
        /// 开始url请求
        /// </summary>
        /// <param name="mk_http_requester"></param>
        /// <param name="url">请求url，支持http/https</param>
        /// <param name="timeout_second">最大超时时间</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_http_requester_start(IntPtr mk_http_requester, [In()][MarshalAs(UnmanagedType.LPStr)] string url, float timeout_second);
    }
}
