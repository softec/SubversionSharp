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
    public unsafe class SvnWcStatus
    {
	 	public delegate void Func(IntPtr baton, AprString path, SvnWcStatus status);
	 	
		public enum Kind {
			None = 1,
			Unversioned,
			Normal,
			Added,
			Missing,
			Deleted,
			Replaced,
			Modified,
			Merged,
			Conflicted,
			Ignored,
			Obstructed,
			External,
			Incomplete
		}

        private svn_wc_status_t *mStatus;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_wc_status_t
		{
			public IntPtr entry;
			public int text_status;
			public int prop_status;
			public int locked;
			public int copied;
			public int switched;
			public int repos_text_status;
			public int repos_prop_status;
   		} 

        #region Generic embedding functions of an IntPtr
        private SvnWcStatus(svn_wc_status_t *ptr)
        {
            mStatus = ptr;
        }
        
        public SvnWcStatus(IntPtr ptr)
        {
            mStatus = (svn_wc_status_t *) ptr.ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mStatus == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mStatus = null;
        }

        public static implicit operator IntPtr(SvnWcStatus status)
        {
            return new IntPtr(status.mStatus);
        }
        
        public static implicit operator SvnWcStatus(IntPtr ptr)
        {
            return new SvnWcStatus(ptr);
        }

        public override string ToString()
        {
            return("[svn_wc_status_t:"+(new IntPtr(mStatus)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public SvnWcEntry Entry
		{
			get
			{
				CheckPtr();
				return(new SvnWcEntry(mStatus->entry));
			}
		}
		 
		public Kind TextStatus
		{
			get
			{
				CheckPtr();
				return((Kind)mStatus->text_status);
			}
		} 

		public Kind PropStatus
		{
			get
			{
				CheckPtr();
				return((Kind)mStatus->prop_status);
			}
		}
		 
		public bool Locked
		{
			get
			{
				CheckPtr();
				return(mStatus->locked != 0);
			}
		}

		public bool Copied
		{
			get
			{
				CheckPtr();
				return(mStatus->copied != 0);
			}
		}

		public bool Switched
		{
			get
			{
				CheckPtr();
				return(mStatus->switched != 0);
			}
		}

		public Kind ReposTextStatus
		{
			get
			{
				CheckPtr();
				return((Kind)mStatus->repos_text_status);
			}
		} 

		public Kind ReposPropStatus
		{
			get
			{
				CheckPtr();
				return((Kind)mStatus->repos_prop_status);
			}
		}
		#endregion
	}
}