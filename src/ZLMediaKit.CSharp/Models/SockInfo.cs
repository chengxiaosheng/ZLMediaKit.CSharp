using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.CSharp.ZLMediaKit;

namespace ZLMediaKit.CSharp.Models
{
    public class SockInfo
    {

        internal SockInfo(IntPtr id)
        {
            this._id = id;
        }

        /// <summary>
        /// Tcp Session 对象
        /// </summary>
        internal IntPtr _id;


        /// <summary>
        /// TCP会话唯一标识
        /// </summary>
        public Int64 Id => (Int64)_id;

        /// <summary>
        /// 对端IP
        /// </summary>
        public string PeerIp => Tcp.GetSockPeerIp(this._id).Result;
        /// <summary>
        /// 对端端口
        /// </summary>
        public ushort PeerPort => Tcp.GetSockPeerPort(this._id).Result;
        /// <summary>
        /// 本地IP
        /// </summary>
        public string LocalIp => Tcp.GetSockLocalIp(this._id).Result;
        /// <summary>
        /// 本地端口
        /// </summary>
        public ushort LocalPort => Tcp.GetSockLocalPort(this._id).Result;

      
    }
}
