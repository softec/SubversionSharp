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
using System.Collections;
using System.Runtime.InteropServices;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public struct SvnAuthBaton
    {
    	public enum Param {
			DefaultUsername,
			DefaultPassword,
			NonInteractive,
			NoAuthCache,
			SslServerFailures,
			SslServerCertInfo,
			Config,
			ServerGroup,
			ConfigDir
		};

		private const string[] ParamName = new string[] { 
			"svn:auth:username",
			"svn:auth:password",
			"svn:auth:non-interactive",
			"svn:auth:no-auth-cache",
			"svn:auth:ssl:failures",
			"svn:auth:ssl:cert-info",
			"svn:auth:config",
			"svn:auth:server-group",
			"svn:auth:config-dir"
		};

        private IntPtr mAuthBaton;
        private IntPtr[] mParamName;
        private IntPtr mPool;
        internal ArrayList mAuthProviders;

        #region Generic embedding functions of an IntPtr
        private SvnAuthBaton(IntPtr ptr, AprPool pool)
        {
            mAuthBaton = ptr;
            mAuthProviders = null;
            mParamName = null;
            mPool = pool;
        }

        private SvnAuthBaton(ArrayList authProviders, AprPool pool)
        {
        	AprArray authArray = AprArray.Make(pool,authProviders.Count,Marshal.SizeOf(typeof(IntPtr)));
            mAuthProviders = new ArrayList();
            foreach(SvnAuthProviderObject authObj in authProviders) {
            	Marshal.WriteIntPtr(authArray.Push(),authObj);
            	mAuthProviders.Add(authObj.mAuthProvider);
            }
            Svn.svn_auth_open(out mAuthBaton, authArray, pool);
            mParamName = null;
            mPool = pool;
        }

        public bool IsNull
        {
        	get
        	{
            	return( mAuthBaton == IntPtr.Zero || mPool == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mAuthBaton = IntPtr.Zero;
        }

        public static implicit operator IntPtr(SvnAuthBaton authProviderObject)
        {
            return authProviderObject.mAuthBaton;
        }
        
        public static implicit operator SvnAuthBaton(IntPtr ptr)
        {
            return new SvnAuthBaton(ptr);
        }

        public override string ToString()
        {
            return("[svn_auth_baton_t:"+mAuthBaton.ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public SvnAuthBaton Open(ArrayList authProviders, AprPool pool)
        {
        	return(new SvnAuthBaton(authProviders,pool));
        }
        
        public void SetParameter(Param param, IntPtr value)
        {
        	CheckPtr();
        	if( mParamName == null )
        	{
        		mParamName = new IntPtr[ParamName.Length];
        		for(int i=0; i<ParamName.Length; i++)
        			mParamName[i] = IntPtr.Zero;
        	}
        	
	   		if( mParamName[param] == IntPtr.Zero )
	   			mParamName[param] = new AprString(pool, ParamName[param]);
        			
        	svn_auth_set_parameter(mAuthBaton, mParamName[param], value);
        }

        public IntPtr GetParameter(Param param)
        {
        	CheckPtr();
        	if( mParamName == null )
        	{
        		mParamName = new IntPtr[ParamName.Length];
        		for(int i=0; i<ParamName.Length; i++)
        			mParamName[i] = IntPtr.Zero;
        	}
        	
	   		if( mParamName[param] == IntPtr.Zero )
	   			mParamName[param] = new AprString(pool, ParamName[param]);
        			
        	return(svn_auth_get_parameter(mAuthBaton, mParamName[param]));
        }
        #endregion
	}
}