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
    public class AprArgumentOutOfRangeException : AprArgumentException
    {
        const int Result = unchecked ((int)0xA0651502);

        private object mActualValue;
        
        public AprArgumentOutOfRangeException() 
               : base ( "Argument is out of range." )
        {
            HResult = Result;
            mActualValue = null;
        }

        public AprArgumentOutOfRangeException(string paramName) 
               : base ( "Argument is out of range.", paramName )
        {
            HResult = Result;
            mActualValue = null;
        }

        public AprArgumentOutOfRangeException(string paramName, string message) 
               : base ( message, paramName )
        {
            HResult = Result;
            mActualValue = null;
        }

        public AprArgumentOutOfRangeException(string paramName, 
                                              object actualValue,
                                              string message) 
               : base ( message, paramName )
        {
            mActualValue = actualValue;
            HResult = Result;
        }

        public AprArgumentOutOfRangeException(string paramName, int apr_status) 
               : base ( apr_status, paramName )
        {
            mActualValue = null;
        }

        public AprArgumentOutOfRangeException(string paramName, 
                                              object actualValue,
                                              int apr_status) 
               : base ( apr_status, paramName )
        {
            mActualValue = actualValue;
        }

        public AprArgumentOutOfRangeException(string paramName, 
                                              int actualValue,
                                              int minval, int maxval) 
               : base ( "Expect an integer value between " + minval 
                                                 + " and " + maxval + ".",
                        paramName )
        {
            mActualValue = (object)actualValue;
        }

        public AprArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
               : base (info, context)
        {
            mActualValue = info.GetString("ActualValue");
        }
        
        public virtual object ActualValue
        {
            get
            {
                return mActualValue;
            }
        }
        
        public override string Message
        {
            get
            {
                string baseMessage = base.Message;
                if(mActualValue == null)
                    return baseMessage;
                
                return( baseMessage + Environment.NewLine + mActualValue );
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info,context);
            info.AddValue("ActualValue", mActualValue);
        }                 
    }
}