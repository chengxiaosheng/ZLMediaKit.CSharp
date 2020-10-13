using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.CSharp.Helper
{
    public static class UnsafeString
    {
        public static unsafe String CStringToString(char * ptr, Encoding encoding)
        {
            if (ptr == null) return "";

            int lengthOfCString = 0;
            while (ptr[lengthOfCString] != '\0')
            {
                lengthOfCString++;
            }

            // now that we have the length of the string, let's get its size in bytes
            int lengthInBytes = encoding.GetByteCount(ptr, lengthOfCString);
            byte[] asByteArray = new byte[lengthInBytes];

            fixed (byte* ptrByteArray = asByteArray)
            {
                encoding.GetBytes(ptr, lengthOfCString, ptrByteArray, lengthInBytes);
            }
            return encoding.GetString(asByteArray);
        }
    }
}
