// project created on 4/28/04 at 9:26 a
using System;
using Softec.AprSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class MainClass
{
	public static void Main(string[] args)
	{
	    Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
        Console.WriteLine("Hello World!");
        
        AprAllocator a = AprAllocator.Create();
        AprPool p = AprPool.Create(a);
        Debug.WriteLine("apr_pool_allocator_get("+p+")=" + p.Allocator);
        Debug.WriteLine("apr_allocator_owner_get("+a+")=" + a.Owner);
        a.Owner = p;
        Debug.WriteLine("apr_allocator_owner_get("+a+")=" + a.Owner);
        AprPool.Create(p).Destroy();
        AprPool.Create(p,a).Destroy();
        AprThreadMutex l=AprThreadMutex.Create(p);
        Debug.WriteLine("apr_thread_mutex_pool_get("+l+")=" + l.Pool);        
        l.Lock();
        Debug.WriteLine("apr_thread_mutex_trylock("+l+")=" + l.TryLock());        
        l.Unlock();
        Debug.WriteLine("apr_thread_mutex_trylock("+l+")=" + l.TryLock());        
        l.Unlock();
        Debug.WriteLine("apr_allocator_mutex_get("+a+")=" + a.Mutex);        
        a.Mutex=l;
        Debug.WriteLine("apr_allocator_mutex_get("+a+")=" + a.Mutex);        
        a.Free(a.Alloc(128));
        
        GCHandle mt;
        AprTimeExp t = AprTimeExp.ManagedAlloc(out mt);
        long now = AprTimeExp.Now();
        Debug.WriteLine("apr_time_now()="+now);
        t.Time = now;
        Debug.WriteLine("apr_rfc822_date=" + AprTimeExp.Rfc822Date(now));
        Debug.WriteLine("apr_ctime=" + AprTimeExp.CTime(now));
        Debug.WriteLine("apr_time_exp_____get=" + t.Time + DumpAprTimeExp(t));
        Debug.WriteLine("apr_time_exp_gmt_get=" + t.GmtTime + DumpAprTimeExp(t));
        Debug.WriteLine("apr_strftime=" + t.ToString("%Y%m%d%H%M%S"));
        t.Time = t.Time;
        Debug.WriteLine("apr_time_exp_____get=" + t.Time + DumpAprTimeExp(t));
        Debug.WriteLine("apr_time_exp_gmt_get=" + t.GmtTime + DumpAprTimeExp(t));
        Debug.WriteLine("apr_strftime=" + t.ToString("%Y%m%d%H%M%S"));
        t.GmtTime = now;
        Debug.WriteLine("apr_time_exp_____get=" + t.Time + DumpAprTimeExp(t));
        Debug.WriteLine("apr_time_exp_gmt_get=" + t.GmtTime + DumpAprTimeExp(t));
        Debug.WriteLine("apr_strftime=" + t.ToString("%Y%m%d%H%M%S"));
        t.GmtTime = t.GmtTime;
        Debug.WriteLine("apr_time_exp_____get=" + t.Time + DumpAprTimeExp(t));
        Debug.WriteLine("apr_time_exp_gmt_get=" + t.GmtTime + DumpAprTimeExp(t));
        Debug.WriteLine("apr_strftime=" + t.ToString("%Y%m%d%H%M%S"));
        t.SetTimeTZ(now, 7200);
        Debug.WriteLine("apr_time_exp_____get=" + t.Time + DumpAprTimeExp(t));
        Debug.WriteLine("apr_time_exp_gmt_get=" + t.GmtTime + DumpAprTimeExp(t));
        Debug.WriteLine("apr_strftime=" + t.ToString("%Y%m%d%H%M%S"));
        mt.Free();
        p.Destroy();

/*
        a = AprAllocator.Create();
        AprMemNode m = a.Alloc(16384);
        PrintMemNode("m", m, true);
        AprMemNode m1 = a.Alloc(32767);
        PrintMemNode("m", m, true);
        PrintMemNode("m1", m1, true);
        a.Free(m1);
        PrintMemNode("m", m, true);
        a.Destroy();
*/
    }
    
    public unsafe static void PrintMemNode(string name, AprMemNode m, bool r)
    {
        if (name != null) Debug.WriteLine("Dump "+name+"="+m.ToString());
        Debug.Indent();
        Debug.WriteLine("m.Next=" + m.Next.ToString());
        if (!m.Next.IsNull() && r) PrintMemNode(name, m.Next, false);
        Debug.WriteLine("m.NativeRef=" + ((Int32)(m.NativeRef)).ToString("X"));
        //Debug.WriteLine("m.Ref=" + m.Ref.ToString());
        //if (!m.Ref.IsNull() && r) PrintMemNode(name, m.Ref, false);
        Debug.WriteLine("m.Index=" + m.Index);
        Debug.WriteLine("m.FreeIndex=" + m.FreeIndex);
        Debug.WriteLine("m.FirstAvail=" + ((Int32)(m.FirstAvail)).ToString("X"));
        Debug.WriteLine("m.EndP=" + ((Int32)(m.EndP)).ToString("X"));
        Debug.Unindent();
    }
    
    public static string DumpAprTimeExp(AprTimeExp t)
    {
        return("{"+t.Day+"/"+t.Month+"/"+t.Year+" "+t.Hours+":"+t.Minutes+":"+t.Seconds+"."+t.MicroSeconds+" off"+t.TimeZone+" W"+t.WeekDay+" Y"+t.YearDay+"}");
    }
}