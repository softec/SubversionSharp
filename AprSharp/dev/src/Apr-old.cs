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
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Softec.AprSharp
{
    public class Apr : IDisposable
    {
        private static Apr mSingleton = null;
    
        #region DLL imports
	    [DllImport("apr-0")]
        private static extern int apr_initialize( );
        
	    [DllImport("apr-0")]
        private static extern void apr_terminate2( );

	    [DllImport("apr-0"), CLSCompliant(false)]
        private static extern void apr_strerror(int apr_status,
                                                StringBuilder buf,
                                                uint size);
        #endregion

        #region Constructor / IDisposable
        public static Apr Initialize()
        {
            lock( typeof(Apr) ) {
                if(Apr.mSingleton == null)
                {
                    Apr.mSingleton = new Apr();
                }
                return Apr.mSingleton;
            }
        }
        
        public void Dispose()
        {
            lock( typeof(Apr) ) {
                if (Apr.mSingleton != null)
                {
                    Debug.Write("apr_terminate2...");
                    apr_terminate2();
                    Debug.WriteLine("Done");
                    Apr.mSingleton = null;
                    GC.SuppressFinalize(this);
                }
            }
        }

        private Apr() {
            int apr_status;
            Debug.Write("apr_initialize...");
            apr_status = apr_initialize();
            Debug.WriteLine("Done");
            if( apr_status != 0 )
                throw new AprException(apr_status);
        }
        
        ~Apr() {
            Dispose();
        }
        #endregion
 
        public static string StrError(int apr_status)
        {       
            StringBuilder buf = new StringBuilder (1024);
            Apr.apr_strerror(apr_status, buf, (uint)buf.Capacity);
            return(buf.ToString());
        }   
    }
}   