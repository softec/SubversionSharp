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
using System.Runtime.InteropServices;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
   public unsafe class SvnError : IAprUnmanaged
   {
        private svn_error_t *mError;

        [StructLayout( LayoutKind.Sequential )]
        private struct svn_error_t
        {
            public int apr_err;
            public IntPtr message;
            public svn_error_t *child;
            public IntPtr pool;
            public IntPtr file;
            public int line;
        }

        #region Generic embedding functions of an IntPtr
        private SvnError(svn_error_t *ptr)
        {
            mError = ptr;
        }

        public SvnError(IntPtr ptr)
        {
            mError = (svn_error_t *) ptr.ToPointer();
        }
        
        public static SvnError NoError = new SvnError(IntPtr.Zero);
        
        public bool IsNoError
        {
        	get
        	{
            	return( mError == null );
            }
        }

        public bool IsNull
        {
        	get
        	{
            	return( mError == null );
            }
        }
        
        private void CheckPtr()
        {
            if( mError == null )
                throw new SvnNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mError = null;
        }

        public IntPtr ToIntPtr()
        {
        	if( IsNoError )
        		return IntPtr.Zero;
        	else
            	return new IntPtr(mError);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnError error)
        {
        	if( error.IsNoError )
        		return IntPtr.Zero;
        	else
            	return new IntPtr(error.mError);
        }
        
        public static implicit operator SvnError(IntPtr ptr)
        {
            return new SvnError(ptr);
        }
        
        public override string ToString()
        {
            return("[svn_error_t:"+(new IntPtr(mError)).ToInt32().ToString("X")+"]");
        }
        #endregion
        
        #region Wrapper methods
        public static SvnError Create(int aprErr, SvnError err, string str)
        {
            return(new SvnError(Svn.svn_error_create(aprErr, new IntPtr(err.mError), str)));
        }

        public void Clear()
        {
        	CheckPtr();
            Svn.svn_error_clear(new IntPtr(mError));
        }
        #endregion

        #region Wrapper properties
        public int AprErr
        {
            get
            {
                CheckPtr();
                return(mError->apr_err);
            }            
        }        

        public AprString Message
        {
            get
            {
                CheckPtr();
                return(mError->message);
            }
        }        

        public SvnError Child
        {
            get
            {
                CheckPtr();
                return(new SvnError(mError->child));
            }            
        }        

        public AprPool Pool
        {
            get
            {
                CheckPtr();
                return(mError->pool);
            }            
        }        

        public AprString File
        {
            get
            {
                CheckPtr();
                return(mError->file);
            }            
        }        

        public int Line
        {
            get
            {
                CheckPtr();
                return(mError->line);
            }            
        }        
        #endregion
   }
}