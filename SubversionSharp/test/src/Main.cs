// project created on 5/13/2004 at 10:00 PM
using System;
using System.Diagnostics;
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
        AprHash ctxcfg = ctx.Config;
        foreach(AprHashEntry cfg in ctxcfg)
        {
        	Console.WriteLine("{0}\t{1:X}",cfg.KeyAsString,cfg.Value.ToInt32());
        }
        pool.Destroy();
	}
}