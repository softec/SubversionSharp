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
using Softec.AprSharp;
using System.Runtime.InteropServices;

namespace Softec.SubversionSharp
{

    public unsafe struct SvnAuthSslServerCertInfo : IAprUnmanaged
    {
        private svn_auth_ssl_server_cert_info *mSslServerCertInfo;

        [StructLayout( LayoutKind.Sequential )]
        private struct svn_auth_ssl_server_cert_info
        {  
			public IntPtr hostname;
			public IntPtr fingerprint;
			public IntPtr valid_from;
			public IntPtr valid_until;
			public IntPtr issuer_dname;
			public IntPtr ascii_cert;
        }

        #region Generic embedding functions of an IntPtr
        private SvnAuthSslServerCertInfo(svn_auth_ssl_server_cert_info *ptr)
        {
            mSslServerCertInfo = ptr;
        }
        
        public SvnAuthSslServerCertInfo(IntPtr ptr)
        {
            mSslServerCertInfo = (svn_auth_ssl_server_cert_info *) ptr.ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mSslServerCertInfo == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mSslServerCertInfo = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mSslServerCertInfo);
        }
        
        public static implicit operator IntPtr(SvnAuthSslServerCertInfo clientContext)
        {
            return new IntPtr(clientContext.mSslServerCertInfo);
        }
        
        public static implicit operator SvnAuthSslServerCertInfo(IntPtr ptr)
        {
            return new SvnAuthSslServerCertInfo(ptr);
        }

        public override string ToString()
        {
            return("[svn_client_context_t:"+(new IntPtr(mSslServerCertInfo)).ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper Properties
        public AprString Hostname
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->hostname);
			}
		}
        public AprString Fingerprint
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->fingerprint);
			}
		}
        public AprString ValidFrom
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->valid_from);
			}
		}
        public AprString ValidUntil
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->valid_until);
			}
		}
        public AprString IssuerDName
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->issuer_dname);
			}
		}
        public AprString AsciiCert
        {
			get
			{
				CheckPtr();
				return(mSslServerCertInfo->ascii_cert);
			}
		}
        #endregion
    }
}