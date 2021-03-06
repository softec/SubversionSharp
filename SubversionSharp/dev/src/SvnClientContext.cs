//  SubversionSharp, a wrapper library around the Subversion client API
#region Copyright (C) 2004 SOFTEC sa.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//  Sources, support options and lastest version of the complete library
//  is available from:
//		http://www.softec.st/SubversionSharp
//		Support@softec.st
//
//  Initial authors : 
//		Denis Gervalle
//		Olivier Desaive
#endregion
//
using System;
using System.Diagnostics;
using Softec.AprSharp;
using System.Runtime.InteropServices;

namespace Softec.SubversionSharp
{

    public unsafe class SvnClientContext : IAprUnmanaged
    {
        private svn_client_ctx_t *mClientContext;
        private SvnAuthBaton mAuthBaton;
        private SvnDelegate mNotifyFunc;
        private SvnDelegate mLogMsgFunc;
        private SvnDelegate mCancelFunc;

        [StructLayout( LayoutKind.Sequential, Pack=4 )]
        private struct svn_client_ctx_t
        {  
			public IntPtr auth_baton;
			public IntPtr notify_func;
			public IntPtr notify_baton;
			public IntPtr log_msg_func;
			public IntPtr log_msg_baton;
			public IntPtr config;
			public IntPtr cancel_func;
			public IntPtr cancel_baton;
        }

        #region Generic embedding functions of an IntPtr
        private SvnClientContext(svn_client_ctx_t *ptr)
        {
            mClientContext = ptr;
            mAuthBaton = new SvnAuthBaton();
        	mNotifyFunc = SvnDelegate.NullFunc;
        	mLogMsgFunc = SvnDelegate.NullFunc;
       		mCancelFunc = SvnDelegate.NullFunc;
        }
        
        public SvnClientContext(IntPtr ptr)
        {
            mClientContext = (svn_client_ctx_t *) ptr.ToPointer();
            mAuthBaton = new SvnAuthBaton();
        	mNotifyFunc = SvnDelegate.NullFunc;
        	mLogMsgFunc = SvnDelegate.NullFunc;
       		mCancelFunc = SvnDelegate.NullFunc;
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

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mClientContext);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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
        public SvnAuthBaton AuthBaton
		{
			get
			{
				CheckPtr();
				return(mAuthBaton);
			}
			set
			{
				CheckPtr();
				mAuthBaton = value;
				mClientContext->auth_baton = mAuthBaton;
			}
		}
		
		public SvnDelegate NotifyFunc
		{
			get
			{
				CheckPtr();
				return(mNotifyFunc);
			}
			set
			{
				CheckPtr();
				mNotifyFunc = value;
				mClientContext->notify_func = mNotifyFunc.WrapperPtr;
				//fixed(IntPtr *ptr = &(mClientContext->notify_func))
				//	mNotifyFunc.MarshalWrapperToPtr(new IntPtr(ptr));
			}
		}

		public IntPtr NotifyBaton
		{
			get
			{
				CheckPtr();
				return(mClientContext->notify_baton);

			}
			set
			{
				CheckPtr();
				mClientContext->notify_baton = value;
			}
		}

		public SvnDelegate LogMsgFunc
		{
			get
			{
				CheckPtr();
				return(mLogMsgFunc);
			}
			set
			{
				CheckPtr();
				mLogMsgFunc = value;
				mClientContext->log_msg_func = mLogMsgFunc.WrapperPtr;
				//fixed(IntPtr *ptr = &(mClientContext->log_msg_func))
				//	mLogMsgFunc.MarshalWrapperToPtr(new IntPtr(ptr));
			}
		}

		public IntPtr LogMsgBaton
		{
			get
			{
				CheckPtr();
				return(mClientContext->log_msg_baton);

			}
			set
			{
				CheckPtr();
				mClientContext->log_msg_baton = value;
			}
		}
		
        public AprHash Config
		{
			get
			{
				CheckPtr();
				return(mClientContext->config);
			}
			set
			{
				CheckPtr();
				mClientContext->config = value;
			}
		}
        
        public SvnDelegate CancelFunc
		{
			get
			{
				CheckPtr();
				return(mCancelFunc);
			}
			set
			{
				CheckPtr();
				mCancelFunc = value;
				mClientContext->cancel_func = mCancelFunc.WrapperPtr;
				//fixed(IntPtr *ptr = &(mClientContext->cancel_func))
				//	mCancelFunc.MarshalWrapperToPtr(new IntPtr(ptr));
			}
		}

		public IntPtr CancelBaton
		{
			get
			{
				CheckPtr();
				return(mClientContext->cancel_baton);

			}
			set
			{
				CheckPtr();
				mClientContext->cancel_baton = value;
			}
		}
		#endregion
    }
}