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
    public class AprArgumentException : AprException
    {
        const int Result = unchecked ((int)0xA0650057);

        private string mParamName;
        
        public AprArgumentException() 
               : base ( "An invalid argument was specified." )
        {
            HResult = Result;
            mParamName = null;
        }

        public AprArgumentException(string message) 
               : base ( message )
        {
            HResult = Result;
            mParamName = null;
        }

        public AprArgumentException(string message, string paramName) 
               : base ( message )
        {
            HResult = Result;
            this.mParamName = paramName;
        }

        public AprArgumentException(int apr_status) 
               : base ( apr_status )
        {
            mParamName = null;
        }
        
        public AprArgumentException(int apr_status, string paramName) 
               : base ( apr_status )
        {
            mParamName = paramName;
        }
        
        public AprArgumentException(int apr_status, Exception innerException) 
               : base ( apr_status, innerException )
        {
            mParamName = null;
        }

        public AprArgumentException(int apr_status, string paramName, Exception innerException) 
               : base ( apr_status, innerException )
        {
            mParamName = paramName;
        }

        public AprArgumentException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
            mParamName = info.GetString("ParamName");
        }
        
        public virtual string ParamName
        {
            get
            {
                return mParamName;
            }
        }
        
        public override string Message
        {
            get
            {
                string baseMessage = base.Message;
                if(baseMessage == null)
                    baseMessage = "An invalid argument was specified.";
                
                if(mParamName == null)
                    return baseMessage;
                else
                    return( baseMessage + Environment.NewLine
                            + "Parameter name: " + mParamName );
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info,context);
            info.AddValue("ParamName", mParamName);
        }                 
    }
}