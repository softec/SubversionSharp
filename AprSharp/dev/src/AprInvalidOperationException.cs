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
	public class AprInvalidOperationException : AprException
	{
		const int Result = unchecked ((int)0xA0651509);
                                                                                                                                  
		// Constructors
		public AprInvalidOperationException ()
			: base ("The requested operation could be performed.")
		{
			HResult = Result;
		}
                                                                                                                                  
		public AprInvalidOperationException (string message)
			: base (message)
		{
			HResult = Result;
		}
                                                                                                                                  
		public AprInvalidOperationException (string message, Exception innerException)
			: base (message, innerException)
		{
			HResult = Result;
		}
                                                                                                                                  
        public AprInvalidOperationException(int apr_status) 
               : base ( apr_status )
        {
        }
        
        public AprInvalidOperationException(int apr_status, Exception innerException) 
               : base ( apr_status, innerException )
        {
        }
        
		protected AprInvalidOperationException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
	}
}
