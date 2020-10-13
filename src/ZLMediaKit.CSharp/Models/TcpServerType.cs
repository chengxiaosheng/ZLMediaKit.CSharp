using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public enum TcpType
    {
        /// <summary>
        /// 普通的tcp
        /// </summary>
        [Description("普通的tcp")]
        mk_type_tcp = 0,
        /// <summary>
        /// ssl类型的tcp
        /// </summary>
        [Description("ssl类型的tcp")]
        mk_type_ssl = 1,
        /// <summary>
        /// 基于webSock的连接
        /// </summary>
        [Description("基于webSock的连接")]
        mk_type_ws = 2,
        /// <summary>
        /// 基于ssl webSock的连接
        /// </summary>
        [Description("基于ssl webSock的连接")]
        mk_type_wss = 3
    }
}
