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
using System.Text;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public struct SvnPath : IAprUnmanaged
    {
        private IntPtr mPath;

		internal static UTF8Encoding Encoder = new UTF8Encoding(); 

        #region Generic embedding functions of an IntPtr
        public SvnPath(IntPtr ptr)
        {
            mPath = ptr;
        }

        public SvnPath(string str, AprPool pool)
        {
        	byte[] utf8str = Encoder.GetBytes(str);
        	mPath = pool.Alloc(utf8str.Length+1);
            Marshal.Copy(utf8str,0,mPath,utf8str.Length);
            Marshal.WriteByte(mPath,utf8str.Length,0);
        }
        
        public SvnPath(AprString str, AprPool pool)
        {
			SvnError err = Svn.svn_utf_cstring_to_utf8(out mPath, str, pool);
            if(!err.IsNoError)
            	throw new SvnException(err);
        }

        public SvnPath(SvnString str, AprPool pool)
        {
        	IntPtr svnStr;
			SvnError err = Svn.svn_utf_string_to_utf8(out svnStr, str, pool);
            if(!err.IsNoError)
            	throw new SvnException(err);
            mPath = ((SvnString)svnStr).Data;
        }
        
        public SvnPath(SvnStringBuf str, AprPool pool)
        {
        	IntPtr svnStrBuf;
			SvnError err = Svn.svn_utf_stringbuf_to_utf8(out svnStrBuf, str, pool);
            if(!err.IsNoError)
            	throw new SvnException(err);
            mPath = ((SvnStringBuf)svnStrBuf).Data;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mPath == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mPath = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mPath;
        }

		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnPath str)
        {
            return str.mPath;
        }
        
        public static implicit operator SvnPath(IntPtr ptr)
        {
            return new SvnPath(ptr);
        }

        public override string ToString()
        {
        	if( IsNull )
        		return("[svn_path:NULL]");
        	else
        	{
				int len = new AprString(mPath).Length;
				if(len == 0)
					return("");
				byte[] str = new byte[len];         		
        		Marshal.Copy(mPath,str,0,len);
            	return(Encoder.GetString(str));
            }
        }
        #endregion
        
        #region Methods wrappers
        public static SvnPath Duplicate(AprPool pool, string str)
        {
            return(new SvnPath(str, pool));
        }

        public static SvnPath Duplicate(AprPool pool, AprString str)
        {
            return(new SvnPath(str, pool));
        }

        public static SvnPath Duplicate(AprPool pool, SvnString str)
        {
            return(new SvnPath(str, pool));
        }
        
        public static SvnPath Duplicate(AprPool pool, SvnStringBuf str)
        {
            return(new SvnPath(str, pool));
        }
        #endregion
    }
}