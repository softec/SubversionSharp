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

namespace Softec.AprSharp
{
	public interface IAprUnmanaged
	{
		IntPtr ToIntPtr();
		
        bool IsNull
        {
        	get;
        }

        void ClearPtr();
        
		bool ReferenceEquals(IAprUnmanaged obj);		
	} 
}