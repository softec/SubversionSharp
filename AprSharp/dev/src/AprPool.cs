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

namespace Softec.AprSharp
{

    public struct AprPool
    {
        IntPtr mPool;

        #region Generic embedding functions of an IntPtr
        public AprPool(IntPtr ptr)
        {
            mPool = ptr;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mPool == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mPool = IntPtr.Zero;
        }

        public static implicit operator IntPtr(AprPool pool)
        {
            return pool.mPool;
        }
        
        public static implicit operator AprPool(IntPtr ptr)
        {
            return new AprPool(ptr);
        }

        public override string ToString()
        {
            return("[apr_pool_t:"+mPool.ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public static AprPool Create()
        {
            return(Create(IntPtr.Zero, IntPtr.Zero));
        }

        public static AprPool Create(AprPool pool)
        {
            return(Create(pool, IntPtr.Zero));
        }

        public static AprPool Create(AprAllocator allocator)
        {
            return(Create(IntPtr.Zero, allocator));
        }
                
        public static AprPool Create(AprPool pool, AprAllocator allocator)
        {
            IntPtr ptr;
            
            Debug.Write(String.Format("apr_pool_create_ex({0},{1})...",pool,allocator));
            int res = Apr.apr_pool_create_ex(out ptr, pool, 
                                             IntPtr.Zero, allocator);
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }
        
        public void Destroy()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_pool_destroy({0:X})...",this));
            Apr.apr_pool_destroy(mPool);
            Debug.WriteLine("Done");
            ClearPtr();
        }
        
        public void Clear()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_pool_clear({0:X})...",this));
            Apr.apr_pool_clear(mPool);
            Debug.WriteLine("Done");
        }

        public unsafe IntPtr Alloc(int size)
        {
            return((IntPtr)Alloc(unchecked((uint)size)));
        }

 	    [CLSCompliant(false)]
        public unsafe byte *Alloc(uint size)
        {
            CheckPtr();
            Debug.WriteLine(String.Format("apr_palloc({0:X})",this));
            return((byte *)Apr.apr_palloc(mPool, size));
        }

        public unsafe IntPtr CAlloc(int size)
        {
            return((IntPtr)CAlloc(unchecked((uint)size)));
        }

 	    [CLSCompliant(false)]
        public unsafe byte *CAlloc(uint size)
        {
            CheckPtr();
            Debug.WriteLine(String.Format("apr_pcalloc({0:X})",this));
            return((byte *)Apr.apr_pcalloc(mPool, size));
        }

        public bool IsAncestor(AprPool pool)
        {
            CheckPtr();
            Debug.WriteLine(String.Format("apr_pool_is_ancestor({0:X},{1:X})",this,pool));
            return(!(Apr.apr_pool_is_ancestor(mPool,pool) == 0));
        }
        #endregion

        #region Wrapper properties
        public AprAllocator Allocator
        {
            get {
                Debug.WriteLine(String.Format("apr_pool_allocator_get({0:X})",this));
                return(Apr.apr_pool_allocator_get(mPool));
            }
        }
        
        public AprPool Parent
        {
            get {
                Debug.WriteLine(String.Format("apr_pool_parent_get({0:X})",this));
                return(Apr.apr_pool_parent_get(mPool));
            }
        }
        #endregion
    }
}