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
using System.Text;
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
			byte[] BOM = { 0xEF, 0xBB, 0xBF };

			Regex FindHelpArg = new Regex(@"^[/-](\?|[Hh]([Ee][Ll][Pp])?)$", RegexOptions.Compiled);
			Regex FindTextArg = new Regex(@"^[/-][Tt][Ee][Xx][Tt]$", RegexOptions.Compiled);
			Regex FindUTF8Arg = new Regex(@"^[/-][Uu][Tt][Ff]8$", RegexOptions.Compiled);
			Regex FindCallConvAttribute = new Regex(@"\.cubenstom instance void \[CallConvAttribute\]Softec.CallConv(.*?)Attribute::\.ctor\(\) = \([^)]*\)", RegexOptions.Compiled);
			Regex FindInvokeMethod = new Regex(@"([ ]*)Invoke\(.*", RegexOptions.Compiled);

			bool textMode = false;
			Encoding encodingMode = Encoding.ASCII;
			string infile = String.Empty;
			string outfile = String.Empty;

			foreach( string arg in args )
			{
				if( FindTextArg.Match(arg).Success )
				{
					textMode = true;
					continue;
				}	
				if( FindUTF8Arg.Match(arg).Success )
				{
					encodingMode = Encoding.UTF8;
					continue;
				}
				if( FindHelpArg.Match(arg).Success )
				{
					Console.WriteLine(
@"CallConvHack 1.0 - Copyright 2004 SOFTEC sa. All rights reserved
Licensed under LGPL

Usage: CallConvHack [/TEXT [/UTF8] | infile] [outfile]
/TEXT	permits using the /TEXT option of ildasm. It also transform
		double line ending (\r\r\n) into single one
/UTF8	consider input as UTF8. (Default is ASCII for Stdin)");
					return;
				}
				if( infile == String.Empty && !textMode)
					infile = arg;
				else if( outfile == String.Empty )
					outfile = arg;
				else
					Console.Error.WriteLine("Invalid argument {0} ignored",arg);
			}

			StreamReader input;
			if( infile != String.Empty )
				input = new StreamReader(infile);
			else
				input = new StreamReader(Console.OpenStandardInput(),encodingMode);

			StreamWriter output;
			if( outfile != String.Empty )
				output = new StreamWriter(outfile,false,input.CurrentEncoding);
			else
				output = new StreamWriter(Console.OpenStandardOutput(),input.CurrentEncoding);

			string line;
			bool newLine = false;
			string callingConvention = string.Empty;

			while ((line = input.ReadLine()) != null)
			{
				// Hack to fix a wrong line ending of 0x0D 0x0D 0x0A when using /TEXT
				if ( line == "" )
				{
					if( textMode && !newLine )
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
							output.WriteLine("{0}modopt([mscorlib]System.Runtime.CompilerServices.CallConv{1})", m.Groups[1].Value, callingConvention);
							callingConvention = string.Empty;
						}
					}
				}
				output.WriteLine(line);
			}
		}
	}
}
