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
    public unsafe class SvnString : IAprUnmanaged
    {
        private svn_string_t *mString;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_string_t
		{
			public IntPtr data;
			public uint len;
   		} 

        #region Generic embedding functions of an IntPtr
        private SvnString(svn_string_t *ptr)
        {
            mString = ptr;
        }
        
        public SvnString(IntPtr ptr)
        {
            mString = (svn_string_t *) ptr.ToPointer();
        }
        
        public SvnString(AprString str, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_create(str, pool)).ToPointer();
		}
        
        public SvnString(string str, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_create(str, pool)).ToPointer();
        }
        
        public SvnString(SvnString str, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_dup(str, pool)).ToPointer();
        }
        
        public SvnString(SvnStringBuf str, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_create_from_buf(str, pool)).ToPointer();
        }
        
        public SvnString(AprString str, int size, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_ncreate(str, unchecked((uint)size), 
        													   pool)).ToPointer();
		}
        
        public SvnString(string str, int size, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_ncreate(str, unchecked((uint)size),
        													   pool)).ToPointer();
        }

		[CLSCompliant(false)]
        public SvnString(AprString str, uint size, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_ncreate(str, size, pool)).ToPointer();
		}
        
		[CLSCompliant(false)]
        public SvnString(string str, uint size, AprPool pool)
        {
        	mString = (svn_string_t *) (Svn.svn_string_ncreate(str, size, pool)).ToPointer();
        }

        public bool IsNull
        {
        	get
        	{
            	return( mString == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mString = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mString);
        }
        
        public static implicit operator IntPtr(SvnString str)
        {
            return new IntPtr(str.mString);
        }
        
        public static implicit operator SvnString(IntPtr ptr)
        {
            return new SvnString(ptr);
        }

        public override string ToString()
        {
        	if( IsNull )
        		return("[svn_string:NULL]");
        	else
            	return(Marshal.PtrToStringAnsi(mString->data));
        }
        #endregion

		#region Method wrappers
		public static SvnString Create(AprString str, AprPool pool)
		{
			return(new SvnString(str,pool));
		}
				
		public static SvnString Create(string str, AprPool pool)
		{
			return(new SvnString(str,pool));
		}		
		
		public static SvnString Create(SvnString str, AprPool pool)
		{
			return(new SvnString(str,pool));
		}
		
		public static SvnString Create(SvnStringBuf str, AprPool pool)
		{
			return(new SvnString(str,pool));
		}
		
		public static SvnString Create(AprString str, int size, AprPool pool)
		{
			return(new SvnString(str,size,pool));
		}
				
		public static SvnString Create(string str, int size, AprPool pool)
		{
			return(new SvnString(str,size,pool));
		}		

		[CLSCompliant(false)]
		public static SvnString Create(AprString str, uint size, AprPool pool)
		{
			return(new SvnString(str,size,pool));
		}
				
		[CLSCompliant(false)]
		public static SvnString Create(string str, uint size, AprPool pool)
		{
			return(new SvnString(str,size,pool));
		}		
		#endregion		
						
		#region Properties wrappers
		public AprString Data
		{
			get
			{
				CheckPtr();
				return(new AprString(mString->data));
			}
		}
		 
		public int Len
		{
			get
			{
				CheckPtr();
				return(unchecked((int)mString->len));
			}
		}
		
		[CLSCompliant(false)]
		public uint NativeLen
		{
			get
			{
				CheckPtr();
				return(mString->len);
			}
		}
		#endregion
	}
}
