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
    public unsafe class SvnStringBuf
    {
        private svn_stringbuf_t *mStringBuf;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_stringbuf_t
		{
  			public IntPtr pool;
			public IntPtr data;
			public uint len;
			public uint blocksize;
		} 

        #region Generic embedding functions of an IntPtr
        private SvnStringBuf(svn_stringbuf_t *ptr)
        {
            mStringBuf = ptr;
        }
        
        public SvnStringBuf(IntPtr ptr)
        {
            mStringBuf = (svn_stringbuf_t *) ptr.ToPointer();
        }
        
        public SvnStringBuf(AprString str, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_create(str, pool)).ToPointer();
		}
        
        public SvnStringBuf(string str, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_create(str, pool)).ToPointer();
        }

        public SvnStringBuf(SvnStringBuf str, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_dup(str, pool)).ToPointer();
        }
        
        public SvnStringBuf(SvnString str, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_create_from_string(str, pool)).ToPointer();
        }
                        
        public SvnStringBuf(AprString str, int size, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_ncreate(str, unchecked((uint)size), 
        													   pool)).ToPointer();
		}
        
        public SvnStringBuf(string str, int size, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_ncreate(str, unchecked((uint)size),
        													   pool)).ToPointer();
        }

		[CLSCompliant(false)]
        public SvnStringBuf(AprString str, uint size, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_ncreate(str, size, pool)).ToPointer();
		}
        
		[CLSCompliant(false)]
        public SvnStringBuf(string str, uint size, AprPool pool)
        {
        	mStringBuf = (svn_stringbuf_t *) (Svn.svn_stringbuf_ncreate(str, size, pool)).ToPointer();
        }

        public bool IsNull
        {
        	get
        	{
            	return( mStringBuf == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mStringBuf = null;
        }

        public static implicit operator IntPtr(SvnStringBuf str)
        {
            return new IntPtr(str.mStringBuf);
        }
        
        public static implicit operator SvnStringBuf(IntPtr ptr)
        {
            return new SvnStringBuf(ptr);
        }

        public override string ToString()
        {
        	if( IsNull )
        		return("[svn_stringbuf:NULL]");
        	else
            	return(Marshal.PtrToStringAnsi(mStringBuf->data));
        }
        #endregion

		#region Method wrappers
		public static SvnStringBuf Create(AprString str, AprPool pool)
		{
			return(new SvnStringBuf(str,pool));
		}
				
		public static SvnStringBuf Create(string str, AprPool pool)
		{
			return(new SvnStringBuf(str,pool));
		}		
		
		public static SvnStringBuf Create(SvnStringBuf str, AprPool pool)
		{
			return(new SvnStringBuf(str,pool));
		}
		
		public static SvnStringBuf Create(SvnString str, AprPool pool)
		{
			return(new SvnStringBuf(str,pool));
		}
		
		public static SvnStringBuf Create(AprString str, int size, AprPool pool)
		{
			return(new SvnStringBuf(str,size,pool));
		}
				
		public static SvnStringBuf Create(string str, int size, AprPool pool)
		{
			return(new SvnStringBuf(str,size,pool));
		}		

		[CLSCompliant(false)]
		public static SvnStringBuf Create(AprString str, uint size, AprPool pool)
		{
			return(new SvnStringBuf(str,size,pool));
		}
				
		[CLSCompliant(false)]
		public static SvnStringBuf Create(string str, uint size, AprPool pool)
		{
			return(new SvnStringBuf(str,size,pool));
		}		
		#endregion		
						
		#region Properties wrappers
		public AprString Data
		{
			get
			{
				CheckPtr();
				return(new AprString(mStringBuf->data));
			}
		}
		 
		public int Len
		{
			get
			{
				CheckPtr();
				return(unchecked((int)mStringBuf->len));
			}
		}
		
		[CLSCompliant(false)]
		public uint NativeLen
		{
			get
			{
				CheckPtr();
				return(mStringBuf->len);
			}
		}
		#endregion
	}
}