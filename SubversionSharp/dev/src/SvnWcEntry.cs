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
		public AprString Name
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->name));
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
		
		public AprString Url
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->url));
			}
		}
		 
		public AprString Repos
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->repos));
			}
		}
		 
		public AprString Uuid
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->uuid));
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

		public AprString CopyFromUrl
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->copyfrom_url));
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
		
		public AprString ConflictOld
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->conflict_old));
			}
		}

		public AprString ConflictNew
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->conflict_new));
			}
		}
		
		public AprString ConflictWork
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->conflict_wrk));
			}
		}

		public AprString RejectFile
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->prejfile));
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
				return(new AprString(mEntry->checksum));
			}
		}
		
		public int CmtRev
		{
			get
			{
				CheckPtr();
				return(mEntry->cmt_rev);
			}
		}

		public long CmtDate
		{
			get
			{
				CheckPtr();
				return(mEntry->cmt_date);
			}
		}

		public AprString CmtAuthor
		{
			get
			{
				CheckPtr();
				return(new AprString(mEntry->cmt_author));
			}
		}
		#endregion
	}
}
