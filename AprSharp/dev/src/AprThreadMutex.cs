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
    public struct AprThreadMutex
    {
        IntPtr mThreadMutex;
        
        ///<remark>Should be synchronized with #define from APR</remarks>
        public enum AprThreadMutexFlags
        {
	       Default,
	       Nested,
	       Unnested
        }

        #region Generic embedding functions of an IntPtr
        private AprThreadMutex(IntPtr ptr)
        {
            mThreadMutex = ptr;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mThreadMutex == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mThreadMutex = IntPtr.Zero;
        }

        public static implicit operator IntPtr(AprThreadMutex threadMutex)
        {
            return threadMutex.mThreadMutex;
        }
        
        public static implicit operator AprThreadMutex(IntPtr ptr)
        {
            return new AprThreadMutex(ptr);
        }

        public override string ToString()
        {
            return("[apr_thread_mutex_t:"+mThreadMutex.ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public static AprThreadMutex Create(AprPool pool)
        {
            return(Create(AprThreadMutexFlags.Default, pool));
        }

        public static AprThreadMutex Create( AprThreadMutexFlags flags,
                                             AprPool pool)
        {
            IntPtr ptr;
            
            Debug.Write(String.Format("apr_thread_mutex_create({0},{1})...",flags,pool));
            int res = Apr.apr_thread_mutex_create(out ptr, (uint)flags, pool); 
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return((AprThreadMutex) ptr);
        }
        
        public void Destroy()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_thread_mutex_destroy({0:X})...",((Int32)mThreadMutex)));
            int res = Apr.apr_thread_mutex_destroy(mThreadMutex);
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine("Done");
            ClearPtr();
        }

        public void Lock()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_thread_mutex_lock({0:X})...",((Int32)mThreadMutex)));
            int res = Apr.apr_thread_mutex_lock(mThreadMutex);
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine("Done");
        }

        public bool TryLock()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_thread_mutex_trylock({0:X})...",((Int32)mThreadMutex)));
            int res = Apr.apr_thread_mutex_trylock(mThreadMutex);
            if(res != 0 ) {
                if(res == 70025 ) {
                    Debug.WriteLine("Fail");
                    return(false);
                }
                throw new AprException(res);
            }
            Debug.WriteLine("Done");
            return(true);
        }
        
        public void Unlock()
        {
            CheckPtr();
            Debug.Write(String.Format("apr_thread_mutex_unlock({0:X})...",((Int32)mThreadMutex)));
            int res = Apr.apr_thread_mutex_unlock(mThreadMutex);
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine("Done");
        }
        #endregion

        #region Wrapper properties
        public AprPool Pool
        {
            get {
                Debug.WriteLine(String.Format("apr_thread_mutex_pool_get({0:X})",((Int32)mThreadMutex)));
                return(Apr.apr_thread_mutex_pool_get(mThreadMutex));
            }
        }
        #endregion
    }
}