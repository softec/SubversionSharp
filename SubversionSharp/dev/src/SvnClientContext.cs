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
using System.Diagnostics;
using Softec.AprSharp;
using System.Runtime.InteropServices;

namespace Softec.SubversionSharp
{

    public unsafe struct SvnClientContext
    {
        private svn_client_ctx_t *mClientContext;

        [StructLayout( LayoutKind.Sequential )]
        private struct svn_client_ctx_t
        {  
			public IntPtr auth_baton;
			public IntPtr notify_func;
			public void *notify_baton;
			public IntPtr log_msg_func;
			public void *log_msg_baton;
			public IntPtr config;
			public IntPtr cancel_func;
			public void *cancel_baton;
        }

        #region Generic embedding functions of an IntPtr
        private SvnClientContext(svn_client_ctx_t *ptr)
        {
            mClientContext = ptr;
        }
        
        public SvnClientContext(IntPtr ptr)
        {
            mClientContext = ptr.ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mClientContext == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mClientContext = null;
        }

        public static implicit operator IntPtr(SvnClientContext clientContext)
        {
            return new IntPtr(clientContext.mClientContext);
        }
        
        public static implicit operator SvnClientContext(IntPtr ptr)
        {
            return new SvnClientContext(ptr);
        }

        public override string ToString()
        {
            return("[svn_client_context_t:"+(new IntPtr(mClientContext)).ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public static SvnClientContext Create(AprPool pool)
        {
        	IntPtr ptr;
            
            Debug.Write(String.Format("svn_client_create_context({0})...",pool));
            SvnError err = Svn.svn_client_create_context(out ptr, pool);
            if(!err.IsNoError)
                throw new SvnException(err);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }
        #endregion

        #region Wrapper Properties
        public AprHash Config
		{
			get
			{
				return(mClientContext->config);
			}
			set
			{
				mClientContext->config = value;
			}
		}
        #endregion
    }
}