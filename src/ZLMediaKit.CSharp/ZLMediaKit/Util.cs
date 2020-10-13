using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZLMediaKit.CSharp.Helper;

namespace ZLMediaKit.CSharp.ZLMediaKit
{
    public class Util
    {
        /// <summary>
        /// 打印二进制为字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public unsafe static string HexDump(IntPtr buffer, int length) => HexDump((void*)buffer, length);
       

        /// <summary>
        /// 打印二进制为字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public unsafe static string HexDump(void * buffer,int length)
        {
            IntPtr outInptr = IntPtr.Zero;
            try
            {
                outInptr  = (IntPtr)PInvoke.ZLMediaKitMethod.mk_util_hex_dump(buffer, length);
                return Marshal.PtrToStringAnsi(outInptr);
            }
            finally
            {
                if (outInptr != IntPtr.Zero) Marshal.FreeHGlobal(outInptr);
            }
        }
        /// <summary>
        /// 打印二进制为字符串
        /// </summary>
        /// <param name="buffer">二进制数据</param>
        /// <returns></returns>
        public unsafe static string HexDump(byte[] buffer) //=> HexDump((void*)buffer., buffer.Length);
        {
            fixed(void * buff = buffer )
            {
                return HexDump(buff, buffer.Length);
            }
        }

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="formmat">时间格式，譬如%Y-%m-%d %H:%M:%S</param>
        /// <returns></returns>
        public unsafe static string GetCurrentTimeString(string formmat)
        {
            IntPtr outInptr = IntPtr.Zero;
            try
            {
                outInptr = (IntPtr)PInvoke.ZLMediaKitMethod.mk_util_get_current_time_string(formmat);
                return Marshal.PtrToStringAnsi(outInptr);
            }
            finally
            {
                Marshal.FreeHGlobal(outInptr); 
            }
        }

        /// <summary>
        /// 获取unix标准的系统时间戳
        /// </summary>
        /// <returns></returns>
        public  static UInt64 GetCurrentMillSecond()
        {
            return PInvoke.ZLMediaKitMethod.mk_util_get_current_millisecond();
        }

        /// <summary>
        /// 获取本程序可执行文件相同目录下文件的绝对路径
        /// </summary>
        /// <param name="relative_path">同目录下文件的路径相对,可以为null</param>
        /// <returns></returns>
        public unsafe static string GetAppDir(string relative_path)
        {
            IntPtr outInptr = IntPtr.Zero;
            try
            {
                outInptr = (IntPtr)PInvoke.ZLMediaKitMethod.mk_util_get_exe_dir(relative_path);
                return Marshal.PtrToStringAnsi(outInptr);
            }
            finally
            {
                Marshal.FreeHGlobal(outInptr);
            }
        }

        /// <summary>
        /// 获取本程序可执行文件路径
        /// </summary>
        /// <returns></returns>
        public unsafe static string GetAppPath()
        {
            IntPtr outInptr = IntPtr.Zero;
            try
            {
                outInptr = (IntPtr)PInvoke.ZLMediaKitMethod.mk_util_get_exe_path();
                return Marshal.PtrToStringAnsi(outInptr);
            }
            finally
            {
                Marshal.FreeHGlobal(outInptr);
            }
        }
    }
}
