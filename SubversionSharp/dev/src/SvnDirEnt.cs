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
using System.Runtime.InteropServices;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public unsafe class SvnDirEnt : IAprUnmanaged
    {
        private svn_dirent_t *mDirEnt;

        [StructLayout( LayoutKind.Sequential, Pack=4 )]
		private struct svn_dirent_t
		{
			public int kind;
			public long size;
			public int has_props;
			public int created_rev;
			public long time;
			public IntPtr last_author;
   		} 

        #region Generic embedding functions of an IntPtr
        private SvnDirEnt(svn_dirent_t *ptr)
        {
            mDirEnt = ptr;
        }
        
        public SvnDirEnt(IntPtr ptr)
        {
            mDirEnt = (svn_dirent_t *) ptr.ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mDirEnt == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mDirEnt = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mDirEnt);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnDirEnt entry)
        {
            return new IntPtr(entry.mDirEnt);
        }
        
        public static implicit operator SvnDirEnt(IntPtr ptr)
        {
            return new SvnDirEnt(ptr);
        }

        public override string ToString()
        {
            return("[svn_dirent_t:"+(new IntPtr(mDirEnt)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public Svn.NodeKind Kind
		{
			get
			{
				CheckPtr();
				return((Svn.NodeKind)mDirEnt->kind);
			}
		} 

		public long Size
		{
			get
			{
				CheckPtr();
				return(mDirEnt->size);
			}
		}
		
		public bool HasProps
		{
			get
			{
				CheckPtr();
				return(mDirEnt->has_props != 0);
			}
		}
		
		public int CreationRevision
		{
			get
			{
				CheckPtr();
				return(mDirEnt->created_rev);
			}
		}
		
		public long Time
		{
			get
			{
				CheckPtr();
				return(mDirEnt->time);
			}
		}
		
		public SvnData LastAuthor
		{
			get
			{
				CheckPtr();
				return(mDirEnt->last_author);
			}
		}
		#endregion
	}
}
