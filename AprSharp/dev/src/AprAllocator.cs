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

	///<summary>Embeds an opaque apr_allocator</summary>
    public struct AprAllocator : IAprUnmanaged
    {
        private IntPtr mAllocator;

        #region Generic embedding functions of an IntPtr
        public AprAllocator(IntPtr ptr)
        {
            mAllocator = ptr;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mAllocator == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mAllocator = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mAllocator;
        }

		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(AprAllocator allocator)
        {
            return allocator.mAllocator;
        }
        
        public static implicit operator AprAllocator(IntPtr ptr)
        {
            return new AprAllocator(ptr);
        }
        
        public override string ToString()
        {
            return("[apr_allocator_t:"+mAllocator.ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public static AprAllocator Create()
        {
            IntPtr ptr;

            Debug.Write("apr_allocator_create...");
            int res = Apr.apr_allocator_create(out ptr);
            
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine(String.Format("Done({0:X})",(Int32)ptr));

            return((AprAllocator) ptr);
        }
        
        public void Destroy()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_allocator_destroy({0:X})...",(Int32)mAllocator));
            Apr.apr_allocator_destroy(mAllocator);
            Debug.WriteLine("Done");
            ClearPtr();
        }
        
        public AprMemNode Alloc(int size)
        {
            return(Alloc(unchecked((uint)size)));
        }

        [CLSCompliant(false)]
        public AprMemNode Alloc(uint size)
        {
            CheckPtr();
            Debug.WriteLine(String.Format("apr_allocator_alloc({0:X},{1})",mAllocator.ToInt32(),size));
            return(new AprMemNode(Apr.apr_allocator_alloc(mAllocator,size)));
        }

        public void Free(AprMemNode memnode)
        {
            CheckPtr();
            Debug.Write(String.Format("apr_allocator_free({0:X},{1:X})...",mAllocator.ToInt32(),(Int32)((IntPtr)memnode)));
            Apr.apr_allocator_free(mAllocator,memnode);
            Debug.WriteLine("Done");
        }
        #endregion

        #region Wrapper Properties
        public AprPool Owner
        {
            get {
                Debug.WriteLine(String.Format("apr_allocator_owner_get({0:X})",mAllocator.ToInt32()));
                return(Apr.apr_allocator_owner_get(mAllocator));
            }
            
            set {
                Debug.Write(String.Format("apr_allocator_owner_set({0:X},{1})...",mAllocator.ToInt32(),value));
                Apr.apr_allocator_owner_set(mAllocator,value);
                Debug.WriteLine("Done");
            }
        }        

        public AprThreadMutex Mutex
        {
            get {
                Debug.WriteLine(String.Format("apr_allocator_mutex_get({0:X})",mAllocator.ToInt32()));
                return(Apr.apr_allocator_mutex_get(mAllocator));
            }
            
            set {
                Debug.Write(String.Format("apr_allocator_mutex_set({0:X},{1})...",mAllocator.ToInt32(),value));
                Apr.apr_allocator_mutex_set(mAllocator,value);
                Debug.WriteLine("Done");
            }
        }  
        
        public int MaxFree
        {
            set 
            {
                NativeMaxFree = unchecked((uint)value);
            }
        }

   	    [CLSCompliant(false)]
        public uint NativeMaxFree
        {
            set 
            {
                Apr.apr_allocator_max_free_set(mAllocator, value);
            }
        }
        #endregion       
    }
}