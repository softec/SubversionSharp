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
    public class SvnClient
    {
    	public delegate SvnError GetCommitLog(out AprString logMessage, out AprString tmpFile,
         						 		   	  AprArray commitItems, IntPtr baton,
         								      AprPool pool);
         								       
		public static int SvnClientCheckout(string url, string path, 
											SvnOptRevision revision, 
											bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_checkout({0},{1},{2},{3},{4},{5})...",url,path,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_checkout(out rev, url, path, 
												   revision, 
												   (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		
		public static int SvnClientUpdate(string path, 
										  SvnOptRevision revision, 
										  bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_update({0},{1},{2},{3},{4})...",path,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_update(out rev, path, 
												 revision,
												 (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static int SvnClientSwitch(string path, string url, 
										  SvnOptRevision revision, 
										  bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_switch({0},{1},{2},{3},{4},{5})...",path,url,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_switch(out rev, path, url, 
												 revision, 
												 (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static void SvnClientAdd(string path,
										bool recurse, 
										SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_add({0},{1},{2},{3},{4})",path,recurse,ctx,pool));
			SvnError err = Svn.svn_client_add(path, 
											  (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
	}
}