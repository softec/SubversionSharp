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
using Softec;

namespace Softec.AprSharp
{
    [Serializable]
    public class AprNullReferenceException : AprException
    {
        const int Result = unchecked ((int)0xA0654003);
        
        public AprNullReferenceException() 
               : base ( "An null or uninitialized instance was found where a valid instance is expected." )
        {
            HResult = Result;
        }

        public AprNullReferenceException(string s) 
               : base ( s )
        {
            HResult = Result;
        }

        public AprNullReferenceException(string s, Exception innerException) 
               : base ( s, innerException )
        {
            HResult = Result;
        }

        public AprNullReferenceException(int apr_status) 
               : base ( apr_status )
        {
        }
        
        public AprNullReferenceException(int apr_status, Exception innerException) 
               : base ( apr_status, innerException )
        {
        }

        public AprNullReferenceException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
        }
    }
}