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
   public unsafe class SvnError
   {
        private svn_error_t *mError;

        [StructLayout( LayoutKind.Sequential )]
        private struct svn_error_t
        {
            public int apr_err;
            public SByte *message;
            public svn_error_t *child;
            public IntPtr pool;
            public SByte *file;
            public int line;
        }

        #region Generic embedding functions of an IntPtr
        private SvnError(svn_error_t *ptr)
        {
            mError = ptr;
        }

        public SvnError(IntPtr ptr)
        {
            mError = ptr.ToPointer();
        }
        
        public bool IsNoError()
        {
            return( mError == null );
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

        public static implicit operator IntPtr(SvnError error)
        {
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
        
        #region Wrapper properties
        public int AprErr
        {
            get
            {
                CheckPtr();
                return(mError->apr_err);
            }            
        }        

        public string Message
        {
            get
            {
                CheckPtr();
                return(new String(mError->message));
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

        public string File
        {
            get
            {
                CheckPtr();
                return(new String(mError->file));
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