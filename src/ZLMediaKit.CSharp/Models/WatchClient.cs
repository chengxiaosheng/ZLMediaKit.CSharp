using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    /// <summary>
    /// 播放者客户端
    /// </summary>
    public class WatchClient
    {
        private WatchClient()
        {

        }
        internal WatchClient(SockInfo sockInfo)
        {
            this.BeginTime = DateTime.Now;
            this.SockInfo = sockInfo;
        }

        /*
         * 暂时没办法主动关闭连接，嗯 这个目前比较可惜
         * 客户端连接对象主要来自于OnPlay事件，OnPlay目前是 sock 网络套字节
         * 虽然我们有套字节的指针，但是目前并没有公布关闭的api
         */
        //todo : 从ZLM的c api中新增套字节关闭的api

        /// <summary>
        /// 网络套接字
        /// </summary>
        public SockInfo SockInfo { get; internal set; }

        /// <summary>
        /// 开始播放时间
        /// </summary>
        public DateTime BeginTime { get; internal set; }

        /// <summary>
        /// 结束播放时间
        /// </summary>
        public DateTime? EndTime { get; internal set; }

        /// <summary>
        /// 总流量
        /// </summary>
        public long? TotalFlow { get; internal set; }

    }
}
