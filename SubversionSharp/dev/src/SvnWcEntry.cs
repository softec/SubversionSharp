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
    public unsafe class SvnWcEntry : IAprUnmanaged
    {
		public enum WcSchedule {
			Normal,
			Add,
			Delete,
			Replace
		}

        private svn_wc_entry_t *mEntry;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_wc_entry_t
		{
			public IntPtr name;
			public int revision;
			public IntPtr url;
			public IntPtr repos;
			public IntPtr uuid;
			public int kind;
			public int schedule;
			public int copied;
			public int deleted;
			public int absent;
			public int incomplete;
			public IntPtr copyfrom_url;
			public int copyfrom_rev;
			public IntPtr conflict_old;
			public IntPtr conflict_new;
			public IntPtr conflict_wrk;
			public IntPtr prejfile;
			public long text_time;
			public long prop_time;
			public IntPtr checksum;
			public int cmt_rev;
			public long cmt_date;
			public IntPtr cmt_author;
   		} 

        #region Generic embedding functions of an IntPtr
        private SvnWcEntry(svn_wc_entry_t *ptr)
        {
            mEntry = ptr;
        }
        
        public SvnWcEntry(IntPtr ptr)
        {
            mEntry = (svn_wc_entry_t *) ptr.ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mEntry == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mEntry = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mEntry);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnWcEntry entry)
        {
            return new IntPtr(entry.mEntry);
        }
        
        public static implicit operator SvnWcEntry(IntPtr ptr)
        {
            return new SvnWcEntry(ptr);
        }

        public override string ToString()
        {
            return("[svn_wc_entry_t:"+(new IntPtr(mEntry)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public SvnPath Name
		{
			get
			{
				CheckPtr();
				return(mEntry->name);
			}
		}
		 
		public int Revision
		{
			get
			{
				CheckPtr();
				return(mEntry->revision);
			}
		}
		
		public SvnUrl Url
		{
			get
			{
				CheckPtr();
				return(mEntry->url);
			}
		}
		 
		public SvnUrl Repos
		{
			get
			{
				CheckPtr();
				return(mEntry->repos);
			}
		}
		 
		public AprString Uuid
		{
			get
			{
				CheckPtr();
				return(mEntry->uuid);
			}
		}
		 
		public Svn.NodeKind Kind
		{
			get
			{
				CheckPtr();
				return((Svn.NodeKind)mEntry->kind);
			}
		} 

		public WcSchedule Schedule
		{
			get
			{
				CheckPtr();
				return((WcSchedule)mEntry->schedule);
			}
		}
		 
		public bool Copied
		{
			get
			{
				CheckPtr();
				return(mEntry->copied != 0);
			}
		}

		public bool Deleted
		{
			get
			{
				CheckPtr();
				return(mEntry->deleted != 0);
			}
		}

		public bool Absent
		{
			get
			{
				CheckPtr();
				return(mEntry->absent != 0);
			}
		}

		public bool Incomplete
		{
			get
			{
				CheckPtr();
				return(mEntry->incomplete != 0);
			}
		}

		public SvnUrl CopyFromUrl
		{
			get
			{
				CheckPtr();
				return(mEntry->copyfrom_url);
			}
		}
		 
		public int CopyFromRevision
		{
			get
			{
				CheckPtr();
				return(mEntry->copyfrom_rev);
			}
		}
		
		public SvnPath ConflictOld
		{
			get
			{
				CheckPtr();
				return(mEntry->conflict_old);
			}
		}

		public SvnPath ConflictNew
		{
			get
			{
				CheckPtr();
				return(mEntry->conflict_new);
			}
		}
		
		public SvnPath ConflictWork
		{
			get
			{
				CheckPtr();
				return(mEntry->conflict_wrk);
			}
		}

		public SvnPath RejectFile
		{
			get
			{
				CheckPtr();
				return(mEntry->prejfile);
			}
		}

		public long TextTime
		{
			get
			{
				CheckPtr();
				return(mEntry->text_time);
			}
		}

		public long PropTime
		{
			get
			{
				CheckPtr();
				return(mEntry->prop_time);
			}
		}
		
		public AprString CheckSum
		{
			get
			{
				CheckPtr();
				return(mEntry->checksum);
			}
		}
		
		public int CommitRev
		{
			get
			{
				CheckPtr();
				return(mEntry->cmt_rev);
			}
		}

		public long CommitDate
		{
			get
			{
				CheckPtr();
				return(mEntry->cmt_date);
			}
		}

		public SvnData CommitAuthor
		{
			get
			{
				CheckPtr();
				return(mEntry->cmt_author);
			}
		}
		#endregion
	}
}
