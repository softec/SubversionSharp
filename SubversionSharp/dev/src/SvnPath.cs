//  SubversionSharp, a wrapper library around the Subversion client API
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
//		http://www.softec.st/SubversionSharp
//		Support@softec.st
//
//  Initial authors : 
//		Denis Gervalle
//		Olivier Desaive
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