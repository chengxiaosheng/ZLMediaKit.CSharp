using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.dtos
{
    public class PublishInvokerDto
    {
        public string Message { get; set; }

        public bool EnableHls { get; set; } = false;

        public bool EnableMp4 { get; set; } = false;
    }
}
