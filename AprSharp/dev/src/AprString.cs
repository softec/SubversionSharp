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

namespace Softec.AprSharp
{
    public struct AprString : IAprUnmanaged
    {
        IntPtr mString;

        #region Generic embedding functions of an IntPtr
        public AprString(IntPtr ptr)
        {
            mString = ptr;
        }

        public AprString(string str, AprPool pool)
        {
            mString = Apr.apr_pstrdup(pool, str);
        }
        
        public AprString(AprString str, AprPool pool)
        {
            mString = Apr.apr_pstrdup(pool, str);
        }

        public AprString(string str, int size, AprPool pool)
        {
            mString = Apr.apr_pstrndup(pool, str, unchecked((uint)size));
        }
        
        public AprString(AprString str, int size, AprPool pool)
        {
            mString = Apr.apr_pstrndup(pool, str, unchecked((uint)size));
        }

        [CLSCompliant(false)]
        public AprString(string str, uint size, AprPool pool)
        {
            mString = Apr.apr_pstrndup(pool, str, size);
        }

        [CLSCompliant(false)]
        public AprString(AprString str, uint size, AprPool pool)
        {
            mString = Apr.apr_pstrndup(pool, str, size);
        }

        public bool IsNull
        {
        	get
        	{
            	return( mString == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mString = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mString;
        }

        public static implicit operator IntPtr(AprString str)
        {
            return str.mString;
        }
        
        public static implicit operator AprString(IntPtr ptr)
        {
            return new AprString(ptr);
        }

        public override string ToString()
        {
        	if( IsNull )
        		return("[apr_string:NULL]");
        	else
            	return(Marshal.PtrToStringAnsi(mString));
        }
        #endregion
        
        #region Methods wrappers
        public static AprString Duplicate(AprPool pool, string str)
        {
            return(new AprString(str, pool));
        }

        public static AprString Duplicate(AprPool pool, AprString str)
        {
            return(new AprString(str, pool));
        }

        public static AprString Duplicate(AprPool pool, string str, int size)
        {
            return(new AprString(str, size, pool));
        }

        [CLSCompliant(false)]
        public static AprString Duplicate(AprPool pool, string str, uint size)
        {
            return(new AprString(str, size, pool));
        }
        
        public static AprString Duplicate(AprPool pool, AprString str, int size)
        {
            return(new AprString(str, size, pool));
        }
        
        [CLSCompliant(false)]
        public static AprString Duplicate(AprPool pool, AprString str, uint size)
        {
            return(new AprString(str, size, pool));
        }
        #endregion
    }
}