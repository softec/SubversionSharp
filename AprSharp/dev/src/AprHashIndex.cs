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
using System.Collections;

namespace Softec.AprSharp
{
    public struct AprHashIndex
    {
        IntPtr mHashIndex;

        #region Generic embedding functions of an IntPtr
        public AprHashIndex(IntPtr ptr)
        {
            mHashIndex = ptr;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mHashIndex == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mHashIndex = IntPtr.Zero;
        }

        public static implicit operator IntPtr(AprHashIndex hashIndex)
        {
            return hashIndex.mHashIndex;
        }
        
        public static implicit operator AprHashIndex(IntPtr ptr)
        {
            return new AprHashIndex(ptr);
        }

        public override string ToString()
        {
            return("[apr_hash_index_t:"+mHashIndex.ToInt32().ToString("X")+"]");
        }
        #endregion
        
        #region Methods wrappers
        public static AprHashIndex First(AprPool pool, AprHash h)
        {
    	    IntPtr ptr;
            
            Debug.Write(String.Format("apr_hash_first({0},{1})...",pool,h));
            ptr = Apr.apr_hash_first(pool,h);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }
        
        public void Next()
        {
    	    IntPtr ptr;
            
        	CheckPtr();
            Debug.Write(String.Format("apr_hash_next({0})...",this));
            ptr = Apr.apr_hash_next(mHashIndex);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));
            mHashIndex = ptr;
        }

        public void This(out string key, out string value)
        {
            IntPtr v;
            
            This(out key, out v);
            value = Marshal.PtrToStringAnsi(v);
        }
        
        public void This(out string key, out IntPtr value)
        {
            IntPtr k;
            int size;
            
            This(out k, out size, out value);
            key = Marshal.PtrToStringAnsi(k);
        }

        public void This(out IntPtr key, out int size, out IntPtr value)
        {
        	CheckPtr();
            Debug.Write(String.Format("apr_hash_this({0})...",this));
            Apr.apr_hash_this(mHashIndex, out key, out size, out value);
            Debug.WriteLine(String.Format("Done({0:X},{1},{2:X})",key.ToInt32(),size,value.ToInt32()));
        }    
   		#endregion
  	}
 }