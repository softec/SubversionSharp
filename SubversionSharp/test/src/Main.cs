// project created on 5/13/2004 at 10:00 PM
using System;
using System.Diagnostics;
using System.Collections;
using Softec.AprSharp;
using Softec.SubversionSharp;

class MainClass
{
	public static void Main(string[] args)
	{
	    Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
        SvnConfig.Ensure();
        
        AprPool pool =  Svn.PoolCreate();
        SvnClientContext ctx = SvnClientContext.Create(pool);
        ctx.Config = SvnConfig.GetConfig(pool);
        
        ArrayList authObj = new ArrayList();
        authObj.Add(SvnAuthProviderObject.GetSimpleProvider(pool));
        authObj.Add(SvnAuthProviderObject.GetUsernameProvider(pool));
        authObj.Add(SvnAuthProviderObject.GetSslServerTrustFileProvider(pool));
        authObj.Add(SvnAuthProviderObject.GetSslClientCertFileProvider(pool));
        authObj.Add(SvnAuthProviderObject.GetSslClientCertPwFileProvider(pool));
        authObj.Add(SvnAuthProviderObject.GetPromptProvider(
        				new SvnAuthProviderObject.SimplePrompt(SimpleAuth),
        				IntPtr.Zero, 2, pool));
        authObj.Add(SvnAuthProviderObject.GetPromptProvider(
        				new SvnAuthProviderObject.UsernamePrompt(UsernameAuth),
        				IntPtr.Zero, 2, pool));
        
        
        pool.Destroy();
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
		string line;
		
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
		cred.Username = new AprString(pool, line);
		Console.Write("Enter Password: ");
		cred.Password = new AprString(pool, Console.ReadLine());
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
		string line;
		
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
		cred.Username = new AprString(pool, line);
		cred.MaySave = maySave;
		return(SvnError.NoError);
	}
}