//  SubversionSharp, a wrapper library around the Subversion client API
//  Copyright (C) 2004 SOFTEC sa.
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
//
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public unsafe class SvnWcStatus : IAprUnmanaged
    {
	 	public delegate void Func(IntPtr baton, SvnPath path, SvnWcStatus status);
	 	
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

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mStatus);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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
