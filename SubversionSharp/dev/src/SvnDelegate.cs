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
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public class SvnDelegate
    {
    	Delegate mFunc;
    	Delegate mWrapperFunc;
    	
       	internal Delegate Wrapper
    	{
    		get
    		{
    			return(mWrapperFunc);
    		}
    	}
    	
    	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    	private struct DelegateStruct {
			public Delegate mFunc;
    	}

       	internal void MarshalWrapperToPtr(IntPtr ptr)
    	{
			DelegateStruct func = new DelegateStruct();
			func.mFunc = mWrapperFunc;
			Marshal.StructureToPtr(func,ptr,false);
    	}
    	
       	internal unsafe IntPtr WrapperPtr
    	{
    		get
    		{
				DelegateStruct func = new DelegateStruct();
				func.mFunc = mWrapperFunc;
				IntPtr *ptr =  stackalloc IntPtr[1];
				Marshal.StructureToPtr(func,new IntPtr(ptr),false);
				return(*ptr);
			}
    	}
    	
    	public static SvnDelegate NullFunc = new SvnDelegate(); 
    	
    	private SvnDelegate()
    	{
    		mFunc = null;
    		mWrapperFunc = null;
    	}
    	
    	// svn_wc_notify_func_t Wrapper
    	public SvnDelegate(SvnWcNotify.Func func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_wc_notify_func_t(SvnWcNotifyFuncWrapper);
    	}
    	
        private void SvnWcNotifyFuncWrapper(IntPtr baton, IntPtr path, 
        									int action, int kind, 
        									IntPtr mime_type, int content_state, 
        									int prop_state, int revision)
        {
        	SvnWcNotify.Func func = mFunc as SvnWcNotify.Func;
        	try {
            	Debug.WriteLine(String.Format("[Callback:{0}]SvnWcNotifyFunc({1:X},{2},{3},{4},{5},{6},{7},{8})",func.Method.Name,baton.ToInt32(),new SvnPath(path),(SvnWcNotify.Action) action,(Svn.NodeKind) kind,new AprString(mime_type),(SvnWcNotify.State) content_state,(SvnWcNotify.State) prop_state,revision));
        		func(baton, new SvnPath(path),
        		     (SvnWcNotify.Action) action, (Svn.NodeKind) kind, 
        		     new AprString(mime_type), (SvnWcNotify.State) content_state,
        		     (SvnWcNotify.State) prop_state, revision);
        	}
        	catch( Exception ) {
        		return;
        	}
        }

		
		// svn_client_get_commit_log_t Wrapper   	
    	public SvnDelegate(SvnClient.GetCommitLog func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_client_get_commit_log_t(SvnClientGetCommitLogWrapper);
    	}
    	
        private IntPtr SvnClientGetCommitLogWrapper(out IntPtr log_message, out IntPtr tmp_file, 
        									          IntPtr commit_items, IntPtr baton,
        									          IntPtr pool)
        {
        	log_message = IntPtr.Zero;
        	tmp_file = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnClient.GetCommitLog func = mFunc as SvnClient.GetCommitLog;
        	try {
	        	AprString logMessage;
	        	SvnPath tmpFile;
            	Debug.Write(String.Format("[Callback:{0}]SvnClientGetCommitLog({1},{2:X},{3})...",func.Method.Name,new AprArray(commit_items),baton.ToInt32(),new AprPool(pool)));
        		err = func(out logMessage, out tmpFile,
         				   new AprArray(commit_items,typeof(SvnClientCommitItem)), baton,
         				   new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0},{1})",logMessage,tmpFile));
        		log_message = logMessage;
        		tmp_file = tmpFile;
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);        
        }

    	    	
		// svn_cancel_func_t Wrapper   	
    	public SvnDelegate(Svn.CancelFunc func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_cancel_func_t(SvnCancelFuncWrapper);
    	}
    	
        private IntPtr SvnCancelFuncWrapper(IntPtr baton)
        {
       		SvnError err = SvnError.NoError;
        	Svn.CancelFunc func = mFunc as Svn.CancelFunc;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnCancelFunc({1:X})...",func.Method.Name,baton.ToInt32()));
        		err = func(baton);
           		Debug.WriteLine((err.IsNoError) ? "Return(NoError)" : String.Format("Return({0})",err.Message));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);        
        }


		// svn_wc_status_func_t Wrapper   	
    	public SvnDelegate(SvnWcStatus.Func func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_wc_status_func_t(SvnWcStatusFuncWrapper);
    	}
    	
	 	private void SvnWcStatusFuncWrapper(IntPtr baton, IntPtr path, IntPtr status)
	 	{
        	SvnWcStatus.Func func = mFunc as SvnWcStatus.Func;
        	try {
            	Debug.WriteLine(String.Format("[Callback:{0}]SvnWcStatusFunc({1:X},{2},{3})",func.Method.Name,baton.ToInt32(),new SvnPath(path),new SvnWcStatus(status)));
        		func(baton, new SvnPath(path), (SvnWcStatus) status);
        	}
        	catch( Exception ) {
        		return;
        	}
	 	}

    	
		// svn_log_message_receiver_t Wrapper   	
    	public SvnDelegate(SvnClient.LogMessageReceiver func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_log_message_receiver_t(SvnLogMessageReceiverWrapper);
    	}
    	
    	//[CLSCompliant(false))]
 		private IntPtr SvnLogMessageReceiverWrapper(IntPtr baton, IntPtr changed_paths, 
 												    int revision, IntPtr author,
 												    IntPtr date, IntPtr message,
 												    IntPtr pool)
 		{
       		SvnError err = SvnError.NoError;
        	SvnClient.LogMessageReceiver func = mFunc as SvnClient.LogMessageReceiver;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnLogMessageReceiver({1:X},{2},{3},{4},{5},{6},{7})...",func.Method.Name,baton.ToInt32(),new AprHash(changed_paths),revision,new AprString(author),new AprString(date),new AprString(message),new AprPool(pool)));
        		err = func(baton, new AprHash(changed_paths),
        				   revision, new AprString(author),
        				   new AprString(date), new AprString(message),
        				   new AprPool(pool));
           		Debug.WriteLine((err.IsNoError) ? "Return(NoError)" : String.Format("Return({0})",err.Message));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);        
 		}         								       
    	

		// svn_client_blame_receiver_t Wrapper   	
    	public SvnDelegate(SvnClient.BlameReceiver func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_client_blame_receiver_t(SvnClientBlameReceiverWrapper);
    	}
    	
 		private IntPtr SvnClientBlameReceiverWrapper(IntPtr baton, long lineNo, int revision,
 													 IntPtr author, IntPtr date, IntPtr line,
 													 IntPtr pool)
 		{
       		SvnError err = SvnError.NoError;
        	SvnClient.BlameReceiver func = mFunc as SvnClient.BlameReceiver;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnClientBlameReceiver({1:X},{2},{3},{4},{5},{6},{7})...",func.Method.Name,baton.ToInt32(),lineNo,revision,new AprString(author),new AprString(date),new AprString(line),new AprPool(pool)));
        		err = func(baton, lineNo, revision,
        				   new AprString(author), new AprString(date), new AprString(line),
        				   new AprPool(pool));
           		Debug.WriteLine((err.IsNoError) ? "Return(NoError)" : String.Format("Return({0})",err.Message));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);        
 		}         								       
 
    	    	    	    	    	    	
		// svn_read_fn_t Wrapper   	
    	public SvnDelegate(SvnStream.ReadFunc func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_read_fn_t(SvnStreamReadWrapper);
    	}
    	
 		private IntPtr SvnStreamReadWrapper(IntPtr baton, IntPtr buffer, ref uint len)
 		{
       		SvnError err = SvnError.NoError;
        	SvnStream.ReadFunc func = mFunc as SvnStream.ReadFunc;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnStreamReadFunc({1:X},{2:X},{3})...",func.Method.Name,baton.ToInt32(),buffer.ToInt32(),len));
	        	int slen = unchecked((int)len);
        		err = func(baton, buffer, ref slen);
	        	len = unchecked((uint)slen);
           		Debug.WriteLine(String.Format("Done({0})",len));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
 		}         								       
 		

		// svn_write_fn_t Wrapper   	
    	public SvnDelegate(SvnStream.WriteFunc func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_write_fn_t(SvnStreamWriteWrapper);
    	}
    	
 		private IntPtr SvnStreamWriteWrapper(IntPtr baton, IntPtr data, ref uint len)
 		{
       		SvnError err = SvnError.NoError;
        	SvnStream.WriteFunc func = mFunc as SvnStream.WriteFunc;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnStreamWriteFunc({1:X},{2:X},{3})...",func.Method.Name,baton.ToInt32(),data.ToInt32(),len));
	        	int slen = unchecked((int)len);
        		err = func(baton, data, ref slen);
	        	len = unchecked((uint)slen);
           		Debug.WriteLine(String.Format("Done({0})",len));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
 		}         								       
    	    	    	    	
    	    	    	    	    	    	    	    	    	    	    	    	
		// svn_close_fn_t Wrapper   	
    	public SvnDelegate(SvnStream.CloseFunc func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_close_fn_t(SvnStreamCloseWrapper);
    	}
    	
 		private IntPtr SvnStreamCloseWrapper(IntPtr baton)
 		{
       		SvnError err = SvnError.NoError;
        	SvnStream.CloseFunc func = mFunc as SvnStream.CloseFunc;
        	try {
            	Debug.Write(String.Format("[Callback:{0}]SvnStreamCloseFunc({1:X})...",func.Method.Name,baton.ToInt32()));
        		err = func(baton);
           		Debug.WriteLine((err.IsNoError) ? "Return(NoError)" : String.Format("Return({0})",err.Message));
        	}
        	catch( SvnException e ) {
        		err = SvnError.Create(e.AprErr, SvnError.NoError, e.Message);
        	}
        	catch( Exception e ) {
        		err = SvnError.Create(215000, SvnError.NoError, e.Message);
        	}
        	return(err);
 		}         								       
    	    	    	    	
    	    	    	    	    	    	    	    	    	    	    	    	
       	// svn_auth_simple_prompt_func_t Wrapper
    	public SvnDelegate(SvnAuthProviderObject.SimplePrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_simple_prompt_func_t(SvnAuthSimplePromptWrapper);
    	}
    	
        private IntPtr SvnAuthSimplePromptWrapper(out IntPtr cred, IntPtr baton, 
        								          IntPtr realm, IntPtr username, 
        								          int may_save, IntPtr pool)
        {
       		cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SimplePrompt func = 
        							mFunc as SvnAuthProviderObject.SimplePrompt;
        	try {
        		SvnAuthCredSimple credSimple;
            	Debug.Write(String.Format("[Callback:{0}]SimplePromptProvider({1:X},{2},{3},{4},{5})...",func.Method.Name,baton.ToInt32(),new AprString(realm),new AprString(username),(may_save != 0),new AprPool(pool)));
        		err = func(out credSimple, baton,
        		           new AprString(realm), new AprString(username), 
        		           (may_save != 0), new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0})",credSimple));
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


    	// svn_auth_username_prompt_func_t Wrapper
       	public SvnDelegate(SvnAuthProviderObject.UsernamePrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_username_prompt_func_t(SvnAuthUsernamePromptWrapper);
    	}

		private  IntPtr SvnAuthUsernamePromptWrapper(out IntPtr cred, IntPtr baton, 
											         IntPtr realm, int may_save, 
											         IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.UsernamePrompt func = 
        							mFunc as SvnAuthProviderObject.UsernamePrompt;
        	try {
        		SvnAuthCredUsername credUsername;
            	Debug.Write(String.Format("[Callback:{0}]UsernamePromptProvider({1:X},{2},{3},{4})...",func.Method.Name,baton.ToInt32(),new AprString(realm),(may_save != 0),new AprPool(pool)));
        		err = func(out credUsername, baton,
        		           new AprString(realm),  
        		           (may_save != 0), new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0})",credUsername));
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
        
        
    	// svn_auth_ssl_server_trust_prompt_func_t Wrapper
    	public SvnDelegate(SvnAuthProviderObject.SslServerTrustPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_server_trust_prompt_func_t(SvnAuthSslServerTrustPromptWrapper);
    	}

		//[CLSCompliant(false)]
		private  IntPtr SvnAuthSslServerTrustPromptWrapper(out IntPtr cred, IntPtr baton, 
													       IntPtr realm, uint failures, 
													       IntPtr cert_info, 
													       int may_save, IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslServerTrustPrompt func = 
        							mFunc as SvnAuthProviderObject.SslServerTrustPrompt;
        	try {
        		SvnAuthCredSslServerTrust credSslServerTrust;
            	Debug.Write(String.Format("[Callback:{0}]SslServerTrustPromptProvider({1:X},{2},{3},{4},{5},{6})...",baton.ToInt32(),new AprString(realm),unchecked((int)failures),new SvnAuthSslServerCertInfo(cert_info),(may_save != 0),new AprPool(pool)));
        		err = func(out credSslServerTrust, baton,
        		           new AprString(realm), (SvnAuthCredSslServerTrust.CertFailures) failures,
        		           new SvnAuthSslServerCertInfo(cert_info), 
        		           (may_save != 0), new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0})",credSslServerTrust));
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

    	
    	// svn_auth_ssl_client_cert_prompt_func_t Wrapper
       	public SvnDelegate(SvnAuthProviderObject.SslClientCertPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_client_cert_prompt_func_t(SvnAuthSslClientCertPromptWrapper);
    	}

		private  IntPtr SvnAuthSslClientCertPromptWrapper(out IntPtr cred, IntPtr baton,
												          IntPtr realm, int may_save,
												          IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslClientCertPrompt func = 
        							mFunc as SvnAuthProviderObject.SslClientCertPrompt;
        	try {
        		SvnAuthCredSslClientCert credSslClientCert;
            	Debug.Write(String.Format("[Callback:{0}]SslClientCertPromptProvider({1:X},{2},{3},{4})...",func.Method.Name,baton.ToInt32(),new AprString(realm),(may_save != 0),new AprPool(pool)));
        		err = func(out credSslClientCert, baton,
        		           new AprString(realm), 
        		           (may_save != 0), new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0})",credSslClientCert));
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


    	// svn_auth_ssl_client_cert_pw_prompt_func_t Wrapper
       	public SvnDelegate(SvnAuthProviderObject.SslClientCertPwPrompt func)
    	{
    		mFunc = func;
    		mWrapperFunc = new Svn.svn_auth_ssl_client_cert_pw_prompt_func_t(SvnAuthSslClientCertPwPromptWrapper);
    	}
    	
		private  IntPtr SvnAuthSslClientCertPwPromptWrapper(out IntPtr cred, 
													 		IntPtr baton,
													 		IntPtr realm, int may_save,
													 		IntPtr pool)
        {
        	cred = IntPtr.Zero;
       		SvnError err = SvnError.NoError;
        	SvnAuthProviderObject.SslClientCertPwPrompt func = 
        							mFunc as SvnAuthProviderObject.SslClientCertPwPrompt;
        	try {
        		SvnAuthCredSslClientCertPw credSslClientCertPw;
            	Debug.Write(String.Format("[Callback:{0}]SslClientCertPwPromptProvider({1:X},{2},{3},{4})...",func.Method.Name,baton.ToInt32(),new AprString(realm),(may_save != 0),new AprPool(pool)));
        		err = func(out credSslClientCertPw, baton,
        		           new AprString(realm),  
        		           (may_save != 0), new AprPool(pool));
            	Debug.WriteLine(String.Format("Done({0})",credSslClientCertPw));
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