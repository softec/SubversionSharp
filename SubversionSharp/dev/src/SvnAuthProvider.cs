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
    public struct SvnAuthProviderObject
    {
        private IntPtr mAuthProviderObject;
        internal SvnAuthProvider mAuthProvider;

        #region Generic embedding functions of an IntPtr
        private SvnAuthProviderObject(IntPtr ptr)
        {
            mAuthProviderObject = ptr;
            mAuthProvider = null;
        }

        private SvnAuthProviderObject(IntPtr ptr, SvnAuthProvider authProvider)
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
													  int failures, 
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
       		SvnAuthProvider auth = new SvnAuthProvider(promptFunc);
       		Svn.svn_client_get_simple_prompt_provider(out authObj, 
        									(Svn.svn_auth_simple_prompt_func_t) auth.Wrapper, 
        									promptBaton, retryLimit, pool);
       		return(new SvnAuthProviderObject(authObj,auth));
       	}
        											
		public static SvnAuthProviderObject GetPromptProvider(
        										UsernamePrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnAuthProvider auth = new SvnAuthProvider(promptFunc);
       		Svn.svn_client_get_username_prompt_provider(out authObj, 
        									(Svn.svn_auth_username_prompt_func_t) auth.Wrapper, 
        									promptBaton, retryLimit, pool);
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        										
		public static SvnAuthProviderObject GetSimpleProvider(AprPool pool)
		{
       		IntPtr authObj;
       		Svn.svn_client_get_simple_provider(out authObj, pool); 
       		return(new SvnAuthProviderObject(authObj));
		}
		
		public static SvnAuthProviderObject GetUsernameProvider(AprPool pool)
		{
       		IntPtr authObj; 
       		Svn.svn_client_get_username_provider(out authObj, pool); 
       		return(new SvnAuthProviderObject(authObj));
		}
		
        public static SvnAuthProviderObject GetSslServerTrustFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
       		Svn.svn_client_get_ssl_server_trust_file_provider(out authObj, pool); 
			return(new SvnAuthProviderObject(authObj));
        }
		
        public static SvnAuthProviderObject GetSslClientCertFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
       		Svn.svn_client_get_ssl_client_cert_file_provider(out authObj, pool); 
       		return(new SvnAuthProviderObject(authObj));
        }
                        
        public static SvnAuthProviderObject GetSslClientCertPwFileProvider(AprPool pool)
        {
       		IntPtr authObj; 
       		Svn.svn_client_get_ssl_client_cert_pw_file_provider(out authObj, pool); 
       		return(new SvnAuthProviderObject(authObj));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
												SslServerTrustPrompt promptFunc,
        										IntPtr promptBaton, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnAuthProvider auth = new SvnAuthProvider(promptFunc);
       		Svn.svn_client_get_ssl_server_trust_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_server_trust_prompt_func_t) auth.Wrapper, 
        								promptBaton, pool);
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
        										SslClientCertPrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnAuthProvider auth = new SvnAuthProvider(promptFunc);
       		Svn.svn_client_get_ssl_client_cert_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_client_cert_prompt_func_t) auth.Wrapper, 
        								promptBaton, retryLimit, pool);
       		return(new SvnAuthProviderObject(authObj,auth));
        }
        
		public static SvnAuthProviderObject GetPromptProvider(
        										SslClientCertPwPrompt promptFunc,
        										IntPtr promptBaton, int retryLimit, AprPool pool)
        {
       		IntPtr authObj; 
       		SvnAuthProvider auth = new SvnAuthProvider(promptFunc);
       		Svn.svn_client_get_ssl_client_cert_pw_prompt_provider(out authObj, 
        								(Svn.svn_auth_ssl_client_cert_pw_prompt_func_t) auth.Wrapper, 
        								promptBaton, retryLimit, pool);
       		return(new SvnAuthProviderObject(authObj,auth));
        }
		#endregion
	}
	
    internal class SvnAuthProvider
    {
    	object mFunc;
    	object mWrapperFunc;
    	
    	public SvnAuthProvider(SvnAuthProviderObject.SimplePrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_simple_prompt_func_t(SvnAuthSimplePrompt);
    	}
    	
    	public SvnAuthProvider(SvnAuthProviderObject.UsernamePrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_username_prompt_func_t(SvnAuthUsernamePrompt);
    	}

    	public SvnAuthProvider(SvnAuthProviderObject.SslServerTrustPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_server_trust_prompt_func_t(SvnAuthSslServerTrustPrompt);
    	}

    	public SvnAuthProvider(SvnAuthProviderObject.SslClientCertPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_client_cert_prompt_func_t(SvnAuthSslClientCertPrompt);
    	}

    	public SvnAuthProvider(SvnAuthProviderObject.SslClientCertPwPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_client_cert_pw_prompt_func_t(SvnAuthSslClientCertPwPrompt);
    	}
    	
       	public object Wrapper
    	{
    		get
    		{
    			return(mWrapperFunc);
    		}
    	}
    	
        private IntPtr SvnAuthSimplePrompt(out IntPtr cred, IntPtr baton, 
        								   IntPtr realm, IntPtr username, 
        								   int may_save, IntPtr pool)
        {
       		cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SimplePrompt func = 
        							mFunc as SvnAuthProviderObject.SimplePrompt;
        	if( func == null ) {
        		err = SvnError.Create(215000,SvnError.NoError,"SvnNullReferenceException: null reference pointer to callback function");
        		return(err);
        	}
        	try {
        		SvnAuthCredSimple credSimple;
        		err = func(out credSimple, baton,
        		           new AprString(realm), new AprString(username), 
        		           (may_save != 0), new AprPool(pool));
        		cred = credSimple;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
        }

		private  IntPtr SvnAuthUsernamePrompt(out IntPtr cred, IntPtr baton, 
											  IntPtr realm, int may_save, 
											  IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.UsernamePrompt func = 
        							mFunc as SvnAuthProviderObject.UsernamePrompt;
        	if( func == null ) {
        		err = SvnError.Create(215000,SvnError.NoError,"SvnNullReferenceException: null reference pointer to callback function");
        		return(err);
        	}
        	try {
        		SvnAuthCredUsername credUsername;
        		err = func(out credUsername, baton,
        		           new AprString(realm),  
        		           (may_save != 0), new AprPool(pool));
        		cred = credUsername;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
        }
															   
		//[CLSCompliant(false)]
		private  IntPtr SvnAuthSslServerTrustPrompt(out IntPtr cred, IntPtr baton, 
													IntPtr realm, uint failures, 
													IntPtr cert_info, 
													int may_save, IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslServerTrustPrompt func = 
        							mFunc as SvnAuthProviderObject.SslServerTrustPrompt;
        	if( func == null ) {
        		err = SvnError.Create(0,SvnError.NoError,"SvnNullReferenceException: null reference pointer to callback function");
        		return(err);
        	}
        	try {
        		SvnAuthCredSslServerTrust credSslServerTrust;
        		err = func(out credSslServerTrust, baton,
        		           new AprString(realm), unchecked((int)failures),
        		           new SvnAuthSslServerCertInfo(cert_info), 
        		           (may_save != 0), new AprPool(pool));
        		cred = credSslServerTrust;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
        }

		private  IntPtr SvnAuthSslClientCertPrompt(out IntPtr cred, IntPtr baton,
												   IntPtr realm, int may_save,
												   IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslClientCertPrompt func = 
        							mFunc as SvnAuthProviderObject.SslClientCertPrompt;
        	if( func == null ) {
        		err = SvnError.Create(215000,SvnError.NoError,"SvnNullReferenceException: null reference pointer to callback function");
        		return(err);
        	}
        	try {
        		SvnAuthCredSslClientCert credSslClientCert;
        		err = func(out credSslClientCert, baton,
        		           new AprString(realm), 
        		           (may_save != 0), new AprPool(pool));
        		cred = credSslClientCert;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
        }

		private  IntPtr SvnAuthSslClientCertPwPrompt(out IntPtr cred, 
													 IntPtr baton,
													 IntPtr realm, int may_save,
													 IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslClientCertPwPrompt func = 
        							mFunc as SvnAuthProviderObject.SslClientCertPwPrompt;
        	if( func == null ) {
        		err = SvnError.Create(215000,SvnError.NoError,"SvnNullReferenceException: null reference pointer to callback function");
        		return(err);
        	}
        	try {
        		SvnAuthCredSslClientCertPw credSslClientCertPw;
        		err = func(out credSslClientCertPw, baton,
        		           new AprString(realm),  
        		           (may_save != 0), new AprPool(pool));
        		cred = credSslClientCertPw;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
        }
	}
}