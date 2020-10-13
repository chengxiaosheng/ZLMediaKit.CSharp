using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.dtos
{
    public class RtspAuth
    {
        /// <summary>
        /// 上一步定义得Realm
        /// </summary>
        public string Realm { get; internal set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; internal set; }
        /// <summary>
        /// 如果为true，则必须提供明文密码(因为此时是base64认证方式),否则会导致认证失败
        /// </summary>
        public bool MustNoEncrypt { get; internal set; }
        /// <summary>
        /// 连接信息
        /// </summary>
        public SockInfo SockInfo { get; internal set; }

        internal MediaInfo MediaInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MediaSourceWapper MediaSourceWapper { get; internal set; }
    }
}
