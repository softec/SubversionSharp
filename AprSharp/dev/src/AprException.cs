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
using System.Runtime.Serialization;

namespace Softec.AprSharp
{
    [Serializable]
    public class AprException : Exception
    {
        const int Result = unchecked ((int)0xA0400000);
        
        public AprException() 
               : base ( "An unknown exception from Apr Library has occured." )
        {
            HResult = Result;
        }

        public AprException(string s) 
               : base ( s )
        {
            HResult = Result;
        }

        public AprException(string s, Exception innerException) 
               : base ( s, innerException )
        {
            HResult = Result;
        }

        public AprException(int apr_status) 
               : base ( Apr.StrError(apr_status) )
        {
            HResult = unchecked (Result + apr_status);
        }
        
        public AprException(int apr_status, Exception innerException) 
               : base ( Apr.StrError(apr_status), innerException )
        {
            HResult = unchecked (Result + apr_status);
        }

        public AprException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
        }
    }
}