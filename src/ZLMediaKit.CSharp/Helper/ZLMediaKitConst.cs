using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Helper
{
    internal class ZLMediaKitConst
    {
        /// <summary>
        /// IP地址长度
        /// </summary>
        internal const int IpStringLength = 15;

        /// <summary>
        /// 可推流的Sechame
        /// </summary>
        internal static readonly string[] PusherSechames = { "rtmp", "rtsp" };

        internal const string DefaultVhost = "__defaultVhost__";


    }
}
