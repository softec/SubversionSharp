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
        
        #region SvnError
        [DllImport("svn_client-1", CharSet=CharSet.Ansi)] static extern
        internal IntPtr svn_error_create(int apr_err, IntPtr child, string message);
        
        [DllImport("svn_client-1", CharSet=CharSet.Ansi)] static extern
        internal void svn_error_clear(IntPtr error);   
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
        
        #region AuthProvider
        public delegate IntPtr svn_auth_simple_prompt_func_t(out IntPtr cred, IntPtr baton, 
        													 IntPtr realm, IntPtr username, 
        													 int may_save, IntPtr pool);

		public delegate IntPtr svn_auth_username_prompt_func_t(out IntPtr cred, IntPtr baton, 
															   IntPtr realm, int may_save, 
															   IntPtr pool);
															   
		[CLSCompliant(false)]
		public delegate IntPtr svn_auth_ssl_server_trust_prompt_func_t(out IntPtr cred, IntPtr baton, 
																	   IntPtr realm, uint failures, 
																	   IntPtr cert_info, 
																	   int may_save, IntPtr pool);

		public delegate IntPtr svn_auth_ssl_client_cert_prompt_func_t(out IntPtr cred, IntPtr baton,
																	  IntPtr realm, int may_save,
																	  IntPtr pool);

		public delegate IntPtr svn_auth_ssl_client_cert_pw_prompt_func_t(out IntPtr cred, 
																		 IntPtr baton,
																  		 IntPtr realm, int may_save,
																  		 IntPtr pool);
																  		 
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_simple_prompt_provider(out IntPtr provider, 
        											svn_auth_simple_prompt_func_t prompt_func, 
        											IntPtr prompt_baton, int retry_limit, IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_username_prompt_provider(out IntPtr provider,
        										svn_auth_username_prompt_func_t prompt_func,
        										IntPtr prompt_baton, int retry_limit, IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_simple_provider(out IntPtr provider,
        										     IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_username_provider(out IntPtr provider,
        											   IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_server_trust_file_provider(out IntPtr provider, 
        															IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_client_cert_file_provider(out IntPtr provider,
        														   IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_client_cert_pw_file_provider(out IntPtr provider, 
        															  IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_server_trust_prompt_provider(out IntPtr provider, 
        										svn_auth_ssl_server_trust_prompt_func_t prompt_func,
        										IntPtr prompt_baton, IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_client_cert_prompt_provider(out IntPtr provider,
        										svn_auth_ssl_client_cert_prompt_func_t prompt_func,
        										IntPtr prompt_baton, int retry_limit, IntPtr pool);
		[DllImport("svn_client-1")] static extern
        internal void svn_client_get_ssl_client_cert_pw_prompt_provider(out IntPtr provider,
        										svn_auth_ssl_client_cert_pw_prompt_func_t prompt_func,
        										IntPtr prompt_baton, int retry_limit, IntPtr pool);
        #endregion
        
        #region AuthBaton
        [DllImport("svn_client-1")] static extern
        internal void svn_auth_open(out IntPtr auth_baton, IntPtr providers, IntPtr pool);
        
        [DllImport("svn_client-1")] static extern
        internal void svn_auth_set_parameter(IntPtr auth_baton, IntPtr name, IntPtr value);
        
        [DllImport("svn_client-1")] static extern
        internal IntPtr svn_auth_get_parameter(IntPtr auth_baton, IntPtr name);
		#endregion
    }
}   