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
using NUnit.Framework;
using Softec.AprSharp;

namespace Softec.AprSharp.Test
{
	[TestFixture]
	public class AprMutexTest
	{
		[TestFixtureSetUp]
		public void Init()
    	{
		    //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
    	}
		
		[Test]
		public void CreateDestroy()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#A01");
		
        	AprThreadMutex m = new AprThreadMutex();
        	Assert.IsTrue(m.IsNull,"#A02");
        	
        	m = AprThreadMutex.Create(p);
        	Assert.IsFalse(m.IsNull,"#A03");
        	Assert.AreEqual(((IntPtr)p).ToInt32(),((IntPtr)(m.Pool)).ToInt32(),"#A04");

        	m.Destroy();
        	Assert.IsTrue(m.IsNull,"#A05");
        	
        	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#A06");
		}

		[Test]
 		public void LockTryLockUnlock()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#B01");
		
        	AprThreadMutex m = AprThreadMutex.Create(p);
        	Assert.IsFalse(m.IsNull,"#B02");
        	Assert.AreEqual(((IntPtr)p).ToInt32(),((IntPtr)(m.Pool)).ToInt32(),"#B03");

	      	Assert.IsTrue(m.TryLock(),"#B04");
        	Assert.IsFalse(m.TryLock(),"#B05");
        	m.Unlock();
        	Assert.IsTrue(m.TryLock(),"#B06");
        	m.Unlock();
        	m.Lock();
        	Assert.IsFalse(m.TryLock(),"#B07");
        	m.Unlock();
        	Assert.IsTrue(m.TryLock(),"#B08");
        	m.Unlock();
        
        	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#A06");
		}        	
        	
		[Test]
 		public void AllocatorMutex()
		{
		    AprAllocator a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull,"#C01");

			AprPool p = AprPool.Create(a);
			Assert.IsFalse(p.IsNull,"#C02");
			Assert.AreEqual(((IntPtr)a).ToInt32(),((IntPtr)(p.Allocator)).ToInt32(),"#C03");
			
			a.Owner = p;
			Assert.AreEqual(((IntPtr)p).ToInt32(),((IntPtr)(a.Owner)).ToInt32(),"#C04");
			
        	AprThreadMutex m = AprThreadMutex.Create(p);
        	Assert.IsFalse(m.IsNull,"#C05");
        	Assert.AreEqual(((IntPtr)p).ToInt32(),((IntPtr)(m.Pool)).ToInt32(),"#C06");

			a.Mutex = m;
			Assert.AreEqual(((IntPtr)m).ToInt32(),((IntPtr)(a.Mutex)).ToInt32(),"#C07");
			
			p.Destroy();
			Assert.IsTrue(p.IsNull,"#C08");
		}		
	}
}