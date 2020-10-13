using System;
using System.Runtime.InteropServices;

namespace ZLMediaKit.CSharp.PInvoke
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user_data">用户自定义数据</param>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate void on_mk_async(IntPtr user_data);

    /// <summary>
    /// 定时器触发事件
    /// </summary>
    /// <param name="user_data">用户自定义数据</param>
    /// <returns>下一次触发延时(单位毫秒)，返回0则不再重复</returns>
    [UnmanagedFunctionPointer(ZLMediaKitMethod.CallingConvention)]
    internal delegate uint on_mk_timer(IntPtr user_data);


    internal partial class ZLMediaKitMethod
    {
        /// <summary>
        /// 获取tcp会话对象所在事件线程
        /// </summary>
        /// <param name="mk_tcp_session">tcp会话对象</param>
        /// <returns>对象所在事件线程</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_thread_from_tcp_session(IntPtr mk_tcp_session);


        /// <summary>
        /// 获取tcp客户端对象所在事件线程
        /// </summary>
        /// <param name="mk_tcp_client">tcp客户端</param>
        /// <returns>对象所在事件线程</returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_thread_from_tcp_client(IntPtr mk_tcp_client);

        /// <summary>
        /// 根据负载均衡算法，从事件线程池中随机获取一个事件线程
        /// </summary>
        /// <returns>事件线程</returns>
        /// <remarks>如果在事件线程内执行此函数将返回本事件线程；事件线程指的是定时器、网络io事件线程</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_thread_from_pool();

        /// <summary>
        /// 根据负载均衡算法，从后台线程池中随机获取一个线程
        /// </summary>
        /// <returns>后台线程</returns>
        /// <remarks>后台线程本质与事件线程相同，只是优先级更低，同时可以执行短时间的阻塞任务 ; ZLMediaKit中后台线程用于dns解析、mp4点播时的文件解复用</remarks>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_thread_from_pool_work();


        /// <summary>
        /// 切换到事件线程并异步执行
        /// </summary>
        /// <param name="mk_thread">事件线程</param>
        /// <param name="callback">回调函数</param>
        /// <param name="user_data">用户数据指针</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_async_do(IntPtr mk_thread, on_mk_async callback, IntPtr user_data);

        /// <summary>
        /// 切换到事件线程并同步执行
        /// </summary>
        /// <param name="mk_thread">事件线程</param>
        /// <param name="callback">回调函数</param>
        /// <param name="user_data"></param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_sync_do(IntPtr mk_thread, on_mk_async callback, System.IntPtr user_data);


        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <param name="mk_thread">线程对象</param>
        /// <param name="delay_ms">执行延时，单位毫秒</param>
        /// <param name="callback">回调函数</param>
        /// <param name="user_data">用户数据指针</param>
        /// <returns>定时器对象 mk_timer </returns>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern IntPtr mk_timer_create(IntPtr mk_thread, uint delay_ms, on_mk_timer callback, IntPtr user_data);


        /// <summary>
        /// 销毁和取消定时器
        /// </summary>
        /// <param name="mk_timer">定时器对象</param>
        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        internal static extern void mk_timer_release(IntPtr mk_timer);





    }
}
