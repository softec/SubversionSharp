//  SvnTest, a client program used to test SubversionSharp library
//  Copyright 2004 by SOFTEC sa
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License as
//  published by the Free Software Foundation; either version 2 of
// the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of 
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//  See the GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public
//  License along with this program; if not, write to the Free Software
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
using System.Collections;
using Mono.GetOptions;
using Softec.AprSharp;
using Softec.SubversionSharp;

namespace Softec.SubversionSharp.Test {

	abstract class CmdBase : Options
	{
		protected Application.SubCommand mSubCmd;
		
		protected SvnClient client;
		
		#region Options
		
		[Option("print debug information", 'd', "debug")]
		public bool oDebug = false;
		
		[Option("print as little as possible", 'q', "quiet")]
		public bool oQuiet = false;

		[Option("read user configuration files from {DIR}", "config-dir")]
		public string oConfigDir = null;
		
		public bool oInteractive = true;
		#endregion
		
		protected SvnRevision StringToRevision(string value)
		{
			try {
				return new SvnRevision(int.Parse(value));
			}
			catch {}
			
			switch(value.ToUpper())
			{
				case "HEAD":
					return new SvnRevision(Svn.Revision.Head);
				case "BASE":
					return new SvnRevision(Svn.Revision.Base);
				case "COMMITED":
					return new SvnRevision(Svn.Revision.Committed);
				case "PREV":
					return new SvnRevision(Svn.Revision.Previous);
					
				default:
					return new SvnRevision(AprTime.FromDateTime(DateTime.Parse(value)));
			}
		}
		
		protected abstract int Execute();
	
		public virtual int Run(Application.SubCommand sc, string[] args)
		{
			int res;
			
			mSubCmd = sc;
			ProcessArgs(args);

			try {
				AprPool p = Svn.PoolCreate();
		        SvnClientContext ctx = SvnClientContext.Create(p);
		        if( oConfigDir != null )
		        	ctx.Config = SvnConfig.GetConfig(new SvnPath(oConfigDir,p), p);
				else
					ctx.Config = SvnConfig.GetConfig(p);
				
				client = new SvnClient(ctx, p);
				
				client.AddSimpleProvider();
	        	client.AddUsernameProvider();
	        	client.AddSslServerTrustFileProvider();
	        	client.AddSslClientCertFileProvider();
	        	client.AddSslClientCertPwFileProvider();
	        	
	        	if( oInteractive )
	        	{
			        client.AddPromptProvider(
			        				new SvnAuthProviderObject.SimplePrompt(SimpleAuth),
			        				IntPtr.Zero, 2);
			        client.AddPromptProvider(
			        				new SvnAuthProviderObject.UsernamePrompt(UsernameAuth),
			        				IntPtr.Zero, 2);
			        client.AddPromptProvider(
			        				new SvnAuthProviderObject.SslServerTrustPrompt(SslServerTrustAuth),
			        				IntPtr.Zero);
			    }
		        client.OpenAuth();

				if( !oQuiet )
		        	client.Context.NotifyFunc = new SvnDelegate(new SvnWcNotify.Func(NotifyCallback));
		        	
	        	client.Context.LogMsgFunc = new SvnDelegate(new SvnClient.GetCommitLog(GetCommitLogCallback));
	        	client.Context.CancelFunc = new SvnDelegate(new Svn.CancelFunc(CancelCallback));
				
				res = Execute();
			}
			catch( Exception e )
			{
				if( oDebug )
					Console.WriteLine(e);
				else
					Console.WriteLine(e.Message);
				res = -1;
			}
			finally
			{			
				client.Pool.Destroy();
			}
			return(res);        
		}
		
		private WhatToDoNext UsageAppend(WhatToDoNext ret)
		{
			if(mSubCmd.Description != string.Empty)
				Console.WriteLine("{0} {1}",mSubCmd.LongName,mSubCmd.Description);
			return(ret);
		}
			
		public override WhatToDoNext DoUsage()
		{
			return UsageAppend(base.DoUsage());
		}

		public override WhatToDoNext DoHelp()
		{
			return UsageAppend(base.DoHelp());
		}
		
		public static void NotifyCallback(IntPtr baton, SvnPath Path,  
	        			 	 	          SvnWcNotify.Action action, Svn.NodeKind kind,
	        			 		          AprString mimeType, SvnWcNotify.State contentState,
	        			 		          SvnWcNotify.State propState, int revNum)
	    {
	    }
		
		public static SvnError GetCommitLogCallback(out AprString logMessage, out SvnPath tmpFile,
							 		   	  	        AprArray commitItems, IntPtr baton,
									      	        AprPool pool)
		{
			if (!commitItems.IsNull)
			{
				foreach (SvnClientCommitItem item in commitItems)
				{
					Console.WriteLine("C1: {1} ({2}) r{3}",
						item.Path, item.Kind, item.Revision);
					Console.WriteLine("C2: {1} -> {2}",
						item.Url,
						item.CopyFromUrl);
				}
				Console.WriteLine();
			}
			
			Console.Write("Enter log message: ");
			logMessage = new AprString(Console.ReadLine(), pool);
			tmpFile = new AprString();
			
			return(SvnError.NoError);
		}

		
		public static SvnError CancelCallback(IntPtr baton)
		{
			return(SvnError.NoError);		
		}
		
