//  SubversionSharp, a wrapper library around the Subversion client API
#region Copyright (C) 2004 SOFTEC sa.
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
#endregion
//
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public struct SvnUrl : IAprUnmanaged
    {
        private IntPtr mUrl;

       #region Generic embedding functions of an IntPtr
        public SvnUrl(IntPtr ptr)
        {
            mUrl = ptr;
        }

        public SvnUrl(string str, AprPool pool)
        {
        	Uri uri;
        	try {
        		uri = new Uri(str);
        	}
        	catch(System.UriFormatException e) {
        		throw new SvnException("Invalid URL",e);
        	}
        	mUrl = new AprString(uri.AbsoluteUri, pool);
        }
        
        public SvnUrl(Uri uri, AprPool pool)
        {
        	mUrl = new AprString(uri.AbsoluteUri, pool);
        }
        
        public SvnUrl(AprString str, AprPool pool)
        {
			mUrl = Svn.svn_path_uri_encode(new SvnPath(str,pool), pool);
        }

        public SvnUrl(SvnString str, AprPool pool)
        {
			mUrl = Svn.svn_path_uri_encode(new SvnPath(str,pool), pool);
        }
        
        public SvnUrl(SvnStringBuf str, AprPool pool)
        {
			mUrl = Svn.svn_path_uri_encode(new SvnPath(str,pool), pool);
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mUrl == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mUrl = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mUrl;
        }

		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnUrl str)
        {
            return str.mUrl;
        }
        
        public static implicit operator SvnUrl(IntPtr ptr)
        {
            return new SvnUrl(ptr);
        }

        public override string ToString()
        {
        	if( IsNull )
        		return("[svn_url:NULL]");
        	else
        	{
				return(new Uri(new AprString(mUrl).ToString()).ToString());
            }
        }
        #endregion
        
        #region Methods wrappers
        public static SvnUrl Duplicate(AprPool pool, string str)
        {
            return(new SvnUrl(str, pool));
        }

        public static SvnUrl Duplicate(AprPool pool, AprString str)
        {
            return(new SvnUrl(str, pool));
        }
        
        public static SvnUrl Duplicate(AprPool pool, SvnString str)
        {
            return(new SvnUrl(str, pool));
        }
        
        public static SvnUrl Duplicate(AprPool pool, SvnStringBuf str)
        {
            return(new SvnUrl(str, pool));
        }
        #endregion
    }
}