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
    public unsafe struct AprArray
    {
        private apr_array_header_t *mArray;

        [StructLayout( LayoutKind.Sequential )]
        private struct apr_array_header_t
        {
    		public IntPtr pool;
			public int elt_size;
			public int nelts;
			public int nalloc;
			public byte *elts;
        }

        #region Generic embedding functions of an IntPtr
        private AprArray(apr_array_header_t *ptr)
        {
            mArray = ptr;
        }

        public AprArray(IntPtr ptr)
        {
            mArray = ptr.ToPointer();
        }
        
        public bool IsNull()
        {
            return( mArray == null );
        }

        private void CheckPtr()
        {
            if( mArray == null )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mArray = null;
        }

        public static implicit operator IntPtr(AprArray array)
        {
            return new IntPtr(array.mArray);
        }
        
        public static implicit operator AprArray(IntPtr ptr)
        {
            return new AprArray(ptr);
        }
        
        public override string ToString()
        {
            return("[apr_array_header_t:"+(new IntPtr(mArray)).ToInt32().ToString("X")+"]");
        }
        #endregion
        
        #region Methods wrappers
        public static AprArray Make(AprPool pool, int nElts, int eltSize )
        {
    	    IntPtr ptr;
            
            Debug.Write(String.Format("apr_array_make({0},{1},{2})...",pool,nElts,eltSize));
            ptr = Apr.apr_array_make(pool,nElts,eltSize);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_make: Can't create an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public AprArray Copy(AprPool pool)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_copy({0},{1})...",pool,this));
            ptr = Apr.apr_array_copy(pool,(IntPtr)mArray);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_copy: Can't copy an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public AprArray CopyHdr(AprPool pool)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_copy_hdr({0},{1})...",pool,this));
            ptr = Apr.apr_array_copy_hdr(pool,new IntPtr(mArray));
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_copy_hdr: Can't copy an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public AprArray Append(AprPool pool, AprArray array)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_append({0},{1},{2})...",pool,array,this));
            ptr = Apr.apr_array_append(pool,(IntPtr)mArray,array);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_append: Can't append an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public void Cat(AprArray array)
        {
            CheckPtr();
            Debug.WriteLine(String.Format("apr_array_cat({0},{1})",this,array));
            Apr.apr_array_cat((IntPtr)mArray,array);
        }        

        public string StrCat(AprPool pool, char sep)
        {
        	return(pStrCat(pool,sep).ToString());
        }

        public AprString pStrCat(AprPool pool, char sep)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_pstrcat({0},{1},{2})...",pool,this,sep));
            ptr = Apr.apr_array_pstrcat(pool,new IntPtr(mArray),sep);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_pstrcat: Can't convert an apr_array_header_t to AprString");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }        

        public IntPtr Push()
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_push({0})...",this));
            ptr = Apr.apr_array_push(new IntPtr(mArray));
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_push: Can't push an element");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }        
        
        public IntPtr Pop()
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_pop({0})...",this));
            ptr = Apr.apr_array_pop(new IntPtr(mArray));
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }        
        
        public bool IsEmpty()
        {
        	bool isEmpty;
            CheckPtr();
            Debug.Write(String.Format("apr_is_empty_array({0})...",this));
            isEmpty = Apr.apr_is_empty_array(new IntPtr(mArray));
            Debug.WriteLine(String.Format("Done({0:X})",isEmpty));
            return(isEmpty);
        }
        #endregion

        #region Wrapper Properties
        public AprPool Pool
        {
            get {
                return(mArray->pool);
            }
        }        

        public int Count
        {
            get {
                return(mArray->nelts);
            }
        }

        public int AllocatedCount
        {
            get {
                return(mArray->nalloc);
            }
        }
        
        public int ElementSize
        {
            get {
                return(mArray->elt_size);
            }
        }
        
        public IntPtr Data
        {
            get {
                return(new IntPtr(mArray->elts));
            }
        }

        [CLSCompliant(false)]
        public byte *NativeData
        {
            get {
                return(mArray->elts);
            }
        }
        #endregion
  	}
}