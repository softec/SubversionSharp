//  SubversionSharp, a wrapper library around the Subversion client API
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
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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