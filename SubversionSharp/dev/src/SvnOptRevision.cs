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
    public class SvnRevision
    {
		Svn.Revision mKind;
		int mNumber;
		long mDate;
		
		#region Ctors
		public SvnRevision(Svn.Revision kind)
		{
			mKind = kind;
			mNumber = 0;
			mDate = 0;
		}

		public SvnRevision(int number)
		{
			mKind = Svn.Revision.Number;
			mNumber = number;
			mDate = 0;
		}

		public SvnRevision(long date)
		{
			mKind = Svn.Revision.Date;
			mNumber = 0;
			mDate = date;
		}
		
		public SvnRevision(SvnOptRevision rev)
		{
	        mKind = rev.Kind;
            if(mKind == Svn.Revision.Number)
            	mNumber = rev.Number;
            else if(mKind == Svn.Revision.Date)
            	mDate = rev.Date;
		}
		#endregion

		#region Operators
        public static implicit operator SvnRevision(Svn.Revision revision)
        {
            return new SvnRevision(revision);
        }
        
        public static implicit operator SvnRevision(int revision)
        {
            return new SvnRevision(revision);
        }
        
        public static implicit operator SvnRevision(long revision)
        {
            return new SvnRevision(revision);
        }
        
        public static implicit operator SvnRevision(SvnOptRevision revision)
        {
            return new SvnRevision(revision);
        }
		#endregion
		
		#region Methods
		public SvnOptRevision ToSvnOpt(AprPool pool)
		{
			return(new SvnOptRevision(this,pool));
		}

		public SvnOptRevision ToSvnOpt(out GCHandle handle)
		{
			return(new SvnOptRevision(this, out handle));
		}
		#endregion

		#region Properties
		public Svn.Revision Kind
		{
			get
			{
				return(mKind);
			}
			set
			{
				mKind = value;
			}
		} 

		public int Number
		{
			get
			{
	            if( mKind != Svn.Revision.Number )
	                throw new AprNullReferenceException(); 
				return(mNumber);
			}
			set
			{
				mKind = Svn.Revision.Number;
				mNumber = value;
			}
		}

		public long Date
		{
			get
			{
	            if( mKind != Svn.Revision.Date )
	                throw new AprNullReferenceException(); 
				return(mDate);
			}
			set
			{
				mKind = Svn.Revision.Date;
				mDate = value;
			}
		}

		public SvnOptRevision Revision
		{
			set
			{
		        mKind = value.Kind;
	            if(mKind == Svn.Revision.Number)
	            	mNumber = value.Number;
	            else if(mKind == Svn.Revision.Date)
	            	mDate = value.Date;
			}
		}
		#endregion
	}
	
    public unsafe struct SvnOptRevision : IAprUnmanaged
    {
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
        
        public SvnOptRevision(AprPool pool)
        {
            mOptRevision = (svn_opt_revision_t *) pool.CAlloc(sizeof(svn_opt_revision_t));
        }
        
        public SvnOptRevision(Svn.Revision revKind, AprPool pool)
        {
            mOptRevision = (svn_opt_revision_t *) pool.CAlloc(sizeof(svn_opt_revision_t));
            Kind = revKind;
        }

        public SvnOptRevision(int revNum, AprPool pool)
        {
            mOptRevision = (svn_opt_revision_t *) pool.CAlloc(sizeof(svn_opt_revision_t));
            Number = revNum;
        }
        
        public SvnOptRevision(long revDate, AprPool pool)
        {
            mOptRevision = (svn_opt_revision_t *) pool.CAlloc(sizeof(svn_opt_revision_t));
            Date = revDate;
        }

        public SvnOptRevision(SvnRevision rev, AprPool pool)
        {
            mOptRevision = (svn_opt_revision_t *) pool.CAlloc(sizeof(svn_opt_revision_t));
            Revision = rev;
        }
        
        public SvnOptRevision(out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
        }
        
        public SvnOptRevision(Svn.Revision revKind, out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
            Kind = revKind;
        }
        
        public SvnOptRevision(int revNum, out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
            Number = revNum;
        }
        
        public SvnOptRevision(long revDate, out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
            Date = revDate;
        }
        
        public SvnOptRevision(SvnRevision rev, out GCHandle handle)
        {
            handle = GCHandle.Alloc(new svn_opt_revision_t(),GCHandleType.Pinned);
            mOptRevision = (svn_opt_revision_t *)handle.AddrOfPinnedObject().ToPointer();
            Revision = rev;
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

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mOptRevision);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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
		public Svn.Revision Kind
		{
			get
			{
				CheckPtr();
				return((Svn.Revision)mOptRevision->kind);
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
	            if( (Svn.Revision) mOptRevision->kind != Svn.Revision.Number )
	                throw new AprNullReferenceException(); 
				return(mOptRevision->number);
			}
			set
			{
				CheckPtr();
	            mOptRevision->kind = (int) Revision.Number; 
				mOptRevision->number = value;
			}
		}

		public long Date
		{
			get
			{
				CheckPtr();
	            if((Svn.Revision)mOptRevision->kind != Svn.Revision.Date )
	                throw new AprNullReferenceException(); 
				return(mOptRevision->date);
			}
			set
			{
				CheckPtr();
	            mOptRevision->kind = (int) Revision.Date; 
				mOptRevision->date = value;
			}
		}
		
		public SvnRevision Revision
		{
			get
			{
				CheckPtr();
				return(new SvnRevision(this));
			}
			set
			{
				CheckPtr();
		        mOptRevision->kind = (int) value.Kind;
	            if((Svn.Revision)mOptRevision->kind == Svn.Revision.Number)
	            	mOptRevision->number = value.Number;
	            else if((Svn.Revision)mOptRevision->kind == Svn.Revision.Date)
	            	mOptRevision->date = value.Date;
			}
		}
		#endregion
	}
}
