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
    public struct AprFile : IAprUnmanaged
    {
    	[Flags]
    	public enum Flags
    	{
    		Read			= 0x00001,
			Write			= 0x00002,
			Create			= 0x00004,
			Append			= 0x00008,
			Truncate		= 0x00010,
			Binary			= 0x00020,
			Exclusive		= 0x00040,
			Buffered		= 0x00080,
			DelayOnClose	= 0x00100,
			XThread			= 0x00200,
			ShareLock		= 0x00400,
			FileNoCleanUp   = 0x00800,
			SendFileEnabled = 0x01000
    	}
    	
    	[Flags]
    	public enum Perms
    	{
    		SetUserId	 = 0x8000,
			UserRead	 = 0x0400,
			UserWrite	 = 0x0200,
			UserExecute	 = 0x0100,
			SetGroupId	 = 0x4000,
			GroupRead    = 0x0040,
			GroupWrite   = 0x0020,
			GroupExecute = 0x0010,
			WorldSticky  = 0x2000,
			WorldRead	 = 0x0004,
			WorldWrite	 = 0x0002,
			WorldExecute = 0x0001,
			OSDefault    = 0x0FFF,
			SourcePerms  = 0x1000
    	}
    	
    	public enum Std
    	{
    		In,
    		Out,
    		Err
    	}
    
        IntPtr mFile;

        #region Generic embedding functions of an IntPtr
        public AprFile(IntPtr ptr)
        {
            mFile = ptr;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mFile == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mFile = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mFile;
        }

		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(AprFile file)
        {
            return file.mFile;
        }
        
        public static implicit operator AprFile(IntPtr ptr)
        {
            return new AprFile(ptr);
        }

        public override string ToString()
        {
            return("[apr_file_t:"+mFile.ToInt32().ToString("X")+"]");
        }
        #endregion

        #region Wrapper methods
        public static AprFile Open(string fname, Flags flag, Perms perm, AprPool pool)
        {
            IntPtr ptr;
            
            Debug.Write(String.Format("apr_file_open({0},{1},{2},{3})...",fname,flag,perm,pool));
            int res = Apr.apr_file_open(out ptr, fname, (int)flag, (int)perm, pool);
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public static AprFile Open(Std stream, AprPool pool)
        {
            IntPtr ptr = IntPtr.Zero;
            int res = 0;
            
            switch(stream)
            {
            	case Std.In:
            		Debug.Write(String.Format("apr_file_open_stdin({0})...",pool));
            		res = Apr.apr_file_open_stdin(out ptr, pool);
            		break;
            	case Std.Out:
            		Debug.Write(String.Format("apr_file_open_stdout({0})...",pool));
            		res = Apr.apr_file_open_stdout(out ptr, pool);
            		break;
            	case Std.Err:
            		Debug.Write(String.Format("apr_file_open_stderr({0})...",pool));
            		res = Apr.apr_file_open_stderr(out ptr, pool);
            		break;
            }
            if(res != 0 )
                throw new AprException(res);
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }
                                
        public void Close()
        {
        	CheckPtr();
            Debug.Write(String.Format("apr_file_close({0})...",this));
            int res = Apr.apr_file_close(mFile);
            if(res != 0 )
                throw new AprException(res);
            mFile = IntPtr.Zero;
        }
        #endregion
    }
}