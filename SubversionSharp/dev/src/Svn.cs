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
using System.Runtime.InteropServices;
using System.Diagnostics;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{

	///<summary>Embeds all Svn external calls</summary>
    public class Svn
    {
        // no instance constructor !
        private Svn() { }
        
        #region Wrapper around APR Pool
        // Sorry, but for now, we call apr directly and do not support
        // actuel svn wrappers around apr pool
        
        public static AprAllocator AllocatorCreate()
        {
    		AprAllocator allocator = AprAllocator.Create();
    		//SVN_ALLOCATOR_RECOMMENDED_MAX_FREE = 4Mb
    		allocator.MaxFree = (4096*1024);
    		
    		return(allocator);
        }

        public static AprPool PoolCreate()
        {
            return(PoolCreate((AprPool)IntPtr.Zero));
        }

        public static AprPool PoolCreate(AprPool pool)
        {
    		AprAllocator allocator = Svn.AllocatorCreate();
            return(AprPool.Create(pool,allocator));
        }
        
        public static AprPool PoolCreate(AprAllocator allocator)
        {
            return(PoolCreate(IntPtr.Zero, allocator));
        }
                
        public static AprPool PoolCreate(AprPool pool, AprAllocator allocator)
        {
            return(AprPool.Create(pool, allocator));
        }
        #endregion

        #region ClientContext
	    [DllImport("svn_client-1")] static extern
        internal IntPtr svn_client_create_context(out IntPtr ctx, IntPtr pool);
        #endregion
                
        #region Config
	    [DllImport("svn_client-1")] static extern
        internal IntPtr svn_config_ensure(IntPtr config_dir, IntPtr pool);
	    [DllImport("svn_client-1", CharSet=CharSet.Ansi)] static extern
        internal IntPtr svn_config_ensure(string config_dir, IntPtr pool);
        
        [DllImport("svn_client-1")] static extern
        internal IntPtr	svn_config_get_config(out IntPtr cfg_hash, IntPtr config_dir, IntPtr pool);
        [DllImport("svn_client-1", CharSet=CharSet.Ansi)] static extern
        internal IntPtr	svn_config_get_config(out IntPtr cfg_hash, string config_dir, IntPtr pool);
        #endregion
    }
}   