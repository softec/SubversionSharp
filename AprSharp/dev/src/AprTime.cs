//
// Softec
//
// Contact: Support@softec.st
//
// Designed by Denis Gervalle and Olivier Desaive
// Written by Denis Gervalle
//
// Copyright 2004 by SOFTEC. All rights reserved.
//
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Softec.AprSharp
{
    public class AprTime
    {
        private AprTime()
        {
        }

        public static long Now()
        {
            return(Apr.apr_time_now());
        }

        public static string Rfc822Date(long value)
        {
            StringBuilder buf = new StringBuilder(32);
            int res = Apr.apr_rfc822_date(buf,value);
            if (res != 0)
                throw new AprException(res);
            return(buf.ToString());
        }

        public static string CTime(long value)
        {
            StringBuilder buf = new StringBuilder(32);
            int res = Apr.apr_ctime(buf,value);
            if (res != 0)
                throw new AprException(res);
            return(buf.ToString());
        }
    }
}