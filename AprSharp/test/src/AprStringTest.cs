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
	public class AprStringTest
	{
		[TestFixtureSetUp]
		public void Init()
    	{
		    //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
    	}
		
		[Test]
		public void Duplicate()
		{
			AprPool p = AprPool.Create();
        	Assert.IsFalse(p.IsNull,"#A01");
		
        	AprString s = new AprString();
        	Assert.IsTrue(s.IsNull,"#A02");
        	
        	s = AprString.Duplicate(p,"This is a test of string duplication");
        	Assert.IsFalse(s.IsNull,"#A03");
        	Assert.AreEqual("This is a test of string duplication",s.ToString(),"#A04");
        	
        	AprString s2 = AprString.Duplicate(p,s);
        	Assert.IsFalse(s2.IsNull,"#A05");
        	Assert.AreEqual("This is a test of string duplication",s2.ToString(),"#A06");

        	s2 = AprString.Duplicate(p,s,14);
        	Assert.IsFalse(s2.IsNull,"#A07");
        	Assert.AreEqual("This is a test",s2.ToString(),"#A08");

        	s = AprString.Duplicate(p,"This is a test of string duplication",14);
        	Assert.IsFalse(s.IsNull,"#A09");
        	Assert.AreEqual("This is a test",s.ToString(),"#A10");

           	p.Destroy();
        	Assert.IsTrue(p.IsNull,"#A11");
		}
	}
}