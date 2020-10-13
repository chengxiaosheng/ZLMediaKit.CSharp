using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZLMediaKit.CSharp.dtos
{
    /// <summary>
    /// 播放状态
    /// </summary>
    public enum EM_PlayStatus
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Init = 0,
        /// <summary>
        /// 播放中
        /// </summary>
        [Description("播放中")]
        Playing ,
        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed
    }
}
