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

namespace Softec.SubversionSharp
{
    [Serializable]
    public class SvnNullReferenceException : SvnException
    {
        const int Result = unchecked ((int)0xA0834003);
        
        public SvnNullReferenceException() 
               : base ( "An null or uninitialized instance was found where a valid instance is expected." )
        {
            HResult = Result;
        }

        public SvnNullReferenceException(string s) 
               : base ( s )
        {
            HResult = Result;
        }

        public SvnNullReferenceException(string s, Exception innerException) 
               : base ( s, innerException )
        {
            HResult = Result;
        }

        public SvnNullReferenceException(SvnError error) 
               : base ( error )
        {
        }
        
        public SvnNullReferenceException(SvnError error, Exception innerException) 
               : base ( error, innerException )
        {
        }

        public SvnNullReferenceException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
        }
    }
}