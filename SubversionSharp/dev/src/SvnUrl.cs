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