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
using System.Runtime.InteropServices;
using NUnit.Framework;
using Softec.AprSharp;

namespace Softec.AprSharp.Test
{
	[TestFixture]
	public class AprArrayTest
	{
		[TestFixtureSetUp]
		public void Init()
    	{
		    //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
    	}
		
		[Test]
		public void Make()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#A01");
		
        	AprArray a = new AprArray();
        	Assert.IsTrue(a.IsNull,"#A02");
        	
        	a = AprArray.Make(p,10,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a.IsNull,"#A03");
        	Assert.AreEqual(((IntPtr) p).ToInt32(),((IntPtr)a.Pool).ToInt32(),"#A04");
        	Assert.AreEqual(10,a.AllocatedCount,"#A05");
        	Assert.AreEqual(Marshal.SizeOf(typeof(int)),a.ElementSize,"#A06");
        	Assert.IsTrue(a.IsEmpty(),"#A07");
        	Assert.AreEqual(0,a.Count,"#A08");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#A09");
		}

		[Test]
		public void PushPop()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#B01");
		
        	AprArray a = AprArray.Make(p,5,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a.IsNull,"#B02");
        	
        	IntPtr ptr = a.Push();
        	Assert.IsTrue(a != IntPtr.Zero,"#B03");
        	Marshal.WriteInt32(ptr,1);
        	Marshal.WriteInt32(a.Push(),2);
        	Marshal.WriteInt32(a.Push(),3);
        	Marshal.WriteInt32(a.Push(),4);
        	Marshal.WriteInt32(a.Push(),5);
        	
        	ptr = a.Pop();
        	Assert.IsTrue(a != IntPtr.Zero,"#B04");
        	Assert.AreEqual(5,Marshal.ReadInt32(ptr),"#B05");
        	Assert.AreEqual(4,Marshal.ReadInt32(a.Pop()),"#BO6");
        	Assert.AreEqual(3,Marshal.ReadInt32(a.Pop()),"#BO7");
        	Assert.AreEqual(2,Marshal.ReadInt32(a.Pop()),"#BO8");
        	Assert.AreEqual(1,Marshal.ReadInt32(a.Pop()),"#BO9");
        	
        	Marshal.WriteInt32(a.Push(),1);
        	Marshal.WriteInt32(a.Push(),2);
        	Marshal.WriteInt32(a.Push(),3);
        	Marshal.WriteInt32(a.Push(),4);
        	Marshal.WriteInt32(a.Push(),5);
        	Marshal.WriteInt32(a.Push(),6);
        	Marshal.WriteInt32(a.Push(),7);
        	Marshal.WriteInt32(a.Push(),8);
        	Marshal.WriteInt32(a.Push(),9);
        	Marshal.WriteInt32(a.Push(),10);

