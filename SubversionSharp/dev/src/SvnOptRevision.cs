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
    public unsafe class SvnOptRevision
    {
		public enum RevisionKind {
			Unspecified,
			Number,
			Date,
			Committed,
			Previous,
			Base,
			Working,
			Head
		}

        private svn_opt_revision_t *mOptRevision;

        [StructLayout( LayoutKind.Explicit )]
		private struct svn_opt_revision_t
		{
  			[FieldOffset(0)]public int kind;
     		[FieldOffset(4)]public int number;
     		[FieldOffset(4)]public long date;
   		} 

        #region Generic embedding functions of an IntPtr
        private SvnOptRevision(svn_opt_revision_t *ptr)
        {
            mOptRevision = ptr;
        }
        
        public SvnOptRevision(IntPtr ptr)
        {
            mOptRevision = (svn_opt_revision_t *) ptr.ToPointer();
        }
        
        public SvnOptRevision(out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mOptRevision == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mOptRevision = null;
        }

        public static implicit operator IntPtr(SvnOptRevision revision)
        {
            return new IntPtr(revision.mOptRevision);
        }
        
        public static implicit operator SvnOptRevision(IntPtr ptr)
        {
            return new SvnOptRevision(ptr);
        }

        public override string ToString()
        {
            return("[svn_opt_revision_t:"+(new IntPtr(mOptRevision)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
        public static SvnOptRevision Alloc(AprPool pool)
        {
            return(new SvnOptRevision((svn_opt_revision_t *)pool.CAlloc(sizeof(svn_opt_revision_t))));
        }

        public static SvnOptRevision Alloc(out GCHandle handle)
        {
            return(new SvnOptRevision(out handle));
        }
        #endregion

		
		#region Properties wrappers
		public RevisionKind Kind
		{
			get
			{
				CheckPtr();
				return((RevisionKind)mOptRevision->kind);
			}
			set
			{
				CheckPtr();
				mOptRevision->kind = (int) value;
			}
		} 

		public int Number
		{
			get
			{
				CheckPtr();
	            if( (RevisionKind) mOptRevision->kind != RevisionKind.Number )
	                throw new AprNullReferenceException(); 
				return(mOptRevision->number);
			}
			set
			{
				CheckPtr();
	            mOptRevision->kind = (int) RevisionKind.Number; 
				mOptRevision->number = value;
			}
		}

		public long Date
		{
			get
			{
				CheckPtr();
	            if( (RevisionKind) mOptRevision->kind != RevisionKind.Date )
	                throw new AprNullReferenceException(); 
				return(mOptRevision->date);
			}
			set
			{
				CheckPtr();
	            mOptRevision->kind = (int) RevisionKind.Date; 
				mOptRevision->date = value;
			}
		}
		#endregion
	}
}
