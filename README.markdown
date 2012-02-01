# SubversionSharp

**SubversionSharp is a C# wrapper that fully covers the client API of Subversion 
SCM 1.0. Easy access to the Subversion API is provided without any compromise 
on fonctionality. This library is the starting point to easily integrate 
Subversion repositories in any .NET managed software. These C# bindings for 
Subversion has been written for portability and performances. This library is 
now available for both Mono/Linux and .NET/Windows environments.**

SVN# is the starting point to easily integrate Subversion repositories in any .NET 
managed software. These C# bindings for Subversion has been written for portability
and performances. This library is available for both Mono/Linux and .NET/Windows
environments. See Getting Started guide for more information.

**Note for .NET 2.0 users and above** : Our current sources are only adapted to
.NET 1.x, users of .NET 2.0 and above may be interested by a branch of our project, 
written by Toby Johnson to better support these latest .NET releases. Toby have set
up its own project named Svn.NET.

SubversionSharp depends on the following sub-projects, also available in this repository.

## AprSharp

AprSharp is a C# wrapper library around the Apache Runtime Library. It provides a
starting point to easily interface any .NET managed languages with libraries using 
`AprPool` for their memory management. AprSharp has been written to support functionality
required by SubversionSharp.

## CallConvHack

The main purpose of CallConvHack is to provide a working SubversionSharp to Windows
users. Subversion callbacks should use Cdecl calling convention. Under Windows, 
C# delegates default to Stdcall calling convention. Moreover, there is no way using
plain C# to change this behaviour (in .NET 1.0). This hack (based on several articles,
blogs and posts publicly available) patch IL code directly to fix the calling
convention of attribute-marked delegates. The originality of our implementation is 
both the external DLL used to implement the `CallConvCdeclAttribute` and the C# program 
that works as a filter to translate the standard output of ildasm.