		public static SvnError SimpleAuth(out SvnAuthCredSimple cred, IntPtr baton, 
	        				   AprString realm, AprString username, 
	        				   bool maySave, AprPool pool)
		{
			Console.WriteLine("Simple Authentication");
			Console.WriteLine("---------------------");
			Console.WriteLine("Realm: {0}", realm);
			Console.WriteLine("");
			
			bool valid = false;
			string line = "";
			
			while(!valid)
			{
				if (!username.IsNull)
					Console.Write("Enter Username ({0}): ", username);
				else
					Console.Write("Enter Username: ");

				line = Console.ReadLine();

				if (line.Trim().Length == 0 && !username.IsNull)
				{
					line = username.ToString();
					valid = true;
				}
				else if (line.Trim().Length > 0)
				{
					valid = true;
				}
			}
			
			cred = SvnAuthCredSimple.Alloc(pool);
			cred.Username = new AprString(line, pool);
			Console.Write("Enter Password: ");
			cred.Password = new AprString(Console.ReadLine(), pool);
			cred.MaySave = maySave;
			return(SvnError.NoError);
		}
		
		public static SvnError UsernameAuth(out SvnAuthCredUsername cred, IntPtr baton, 
											AprString realm, bool maySave, AprPool pool)
		{
			Console.WriteLine("Username Authentication:");
			Console.WriteLine("------------------------");
			Console.WriteLine("Realm: {0}", realm);
			Console.WriteLine("");

			bool valid = false;
			string line = "";
			
			while(!valid)
			{
				Console.Write("Enter Username: ");

				line = Console.ReadLine();

				if (line.Trim().Length > 0)
				{
					valid = true;
				}
			}
			
			cred = SvnAuthCredUsername.Alloc(pool);
			cred.Username = new AprString(line, pool);
			cred.MaySave = maySave;
			return(SvnError.NoError);
		}
		
	    public static SvnError SslServerTrustAuth(out SvnAuthCredSslServerTrust cred, 
						       				      IntPtr baton, AprString realm, 
										   		  SvnAuthCredSslServerTrust.CertFailures failures, 
										   		  SvnAuthSslServerCertInfo certInfo, 
										   		  bool maySave, IntPtr pool)
		{
			Console.WriteLine("Ssl Server Trust Prompt:");
			Console.WriteLine("------------------------");
			Console.WriteLine("");
			
			Console.WriteLine("Error validating server certificate for '{0}':", realm);
			if ((failures & SvnAuthCredSslServerTrust.CertFailures.UnknownCA) > 0)
				Console.WriteLine(" - The certificate is not issued by a trusted authority");
			if ((failures & SvnAuthCredSslServerTrust.CertFailures.CNMismatch) > 0)
				Console.WriteLine(" - The certificate hostname does not match");
			if ((failures & SvnAuthCredSslServerTrust.CertFailures.NotYetValid) > 0)
				Console.WriteLine(" - The certificate is not yet valid");
			if ((failures & SvnAuthCredSslServerTrust.CertFailures.Expired) > 0)
				Console.WriteLine(" - The certificate has expired");
			if ((failures & SvnAuthCredSslServerTrust.CertFailures.Other) > 0)
				Console.WriteLine(" - The certificate has an unknown error");
		
			Console.WriteLine("Certificate informations:");
			Console.WriteLine("\tHostName:    " + certInfo.Hostname);
			Console.WriteLine("\tIssuer:      " + certInfo.IssuerDName);
			Console.WriteLine("\tValid From:  " + certInfo.ValidFrom);
			Console.WriteLine("\tValid Until: " + certInfo.ValidUntil);
			Console.WriteLine("\tFingerprint: " + certInfo.Fingerprint);
		
			cred = SvnAuthCredSslServerTrust.Alloc(pool);
			bool valid = false;
			while (!valid)
			{
				if (maySave)
					Console.WriteLine("(R)eject, accept (t)emporarily or accept (p)ermanently? ");
				else
					Console.WriteLine("(R)eject or accept (t)emporarily? ");
					
				string line = Console.ReadLine();
				if (line.Length > 0)
				{
					char choice = line.ToLower()[0];
					if (choice == 'r')
					{
						cred.AcceptedFailures = 0;
						cred.MaySave=false;
						valid = true;
					}
					else if (choice == 't')
					{
						cred.AcceptedFailures = failures;
						cred.MaySave=false;
						valid = true;
					}
					else if (choice == 'p')
					{
						cred.AcceptedFailures = failures;
						cred.MaySave=true;
						valid = true;
					}
				}
			}
			return(SvnError.NoError);
		}
			
		}


		abstract class CmdBaseWithAuth : CmdBase
		{
			[Option("authentify using username {USER}", "username")]
			public string oUsername = null;
			
			[Option("authentify using password {PASS}", "password")]
			public string oPassword = null;
			
			[Option("do not cache authentication tokens", "no-auth-cache")]
			public bool oMayNotSave {
				set { oMaySave = !value; }
			}
			public bool oMaySave = true;
			
			[Option("do no interactive prompting", "non-interactive")]
			public bool oNotInteractive {
				set { oInteractive = !value; }
			}
		}	
}
