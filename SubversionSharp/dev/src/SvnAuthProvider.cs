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

namespace Softec.SubversionSharp
{
    public struct SvnAuthProviderObject : IAprUnmanaged
    {
        private IntPtr mAuthProviderObject;
        internal SvnDelegate mAuthProvider;

        #region Generic embedding functions of an IntPtr
        private SvnAuthProviderObject(IntPtr ptr)
        {
            mAuthProviderObject = ptr;
            mAuthProvider = null;
        }

        private SvnAuthProviderObject(IntPtr ptr, SvnDelegate authProvider)
        {
            mAuthProviderObject = ptr;
            mAuthProvider = authProvider;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mAuthProviderObject == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mAuthProviderObject = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mAuthProviderObject;
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnAuthProviderObject authProviderObject)
        {
            return authProviderObject.mAuthProviderObject;
        }
        
        public static implicit operator SvnAuthProviderObject(IntPtr ptr)
        {
            return new SvnAuthProviderObject(ptr);
        }

        public override string ToString()
        {
            return("[svn_auth_provider_object_t:"+mAuthProviderObject.ToInt32().ToString("X")+"]");
        }
        #endregion

		#region Wrapper delegates
        public delegate SvnError SimplePrompt(out SvnAuthCredSimple cred, IntPtr baton, 
        				 		   			  AprString realm, AprString username, 
        								   	  bool maySave, AprPool pool);

		public delegate SvnError UsernamePrompt(out SvnAuthCredUsername cred, IntPtr baton, 
												AprString realm, bool maySave, 
											    AprPool pool);
															   
		public delegate SvnError SslServerTrustPrompt(out SvnAuthCredSslServerTrust cred, 
													  IntPtr baton, AprString realm, 
													  SvnAuthCredSslServerTrust.CertFailures failures, 
													  SvnAuthSslServerCertInfo certInfo, 
													  bool maySave, IntPtr pool);

		public delegate SvnError SslClientCertPrompt(out SvnAuthCredSslClientCert cred, 
													 IntPtr baton, AprString realm,
													 bool maySave, IntPtr pool);

		public delegate SvnError SslClientCertPwPrompt(out SvnAuthCredSslClientCertPw cred, 
													   IntPtr baton, AprString realm, 
													   bool maySave, IntPtr pool);
        #endregion

		#region Wrapper client method
        public static SvnAuthProviderObject GetPromptProvider( 
        											SimplePrompt promptFunc, 
        											IntPtr promptBaton, int retryLimit, AprPool pool)
       	{
       		IntPtr authObj;
       		SvnDelegate auth = new SvnDelegate(promptFunc);
            Debug.Write(String.Format("svn_client_get_simple_prompt_provider([callback:{0}],{1:X},{2},{3})...",promptFunc.Method.Name,promptBaton.ToInt32(),retryLimit,pool));
       		Svn.svn_client_get_simple_prompt_provider(out authObj, 
        									(Svn.svn_auth_simple_prompt_func_t) auth.Wrapper, 
        									promptBaton, retryLimit, pool);
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj,auth));
       	}
        											
		public static SvnAuthProviderObject GetPromptProvider(
        										UsernamePrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnDelegate auth = new SvnDelegate(promptFunc);
            Debug.Write(String.Format("svn_client_get_username_prompt_provider([callback:{0}],{1:X},{2},{3})...",promptFunc.Method.Name,promptBaton.ToInt32(),retryLimit,pool));
       		Svn.svn_client_get_username_prompt_provider(out authObj, 
        									(Svn.svn_auth_username_prompt_func_t) auth.Wrapper, 
        									promptBaton, retryLimit, pool);
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        										
		public static SvnAuthProviderObject GetSimpleProvider(AprPool pool)
		{
       		IntPtr authObj;
            Debug.Write(String.Format("svn_client_get_simple_provider({0:X})",pool));
       		Svn.svn_client_get_simple_provider(out authObj, pool); 
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj));
		}
		
		public static SvnAuthProviderObject GetUsernameProvider(AprPool pool)
		{
       		IntPtr authObj; 
            Debug.Write(String.Format("svn_client_get_username_provider({0:X})",pool));
       		Svn.svn_client_get_username_provider(out authObj, pool); 
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj));
		}
		
        public static SvnAuthProviderObject GetSslServerTrustFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
            Debug.Write(String.Format("svn_client_get_ssl_server_trust_file_provider({0:X})",pool));
       		Svn.svn_client_get_ssl_server_trust_file_provider(out authObj, pool); 
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
			return(new SvnAuthProviderObject(authObj));
        }
		
        public static SvnAuthProviderObject GetSslClientCertFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
            Debug.Write(String.Format("svn_client_get_ssl_client_cert_file_provider({0:X})",pool));
       		Svn.svn_client_get_ssl_client_cert_file_provider(out authObj, pool); 
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj));
        }
                        
        public static SvnAuthProviderObject GetSslClientCertPwFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
            Debug.Write(String.Format("svn_client_get_ssl_client_cert_pw_file_provider({0:X})",pool));
       		Svn.svn_client_get_ssl_client_cert_pw_file_provider(out authObj, pool); 
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
												SslServerTrustPrompt promptFunc,
        										IntPtr promptBaton, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnDelegate auth = new SvnDelegate(promptFunc);
            Debug.Write(String.Format("svn_client_get_ssl_server_trust_prompt_provider([callback:{0}],{1:X},{2})...",promptFunc.Method.Name,promptBaton.ToInt32(),pool));
       		Svn.svn_client_get_ssl_server_trust_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_server_trust_prompt_func_t) auth.Wrapper, 
        								promptBaton, pool);
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
        										SslClientCertPrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnDelegate auth = new SvnDelegate(promptFunc);
            Debug.Write(String.Format("svn_client_get_ssl_client_cert_prompt_provider([callback:{0}],{1},{2},{3})...",auth.Wrapper,promptBaton,retryLimit,pool));
       		Svn.svn_client_get_ssl_client_cert_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_client_cert_prompt_func_t) auth.Wrapper, 
        								promptBaton, retryLimit, pool);
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
        										SslClientCertPwPrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnDelegate auth = new SvnDelegate(promptFunc);
            Debug.Write(String.Format("svn_client_get_ssl_client_cert_pw_prompt_provider([callback:{0}],{1:X},{2},{3})...",promptFunc.Method.Name,promptBaton.ToInt32(),retryLimit,pool));
       		Svn.svn_client_get_ssl_client_cert_pw_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_client_cert_pw_prompt_func_t) auth.Wrapper, 
        								promptBaton, retryLimit, pool);
            Debug.WriteLine(String.Format("Done({0:X})",authObj.ToInt32()));
       		return(new SvnAuthProviderObject(authObj,auth));
        }
		#endregion
	}
}