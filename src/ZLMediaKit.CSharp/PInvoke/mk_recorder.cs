using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 创建flv录制器
        /// </summary>
        /// <returns></returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_flv_recorder_create();


        /// <summary>
        /// 释放flv录制器
        /// </summary>
        /// <param name="mk_flv_recorder"></param>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern void mk_flv_recorder_release(System.IntPtr mk_flv_recorder);



        /// <summary>
        /// 开始录制flv
        /// </summary>
        /// <param name="mk_flv_recorder">flv录制器</param>
        /// <param name="vhost">虚拟主机</param>
        /// <param name="app">绑定的RtmpMediaSource的 app名</param>
        /// <param name="stream">绑定的RtmpMediaSource的 stream名</param>
        /// <param name="file_path">文件存放地址</param>
        /// <returns>0:开始超过，-1:失败,打开文件失败或该RtmpMediaSource不存在</returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_flv_recorder_start(IntPtr mk_flv_recorder,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string stream,
                                                         [In()][MarshalAs(UnmanagedType.LPStr)] string file_path);


        /// <summary>
        /// 获取录制状态
        /// </summary>
        /// <param name="type">0:hls,1:MP4</param>
        /// <param name="vhost">虚拟主机</param>
        /// <param name="app">应用名</param>
        /// <param name="stream">流id</param>
        /// <returns>录制状态,0:未录制, 1:正在录制</returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_recorder_is_recording(int type,
                                                            [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                            [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                            [In()][MarshalAs(UnmanagedType.LPStr)] string stream);

        /// <summary>
        /// 开始录制
        /// </summary>
        /// <param name="type">0:hls,1:MP4</param>
        /// <param name="vhost">虚拟主机</param>
        /// <param name="app">应用名</param>
        /// <param name="stream">流id</param>
        /// <param name="customized_path">录像文件保存自定义目录，默认为空或null则自动生成</param>
        /// <returns>1代表成功，0代表失败</returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_recorder_start(int type,
                                                     [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                     [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                     [In()][MarshalAs(UnmanagedType.LPStr)] string stream,
                                                     [In()][MarshalAs(UnmanagedType.LPStr)] string customized_path);


        /// <summary>
        /// 停止录制
        /// </summary>
        /// <param name="type">0:hls,1:MP4</param>
        /// <param name="vhost">虚拟主机</param>
        /// <param name="app">应用名</param>
        /// <param name="stream">流id</param>
        /// <returns> 1:成功，0：失败</returns>
        [DllImport(NativeLibrary,CallingConvention = CallingConvention)]
        internal static extern int mk_recorder_stop(int type,
                                                    [In()][MarshalAs(UnmanagedType.LPStr)] string vhost,
                                                    [In()][MarshalAs(UnmanagedType.LPStr)] string app,
                                                    [In()][MarshalAs(UnmanagedType.LPStr)] string stream);
    }
}
