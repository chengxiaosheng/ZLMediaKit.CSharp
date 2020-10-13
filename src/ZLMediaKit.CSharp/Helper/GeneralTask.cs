using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.CSharp.Helper
{
    internal class GeneralTask
    {
        private static ILogger logger = NullLoggerFactory.Instance.CreateLogger("ZLMediaKit.CSharp");

        internal static Task WriteFaultedLog(Task t)
        {
            if (t.Exception is object) logger.LogError(t.Exception,""); 
            return t;
        }

        internal static T WriteFaultedLog<T>(Task<T> t)
        {
            logger.LogError(t.Exception, "");
            throw t.Exception;
        }
    }
}
