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
	public class AprHashTest
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
		
        	AprHash h = new AprHash();
        	Assert.IsTrue(h.IsNull,"#A02");
        	
        	h = AprHash.Make(p);
        	Assert.IsFalse(h.IsNull,"#A03");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#A04");
		}

		[Test]
		public void SetGet()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#B01");
		
        	AprHash h = AprHash.Make(p);
        	Assert.IsFalse(h.IsNull,"#B02");
        	
        	h.Set("A","A1");
        	h.Set("B","B2");
        	h.Set("C","C3");
        	h.Set("D","D4");
        	h.Set("E","E5");
        	
        	Assert.AreEqual("A1",h.GetAsString("A"),"#B03");
        	Assert.AreEqual("B2",h.GetAsString("B"),"#B04");
        	Assert.AreEqual("C3",h.GetAsString("C"),"#B05");
        	Assert.AreEqual("D4",h.GetAsString("D"),"#B06");
        	Assert.AreEqual("E5",h.GetAsString("E"),"#B07");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#B08");
		}
		
		[Test]
		public void CountCopy()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#C01");
		
        	AprHash h = AprHash.Make(p);
        	Assert.IsFalse(h.IsNull,"#C02");
        	Assert.AreEqual(0,h.Count,"#C03");
        	
        	h.Set("A","A1");
        	h.Set("B","B2");
        	h.Set("C","C3");
        	h.Set("D","D4");
        	h.Set("E","E5");
        	Assert.AreEqual(5,h.Count,"#C04");
        	
        	AprHash ch = h.Copy(p);
        	Assert.IsTrue(((IntPtr)h).ToInt32()!=((IntPtr)ch).ToInt32(),"#C05");
        	Assert.AreEqual(h.Count,ch.Count,"#C06");
        	Assert.AreEqual("A1",ch.GetAsString("A"),"#C07");
        	Assert.AreEqual("B2",ch.GetAsString("B"),"#C08");
        	Assert.AreEqual("C3",ch.GetAsString("C"),"#C09");
        	Assert.AreEqual("D4",ch.GetAsString("D"),"#C10");
        	Assert.AreEqual("E5",ch.GetAsString("E"),"#C11");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#C12");
		}		

		[Test]
		public void Overlay()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#C01");
		
        	AprHash h = AprHash.Make(p);
        	Assert.IsFalse(h.IsNull,"#C02");
        	
        	h.Set("A","A1");
        	h.Set("B","B2");
        	h.Set("C","CC3");
        	h.Set("D","D4");
        	h.Set("E","E5");
        	
        	AprHash h2 = AprHash.Make(p);
        	Assert.IsFalse(h2.IsNull,"#C03");
        	
        	h.Set("C","C3");
        	h.Set("D","D4");
        	h.Set("F","F6");
        	h.Set("G","G7");

        	AprHash ch = h.Overlay(p,h2);
        	Assert.AreEqual(7,ch.Count,"#C03");
        	
        	Assert.AreEqual("A1",ch.GetAsString("A"),"#C04");
        	Assert.AreEqual("B2",ch.GetAsString("B"),"#C05");
        	Assert.AreEqual("C3",ch.GetAsString("C"),"#C06");
        	Assert.AreEqual("D4",ch.GetAsString("D"),"#C07");
        	Assert.AreEqual("E5",ch.GetAsString("E"),"#C08");
        	Assert.AreEqual("F6",ch.GetAsString("F"),"#C09");
        	Assert.AreEqual("G7",ch.GetAsString("G"),"#C10");
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#C11");
		}
		
		[Test]
		public void CopyTo()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#C01");
		
        	AprHash h = AprHash.Make(p);
        	Assert.IsFalse(h.IsNull,"#C02");
				
        	h.Set("A","A1");
        	h.Set("B","B2");
        	h.Set("C","C3");
        	h.Set("D","D4");
        	h.Set("E","E5");
        	
        	AprHashEntry[] a = new AprHashEntry[5]; 
        	h.CopyTo(a,0);
        	Assert.AreEqual("A",a[0].KeyAsString,"#C03");
        	Assert.AreEqual("A1",a[0].ValueAsString,"#C04");
        	Assert.AreEqual("B",a[1].KeyAsString,"#C05");
        	Assert.AreEqual("B2",a[1].ValueAsString,"#C06");
        	Assert.AreEqual("C",a[2].KeyAsString,"#C07");
        	Assert.AreEqual("C3",a[2].ValueAsString,"#C08");
        	Assert.AreEqual("D",a[3].KeyAsString,"#C09");
        	Assert.AreEqual("D4",a[3].ValueAsString,"#C10");
        	Assert.AreEqual("E",a[4].KeyAsString,"#C11");
        	Assert.AreEqual("E5",a[4].ValueAsString,"#C12");
        	
        	
           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#C13");
		}
	}
}