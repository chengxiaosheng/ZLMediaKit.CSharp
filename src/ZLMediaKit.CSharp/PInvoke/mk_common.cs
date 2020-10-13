using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    internal struct mk_config
    {
        /// <summary>
        /// 线程数
        /// </summary>
        internal int thread_num;

        /// <summary>
        /// 日志级别,支持0~4
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        internal int log_level;
        //文件日志保存路径,路径可以不存在(内部可以创建文件夹)，设置为NULL关闭日志输出至文件
        internal string log_file_path;
        //文件日志保存天数,设置为0关闭日志文件
        [MarshalAs(UnmanagedType.I4)]
        internal int log_file_days;

        // 配置文件是内容还是路径
        internal int ini_is_path;
        // 配置文件内容或路径，可以为NULL,如果该文件不存在，那么将导出默认配置至该文件
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ini;

        // ssl证书是内容还是路径
        [MarshalAs(UnmanagedType.I4)]
        internal int ssl_is_path;
        // ssl证书内容或路径，可以为NULL
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ssl;
        // 证书密码，可以为NULL
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ssl_pwd;
    }

    internal partial class ZLMediaKitMethod
    {

        /// <summary>
        /// 初始化环境，调用该库前需要先调用此函数
        /// </summary>
        /// <param name="cfg">库运行相关参数</param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_env_init(ref mk_config cfg);

        /// <summary>
        /// 关闭所有服务器，请在main函数退出时调用
        /// </summary>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_stop_all_server();

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
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_env_init1(int thread_num,
                                                 int log_level,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string log_file_path,
                                                 int log_file_days,
                                                 int ini_is_path,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string ini,
                                                 int ssl_is_path,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string ssl,
                                                 [In()][MarshalAs(UnmanagedType.LPStr)] string ssl_pwd);

        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <param name="key">配置项名</param>
        /// <param name="val">配置项值</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_set_option([In()][MarshalAs(UnmanagedType.LPStr)] string key,
                                                  [In()][MarshalAs(UnmanagedType.LPStr)] string val);

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">配置项名</param>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern  IntPtr mk_get_option([In()][MarshalAs(UnmanagedType.LPStr)] string key);

        /// <summary>
        /// 创建http[s]服务器
        /// </summary>
        /// <param name="port">htt监听端口，推荐80，传入0则随机分配</param>
        /// <param name="ssl">是否为ssl类型服务器</param>
        /// <returns>0:失败,非0:端口号</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_http_server_start(ushort port, int ssl);


        /// <summary>
        /// 创建rtsp[s]服务器
        /// </summary>
        /// <param name="port">rtsp监听端口，推荐554，传入0则随机分配</param>
        /// <param name="ssl">是否为ssl类型服务器</param>
        /// <returns> 0:失败,非0:端口号</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_rtsp_server_start(ushort port, int ssl);



        /// <summary>
        /// 创建rtmp[s]服务器
        /// </summary>
        /// <param name="port">rtmp监听端口，推荐1935，传入0则随机分配</param>
        /// <param name="ssl">是否为ssl类型服务器</param>
        /// <returns>0:失败,非0:端口号</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_rtmp_server_start(ushort port, int ssl);



        /// <summary>
        /// 创建rtp服务器
        /// </summary>
        /// <param name="port">rtp监听端口(包括udp/tcp)</param>
        /// <returns>0:失败,非0:端口号</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_rtp_server_start(ushort port);


        /// <summary>
        /// 创建shell服务器
        /// </summary>
        /// <param name="port">shell监听端口</param>
        /// <returns>0:失败,非0:端口号</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern ushort mk_shell_server_start(ushort port);


    }
}
