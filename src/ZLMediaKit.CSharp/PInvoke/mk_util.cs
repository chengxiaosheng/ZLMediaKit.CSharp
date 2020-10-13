using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLMediaKit.CSharp.Helper;
using ZLMediaKit.CSharp.Variables;

namespace ZLMediaKit.CSharp.PInvoke
{
    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 获取本程序可执行文件路径
        /// </summary>
        /// <returns>文件路径，使用完后需要自己free</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal unsafe static extern char* mk_util_get_exe_path();

        /// <summary>
        /// 获取本程序可执行文件相同目录下文件的绝对路径
        /// </summary>
        /// <param name="relative_path">同目录下文件的路径相对,可以为null</param>
        /// <returns>文件路径，使用完后需要自己free</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal unsafe static extern char* mk_util_get_exe_dir([In()][MarshalAs(UnmanagedType.LPStr)] string relative_path);

        /// <summary>
        /// 获取unix标准的系统时间戳
        /// </summary>
        /// <returns>当前系统时间戳</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern UInt64 mk_util_get_current_millisecond();

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <returns></returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal unsafe static extern char* mk_util_get_current_time_string([In()][MarshalAs(UnmanagedType.LPStr)]string fmt);

        /// <summary>
        /// 打印二进制为字符串
        /// </summary>
        /// <param name="buf">二进制数据</param>
        /// <param name="len">数据长度</param>
        /// <returns>可打印的调试信息，使用完后需要自己free</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal unsafe static extern char* mk_util_hex_dump(void * buf ,int len);

        #region removed

        // 通过 msvcrt->vprintf(const char * fmt,...) 方法尝试确定 C# PInvoke 模式是可以call c __VA_ARGS__ 可变参
        // 但是在call zlmediakit->mk_log_printf 提示 System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt
        // 暂时未能成功调用到 ZLMediaKit 的 mk_log_printf方法
        /*
         * 这里仅仅只是研究 C# PInvoke -> C __VA_ARGS__ 
         * 当然 ，从项目的定位而言，离交互最近的应用才应该是日志的输出者，包括当前的 中间库也不应该直接写入日志
         * 因此，从这里调用ZLMediaKit 的日志方法其实是不合理的 
         * 合理的方式应该由ZLMediaKit 回调当前库的日志方法，当前库回调上层库的日志方法，从而实现全局日志统一处理
         */

        ///// <summary>
        ///// 打印日志
        ///// </summary>
        ///// <param name="level">日志级别,支持0~4</param>
        ///// <param name="file">__FILE__</param>
        ///// <param name="function">__FUNCTION__</param>
        ///// <param name="line">__LINE__</param>
        ///// <param name="fmt">printf类型的格式控制字符串</param>
        ///// <param name="p">不定长参数</param>
        //[DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        //internal unsafe static extern void mk_log_printf(int level,
        //                                                 [MarshalAs(UnmanagedType.LPStr)] string file,
        //                                                 [MarshalAs(UnmanagedType.LPStr)] string function,
        //                                                 int line,
        //                                                 [MarshalAs(UnmanagedType.LPStr)] string fmt ,
        //                                                   IntPtr intPtr);




        //private static string __FILE__ = "./log/log_11111";
        //private static string __FUNCTION__ = "test_printf_info";

        //[DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
        //static extern int vprintf(string format, IntPtr ptr);

        //internal unsafe static void log_info(string formmat, params string[] args)
        //{
        //    using (var list = new VaList(false, args))
        //    {
        //        var ptr = list.AddrOfPinnedObject();
        //        var result = vprintf(formmat, ptr);
        //        mk_log_printf(1, __FILE__, __FUNCTION__, 1, formmat, ptr);

        //    }
        //}

        #endregion removed

    }
}
