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
    public class SvnException : SoftecException
    {
        const int Result = unchecked ((int)0xA0520000);
        
        public SvnException() 
               : base ( "An unknown exception from Subversion Library has occured." )
        {
            HResult = Result;
        }

        public SvnException(string s) 
               : base ( s )
        {
            HResult = Result;
        }

        public SvnException(string s, Exception innerException) 
               : base ( s, innerException )
        {
            HResult = Result;
        }

        public SvnException(SvnError error) 
               : base ( error.Message )
        {
            HResult = unchecked (Result + error.AprErr);
        }
        
        public SvnException(SvnError error, Exception innerException) 
               : base ( error.Message, innerException )
        {
            HResult = unchecked (Result + error.AprErr);
        }

        public SvnException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
        }
    }
}