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
	public class AprAllocatorTest
	{
		[TestFixtureSetUp]
		public void Init()
    	{
		    //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
    	}
		
		[Test]
		public void CreateDestroy()
		{
			AprAllocator a = new AprAllocator();
			Assert.IsTrue(a.IsNull(),"#A01");
			
			a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull(),"#A02");
			
			a.Destroy();
			Assert.IsTrue(a.IsNull(),"#A03");
		}

		private AprMemNode AllocHelper(AprAllocator a, int size, string test)
		{
			AprMemNode m = a.Alloc(size);
			Assert.IsFalse(m.IsNull(),test+"a");
			if( (size+24) <= 8192 ) {
				Assert.AreEqual(1,m.NativeIndex,test+"c");
				Assert.AreEqual((8192-24),(m.EndP.ToInt32()-m.FirstAvail.ToInt32()),test+"d");
			} else {
				Assert.AreEqual((size-4097+24)/4096+1,m.NativeIndex,test+"c");
				Assert.AreEqual((m.NativeIndex-1)*4096+(8192-24),(m.EndP.ToInt32()-m.FirstAvail.ToInt32()),test+"d");
			}
			return m;
		}

		[Test]
		public void AllocSimple()
		{
			AprAllocator a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull(),"#B01");

			AprMemNode m = AllocHelper(a,256,"#B02");
			a.Free(m);
			
			a.Destroy();
			Assert.IsTrue(a.IsNull(),"#B03");
		}
		

		[Test]
		public void AllocCriticalSize()
		{
			AprAllocator a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull(),"#C01");

			AprMemNode m = AllocHelper(a,256,"#C02");
			a.Free(m);

			AprMemNode m1 = AllocHelper(a,512,"#C03");
			AprMemNode m2 = AllocHelper(a,1024,"#C04");
			a.Free(m2);

			AprMemNode m3 = AllocHelper(a,2048,"#C05");
			AprMemNode m4 = AllocHelper(a,4096,"#C06");
			AprMemNode m5 = AllocHelper(a,6148,"#C07");
			a.Free(m5);
			
			a.Free(m3);
			a.Free(m1);
			a.Free(m4);

			m1 = AllocHelper(a,9216,"#D02");
			m2 = AllocHelper(a,12265,"#D03");
			m3 = AllocHelper(a,16384,"#D04");
			a.Free(m2);
			a.Free(m3);
			a.Free(m1);

			a.Destroy();
			Assert.IsTrue(a.IsNull(),"#C08");
		}
		
		[Test]
		public void AllocLoop()
		{
			AprAllocator a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull(),"#D000001");

			int adrChange = 0;
			int currAdr = 0;
			int lastAdr = 0;
			AprMemNode m;
			for(int i=24;i<(4096*1024);i+=24)
			{
				m = AllocHelper(a,i,String.Format("#D{0,6}",i+3));
				currAdr = ((IntPtr)m).ToInt32();
				if( lastAdr != currAdr )
				{
					lastAdr = currAdr;
					adrChange++;
				}
				a.Free(m);
			}
			Assert.AreEqual(1024,adrChange,1,"#D000002");
			
			a.Destroy();
			Assert.IsTrue(a.IsNull(),"#D000003");
		}

		[Test]
		public void AllocLoopMaxFree()
		{
			AprAllocator a = AprAllocator.Create();
			Assert.IsFalse(a.IsNull(),"#E000001");
			a.MaxFree = 81920;
			
			int adrChange = 0;
			int currAdr = 0;
			int lastAdr = 0;
			AprMemNode m;
			for(int i=24;i<(4096*1024);i+=24)
			{
				m = AllocHelper(a,i,String.Format("#E{0,6}",i+3));
				currAdr = ((IntPtr)m).ToInt32();
				if( lastAdr != currAdr )
				{
					lastAdr = currAdr;
					adrChange++;
				}
				a.Free(m);
			}
			Assert.AreEqual(50,adrChange,50,"#D000002"); // Normaly less than 10

			a.Destroy();
			Assert.IsTrue(a.IsNull(),"#E000003");
		}
	}
}