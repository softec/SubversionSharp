//  AprSharp, a wrapper library around the Apache Runtime Library
//  Copyright (C) 2004 SOFTEC sa.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//  Sources, support options and lastest version of the complete library
//  is available from:
//		http://www.softec.st/AprSharp
//		Support@softec.st
//
//  Initial authors : 
//		Denis Gervalle
//		Olivier Desaive
//
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Softec.AprSharp
{

	///<summary>Embeds all APR external calls</summary>
	///<remark>Take care to call apr_initialize</remark>
    public sealed class Apr
    {
        // no instance constructor !
        private Apr() { }
        
        static Apr()
        {
            int apr_status;
            Debug.Write("apr_initialize...");
            apr_status = apr_initialize();
            Debug.WriteLine("Done");
            if( apr_status != 0 )
                throw new AprException(apr_status);
                
            //FIXME: Shoud shedule a call to apr_terminate 
            //       at process exit        
        }
        
        public static void Terminate()
        {
            Debug.Write("apr_terminate...");
        	apr_terminate();
            Debug.WriteLine("Done");
        }
         
        #region Initialize / Terminate
	    [DllImport("apr-0")]
        private static extern int apr_initialize( );
        
	    [DllImport("apr-0")]
        private static extern void apr_terminate( );
        #endregion
        
        #region Error
	    [DllImport("apr-0")] // CLSCompliant(false)
        private static extern void apr_strerror(int apr_status,
                                                StringBuilder buf,
                                                uint size);

        public static string StrError(int apr_status)
        {       
            StringBuilder buf = new StringBuilder (1024);
            Apr.apr_strerror(apr_status, buf, (uint)buf.Capacity);
            return(buf.ToString());
        }
        #endregion

        #region Allocator
	    [DllImport("apr-0")] static extern 
        internal int apr_allocator_create(out IntPtr allocator);
        
	    [DllImport("apr-0")] static extern
        internal void apr_allocator_destroy(IntPtr allocator);
        
        [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal IntPtr apr_allocator_alloc(IntPtr allocator, 
                                            uint size);
        
        [DllImport("apr-0")] static extern
        internal void apr_allocator_free(IntPtr allocator,
                                         IntPtr memnode);
        
	    [DllImport("apr-0")] static extern
        internal void apr_allocator_owner_set(IntPtr allocator, 
                                              IntPtr pool);

	    [DllImport("apr-0")] static extern
        internal IntPtr apr_allocator_owner_get(IntPtr allocator);
        
	    [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal void apr_allocator_max_free_set(IntPtr allocator,
                                                 uint size);

	    [DllImport("apr-0")] static extern
        internal void apr_allocator_mutex_set(IntPtr allocator,
                                              IntPtr mutex);

	    [DllImport("apr-0")] static extern
        internal IntPtr apr_allocator_mutex_get(IntPtr allocator);
        #endregion

        #region ThreadMutex
	    [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal int apr_thread_mutex_create(out IntPtr mutext,
                                             uint flags,
                                             IntPtr pool);
                                             
	    [DllImport("apr-0")] static extern
        internal int apr_thread_mutex_lock(IntPtr mutex);
        
	    [DllImport("apr-0")] static extern
        internal int apr_thread_mutex_trylock(IntPtr mutex);
        
	    [DllImport("apr-0")] static extern
        internal int apr_thread_mutex_unlock(IntPtr mutex);
        
	    [DllImport("apr-0")] static extern
        internal int apr_thread_mutex_destroy(IntPtr mutex);
        
	    [DllImport("apr-0")] static extern
        internal IntPtr apr_thread_mutex_pool_get(IntPtr mutex);        
        #endregion
        
        #region Pool
	    [DllImport("apr-0")] static extern
        internal int apr_pool_create_ex(out IntPtr newpool, IntPtr parent,
                                        IntPtr abort_fn, IntPtr allocator);

	    [DllImport("apr-0")] static extern
        internal void apr_pool_destroy(IntPtr p);
        
	    [DllImport("apr-0")] static extern
        internal IntPtr apr_pool_allocator_get(IntPtr pool);
        
	    [DllImport("apr-0")] static extern
        internal void apr_pool_clear(IntPtr pool);
        
	    [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal IntPtr apr_palloc(IntPtr pool, uint size);
        
	    [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal IntPtr apr_pcalloc(IntPtr pool, uint size);
        
	    [DllImport("apr-0")] static extern
        internal IntPtr	apr_pool_parent_get(IntPtr pool);
        
	    [DllImport("apr-0")] static extern
        internal int apr_pool_is_ancestor(IntPtr a, IntPtr b);
/*        
	    [DllImport("apr-0")] static extern
        internal void apr_pool_tag(IntPtr pool, IntPtr tag);
        
        internal delegate int AprPoolCleanUpDelegate(IntPtr data);
        
	    [DllImport("apr-0")] static extern
        internal int apr_pool_userdata_set(IntPtr data, 
                                           IntPtr key, 
                                           AprPoolCleanUpDelegate cleanup,
                                           IntPtr pool);
                                           
	    [DllImport("apr-0")] static extern
        internal int apr_pool_userdata_setn(IntPtr data, 
                                           IntPtr key, 
                                           AprPoolCleanUpDelegate cleanup,
                                           IntPtr pool);
                                           
	    [DllImport("apr-0")] static extern
        internal int apr_pool_userdata_get(out IntPtr data, 
                                           IntPtr key,
                                           IntPtr pool);
        
	    [DllImport("apr-0")] static extern
        internal void apr_pool_cleanup_register(IntPtr pool, 
                                                IntPtr data, 
                                                AprPoolCleanUpDelegate plaincleanup,
                                                AprPoolCleanUpDelegate childcleanup);

	    [DllImport("apr-0")] static extern
        internal void apr_pool_cleanup_kill(IntPtr pool,
                                            IntPtr data,
                                            AprPoolCleanUpDelegate cleanup);
                                            
	    [DllImport("apr-0")] static extern
        internal void apr_pool_child_cleanup_set(IntPtr pool, 
                                                 IntPtr data, 
                                                AprPoolCleanUpDelegate plaincleanup,
                                                AprPoolCleanUpDelegate childcleanup);
                                                
	    [DllImport("apr-0")] static extern
        internal int apr_pool_cleanup_run(IntPtr pool,
                                          IntPtr data,
                                          AprPoolCleanUpDelegate cleanup);

	    [DllImport("apr-0")] static extern
        internal int apr_pool_cleanup_null(IntPtr data);
        
	    [DllImport("apr-0")] static extern
        internal void apr_pool_cleanup_for_exec();
*/
        #endregion
        
        #region AprTime
	    [DllImport("apr-0")] static extern
        internal long apr_time_now();
        
        [DllImport("apr-0")] static extern
        internal int apr_ctime(StringBuilder date_str, long input);

        [DllImport("apr-0")] static extern
        internal int apr_rfc822_date(StringBuilder date_str, long input);
        #endregion
        
        #region AprTimeExp
	    [DllImport("apr-0")] static extern
        internal int apr_time_exp_gmt(IntPtr result, long input);
        
	    [DllImport("apr-0")] static extern
        internal int apr_time_exp_gmt_get(out long result, IntPtr input);

	    [DllImport("apr-0")] static extern
        internal int apr_time_exp_get(out long result, IntPtr input);
        
        [DllImport("apr-0")] static extern
        internal int apr_time_exp_lt(IntPtr result,	long input);
        
        [DllImport("apr-0")] static extern
        internal int apr_time_exp_tz(IntPtr result, long input, int offset);
        
        [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal int apr_strftime(StringBuilder s, out uint retsize,
                                  uint maxsize, string Format, IntPtr input);
        #endregion
        
        #region AprString
        [DllImport("apr-0")] static extern
        internal IntPtr apr_pstrdup(IntPtr pool, IntPtr str);
        [DllImport("apr-0", CharSet=CharSet.Ansi)] static extern
        internal IntPtr apr_pstrdup(IntPtr pool, string str);
                
        [DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal IntPtr apr_pstrndup(IntPtr pool, IntPtr str, uint size);
        [DllImport("apr-0", CharSet=CharSet.Ansi)] /* CLSCompliant(false) */ static extern
        internal IntPtr apr_pstrndup(IntPtr pool, string str, uint size);
        #endregion

        #region AprHash
        [DllImport("apr-0")] static extern
        internal IntPtr apr_hash_make(IntPtr pool);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_copy (IntPtr pool, IntPtr h);

		[DllImport("apr-0")] static extern
        internal void apr_hash_set (IntPtr ht, IntPtr key, int klen, IntPtr val);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_get (IntPtr ht, IntPtr key, int klen);

		[DllImport("apr-0")] /* CLSCompliant(false) */ static extern
        internal uint apr_hash_count(IntPtr ht);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_overlay (IntPtr p, IntPtr overlayh, IntPtr baseh);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_pool_get(IntPtr thehash);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_first (IntPtr p, IntPtr ht);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_hash_next(IntPtr hi);

		[DllImport("apr-0")] static extern
        internal void apr_hash_this(IntPtr hi, out IntPtr key, out int klen, out IntPtr val);
        #endregion
        
        #region AprHash
        [DllImport("apr-0")] static extern
        internal IntPtr apr_array_make(IntPtr pool, int elts, int elt_size);

        [DllImport("apr-0")] static extern
        internal IntPtr apr_array_copy(IntPtr pool, IntPtr arr);
        
        [DllImport("apr-0")] static extern
        internal IntPtr apr_array_copy_hdr(IntPtr pool, IntPtr arr);

		[DllImport("apr-0")] static extern
        internal IntPtr apr_array_append (IntPtr pool, IntPtr first, IntPtr second);

		[DllImport("apr-0")] static extern
        internal void apr_array_cat (IntPtr dst, IntPtr src);

		[DllImport("apr-0", CharSet=CharSet.Ansi)] static extern
        internal IntPtr apr_array_pstrcat (IntPtr pool, IntPtr arr, char sep);

        [DllImport("apr-0")] static extern
        internal IntPtr apr_array_push(IntPtr arr);

        [DllImport("apr-0")] static extern
        internal IntPtr apr_array_pop(IntPtr arr);

        [DllImport("apr-0")] static extern
        internal bool apr_is_empty_array(IntPtr arr);
        #endregion
        
        #region AprFile
        [DllImport("apr-0", CharSet=CharSet.Ansi)] static extern
        internal int apr_file_open(out IntPtr new_file, string fname,
								   int flag, int perm, IntPtr pool);

		[DllImport("apr-0")] static extern
        internal int apr_file_close(IntPtr file);
        
        [DllImport("apr-0")] static extern
        internal int apr_file_open_stdin(out IntPtr thefile, IntPtr pool);
        
        [DllImport("apr-0")] static extern
        internal int apr_file_open_stdout(out IntPtr thefile, IntPtr pool);
        
        [DllImport("apr-0")] static extern
        internal int apr_file_open_stderr(out IntPtr thefile, IntPtr pool);
        #endregion
    }
}   