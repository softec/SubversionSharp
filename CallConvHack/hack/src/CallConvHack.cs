// ConvCallHack, an helper program to create cdecl delegates
#region Copyright (C) 2004 SOFTEC sa.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
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
#endregion
//
using System;
using System.IO;
using System.Text.RegularExpressions;

/*
 * Transform this:
 * 
.assembly extern CallConvAttribute
{
  .ver 1:0:1782:35477
}

.class public auto ansi sealed MyDelegate
    extends [mscorlib]System.MulticastDelegate
{
  .custom instance void [CallConvAttribute]Softec.CallConvCdeclAttribute::.ctor() = ( 01 00 00 00 ) 
  .method public hidebysig specialname rtspecialname 
       instance void  .ctor(object 'object',
                        native int 'method') runtime managed
  {
  } // end of method MyDelegate::.ctor

  .method public hidebysig virtual instance native int 
        Invoke(native int baton) runtime managed
  {
  } // end of method MyDelegate::Invoke

...

} // end of class MyDelegate
 * 
 * Into this:
 * 
.class public auto ansi sealed MyDelegate
    extends [mscorlib]System.MulticastDelegate
{
  .method public hidebysig specialname rtspecialname 
       instance void  .ctor(object 'object',
                        native int 'method') runtime managed
  {
  } // end of method MyDelegate::.ctor

  .method public hidebysig virtual instance native int
          modopt([mscorlib]System.Runtime.CompilerServices.CallConvCdecl)
          Invoke(int32 cb) runtime managed
  {
  } // end of method MyDelegate::Invoke

...

} // end of class MyDelegate
*/

namespace Softec.Applications
{
	/// <summary>
	/// HackCallConv application main class
	/// </summary>
	class HackCallConv
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Regex FindCallConvAttribute = new Regex(@"\.custom instance void \[CallConvAttribute\]Softec.CallConv(.*?)Attribute::\.ctor\(\) = \([^)]*\)", RegexOptions.Compiled);
			Regex FindInvokeMethod = new Regex(@"([ ]*)Invoke\(.*", RegexOptions.Compiled);

			string callingConvention = string.Empty;
			bool newLine = false;

			string line;
			while ((line = Console.ReadLine()) != null)
			{
				if (line == "" )
				{
					if( !newLine )
					{
						newLine = true;
						continue;
					}
					newLine = false;
				}
				else
				{
					newLine = false;

					if (line == ".assembly extern CallConvAttribute")
					{
						while ((line = Console.ReadLine()) != null && line != "}") {}
						continue;
					}

					Match m = FindCallConvAttribute.Match(line);
					if (m.Success)
					{
						callingConvention = m.Groups[1].Value;
						continue;
					}

					if (callingConvention != string.Empty)
					{
						m = FindInvokeMethod.Match(line);
						if (m.Success)
						{
							Console.WriteLine("{0}modopt([mscorlib]System.Runtime.CompilerServices.CallConv{1})", m.Groups[1].Value, callingConvention);
							callingConvention = string.Empty;
						}
					}
				}
				Console.WriteLine(line);
			}
		}
	}
}