        	Assert.AreEqual(10,Marshal.ReadInt32(a.Pop()),"#B10");
        	Assert.AreEqual(9,Marshal.ReadInt32(a.Pop()),"#B11");
        	Assert.AreEqual(8,Marshal.ReadInt32(a.Pop()),"#B12");
        	Assert.AreEqual(7,Marshal.ReadInt32(a.Pop()),"#B13");
        	Assert.AreEqual(6,Marshal.ReadInt32(a.Pop()),"#B14");
        	Assert.AreEqual(5,Marshal.ReadInt32(a.Pop()),"#B15");
        	Assert.AreEqual(4,Marshal.ReadInt32(a.Pop()),"#B16");
        	Assert.AreEqual(3,Marshal.ReadInt32(a.Pop()),"#B17");
        	Assert.AreEqual(2,Marshal.ReadInt32(a.Pop()),"#B18");
        	Assert.AreEqual(1,Marshal.ReadInt32(a.Pop()),"#B19");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#B20");
		}
		
		[Test]
		public void Copy()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#C01");
		
        	AprArray a = AprArray.Make(p,5,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a.IsNull,"#C02");
        	
        	Marshal.WriteInt32(a.Push(),1);
        	Marshal.WriteInt32(a.Push(),2);
        	Marshal.WriteInt32(a.Push(),3);
        	Marshal.WriteInt32(a.Push(),4);
        	Marshal.WriteInt32(a.Push(),5);
        	
        	AprArray ca = a.Copy(p);
        	Assert.IsTrue(((IntPtr) a).ToInt32()!=((IntPtr)ca).ToInt32(),"#C03");
        	Assert.AreEqual(a.Count,ca.Count,"#C04");
        	Assert.AreEqual(5,Marshal.ReadInt32(ca.Pop()),"#C05");
        	Assert.AreEqual(4,Marshal.ReadInt32(ca.Pop()),"#CO6");
        	Assert.AreEqual(3,Marshal.ReadInt32(ca.Pop()),"#CO7");
        	Assert.AreEqual(2,Marshal.ReadInt32(ca.Pop()),"#CO8");
        	Assert.AreEqual(1,Marshal.ReadInt32(ca.Pop()),"#CO9");        	

           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#C10");
        }		

		[Test]
		public void Append()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#D01");
		
        	AprArray a = AprArray.Make(p,2,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a.IsNull,"#D02");
        	
        	Marshal.WriteInt32(a.Push(),1);
        	Marshal.WriteInt32(a.Push(),2);
        	
        	AprArray a2 = AprArray.Make(p,3,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a2.IsNull,"#D03");
        	
        	Marshal.WriteInt32(a2.Push(),3);
        	Marshal.WriteInt32(a2.Push(),4);
        	Marshal.WriteInt32(a2.Push(),5);

           	AprArray ca = a.Append(p,a2);
        	Assert.AreEqual(a.Count+a2.Count,ca.Count,"#D04");
        	Assert.AreEqual(5,Marshal.ReadInt32(ca.Pop()),"#D05");
        	Assert.AreEqual(4,Marshal.ReadInt32(ca.Pop()),"#DO6");
        	Assert.AreEqual(3,Marshal.ReadInt32(ca.Pop()),"#DO7");
        	Assert.AreEqual(2,Marshal.ReadInt32(ca.Pop()),"#DO8");
        	Assert.AreEqual(1,Marshal.ReadInt32(ca.Pop()),"#DO9");
        	        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#D10");
        }		
	
		[Test]
		public void Cat()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#D01");
		
        	AprArray a = AprArray.Make(p,2,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a.IsNull,"#D02");
        	
        	Marshal.WriteInt32(a.Push(),1);
        	Marshal.WriteInt32(a.Push(),2);
        	
        	AprArray a2 = AprArray.Make(p,3,Marshal.SizeOf(typeof(int)));
        	Assert.IsFalse(a2.IsNull,"#D03");
        	
        	Marshal.WriteInt32(a2.Push(),3);
        	Marshal.WriteInt32(a2.Push(),4);
        	Marshal.WriteInt32(a2.Push(),5);

           	a.Cat(a2);
        	Assert.AreEqual(5,Marshal.ReadInt32(a.Pop()),"#D05");
        	Assert.AreEqual(4,Marshal.ReadInt32(a.Pop()),"#DO6");
        	Assert.AreEqual(3,Marshal.ReadInt32(a.Pop()),"#DO7");
        	Assert.AreEqual(2,Marshal.ReadInt32(a.Pop()),"#DO8");
        	Assert.AreEqual(1,Marshal.ReadInt32(a.Pop()),"#DO9");
        	        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#D10");
        }		

		[Test]
		public void StrCat()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#C01");
		
        	AprArray a = AprArray.Make(p,4,Marshal.SizeOf(typeof(IntPtr)));
        	Assert.IsFalse(a.IsNull,"#C02");
        	
        	Marshal.WriteIntPtr(a.Push(),new AprString(p,"This"));
        	Marshal.WriteIntPtr(a.Push(),new AprString(p,"is"));
        	Marshal.WriteIntPtr(a.Push(),new AprString(p,"a"));
        	Marshal.WriteIntPtr(a.Push(),new AprString(p,"test."));
        	
        	Assert.AreEqual("This is a test.",a.StrCat(p,' '),"#CO3");        	

           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#C04");
        }		
    }
}