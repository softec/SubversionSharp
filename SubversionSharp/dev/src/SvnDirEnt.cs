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
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public unsafe class SvnDirEnt : IAprUnmanaged
    {
        private svn_dirent_t *mDirEnt;

        [StructLayout( LayoutKind.Sequential )]
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
		
		public AprString LastAuthor
		{
			get
			{
				CheckPtr();
				return(new AprString(mDirEnt->last_author));
			}
		}
		#endregion
	}
}
