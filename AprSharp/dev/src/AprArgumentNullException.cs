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
    public class AprArgumentNullException : AprArgumentException
    {
        const int Result = unchecked ((int)0xA0654003);

        public AprArgumentNullException() 
               : base ("Argument cannot be null.")
        {
            HResult = Result;
        }

        public AprArgumentNullException(string paramName) 
               : base ("Argument cannot be null.", paramName)
        {
            HResult = Result;
        }

        public AprArgumentNullException(string paramName, string message) 
               : base ( message, paramName )
        {
            HResult = Result;
        }

        public AprArgumentNullException(string paramName, int apr_status) 
               : base ( apr_status, paramName )
        {
        }
        
        public AprArgumentNullException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
        }
    }
}