using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public enum EM_Codec : byte
    {
        /// <summary>
        /// H.264/AVC
        /// </summary>
        [Description("H.264/AVC")]
        H264 = 0,
        /// <summary>
        /// 高效视频编码
        /// <code>High Efficiency Video Coding</code>
        /// </summary>
        [Description("High Efficiency Video Coding")]
        H265,
        /// <summary>
        /// 高级音频编码
        /// <code>Advanced Audio Coding</code>
        /// </summary>
        [Description("Advanced Audio Coding")]
        AAC,
        /// <summary>
        /// G711 Alaw
        /// </summary>
        [Description("G711 Alaw")]
        G711A,
        /// <summary>
        /// G711 Ulaw
        /// </summary>
        [Description("G711 Ulaw")]
        G711U,
        /// <summary>
        /// OPUS
        /// </summary>
        [Description("OPUS")]
        OPUS
    }
}
