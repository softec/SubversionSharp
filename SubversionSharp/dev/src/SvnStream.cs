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
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public struct SvnStream : IAprUnmanaged
    {
        public delegate SvnError ReadFunc(IntPtr baton, IntPtr buffer, ref int len);
        public delegate SvnError WriteFunc(IntPtr baton, IntPtr data, ref int len);
        public delegate SvnError CloseFunc(IntPtr baton);

        private IntPtr mStream;
        internal SvnDelegate mReadDelegate;
        internal SvnDelegate mWriteDelegate;
        internal SvnDelegate mCloseDelegate;

        #region Generic embedding functions of an IntPtr
        private SvnStream(IntPtr ptr)
        {
            mStream = ptr;
            mReadDelegate = null;
        	mWriteDelegate = null;
        	mCloseDelegate = null;
        }

        private SvnStream(IntPtr ptr, SvnDelegate read, SvnDelegate write, SvnDelegate close)
        {
            mStream = ptr;
            mReadDelegate = read;
        	mWriteDelegate = write;
        	mCloseDelegate = close;
        }
        
        public SvnStream(IntPtr baton, AprPool pool)
        {
        	mStream = Svn.svn_stream_create(baton, pool);
            mReadDelegate = null;
        	mWriteDelegate = null;
        	mCloseDelegate = null;
		}

        public bool IsNull
        {
        	get
        	{
            	return( mStream == IntPtr.Zero );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mStream = IntPtr.Zero;
        }

        public IntPtr ToIntPtr()
        {
            return mStream;
        }
        
        public static implicit operator IntPtr(SvnStream stream)
        {
            return stream.mStream;
        }
        
        public static implicit operator SvnStream(IntPtr ptr)
        {
            return new SvnStream(ptr);
        }

        public override string ToString()
        {
            return("[svn_stream_t:"+mStream.ToInt32().ToString("X")+"]");
        }
        #endregion
        
        #region Methods wrappers
        public static SvnStream Create(AprPool pool)
        {
	        return(new SvnStream(Svn.svn_stream_empty(pool)));
        }

        public static SvnStream Create(IntPtr baton, AprPool pool)
        {
            return(new SvnStream(baton,pool));
		}
		
        public static SvnStream Create(AprFile file, AprPool pool)
        {
	        return(new SvnStream(Svn.svn_stream_from_aprfile(file,pool)));
        }
        
        public static SvnStream Compress(SvnStream stream, AprPool pool)
        {
	        return(new SvnStream(Svn.svn_stream_compressed(stream,pool)));
        }
        
        public static SvnStream Stdout(AprPool pool)
        {
            IntPtr ptr;
            SvnError err = Svn.svn_stream_for_stdout(out ptr, pool);
            if( !err.IsNoError )
                throw new SvnException(err);
            return(ptr);
        }

        #endregion

        #region Properties wrappers
        public IntPtr Baton
        {
        	set
        	{
        		Svn.svn_stream_set_baton(mStream, value);
        	}
        }

        public SvnDelegate ReadDelegate
        {
        	get
        	{
        		return(mReadDelegate);
        	}
        	set
        	{
        		mReadDelegate = value;
        		Svn.svn_stream_set_read(mStream, (Svn.svn_read_fn_t)value.Wrapper);
        	}
        }

        public SvnDelegate WriteDelegate
        {
        	get
        	{
        		return(mWriteDelegate);
        	}
        	set
        	{
        		mWriteDelegate = value;
        		Svn.svn_stream_set_write(mStream, (Svn.svn_write_fn_t)value.Wrapper);
        	}
        }
        
        public SvnDelegate CloseDelegate
        {
        	get
        	{
        		return(mCloseDelegate);
        	}
        	set
        	{
        		mCloseDelegate = value;
        		Svn.svn_stream_set_close(mStream, (Svn.svn_close_fn_t)value.Wrapper);
        	}
        }
        #endregion
	}
}