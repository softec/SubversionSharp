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
    public unsafe class SvnClientCommitItem : IAprUnmanaged
    {
        private svn_client_commit_item_t *mCommitItem;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_client_commit_item_t
		{
			public IntPtr path;
			public int	kind;
			public IntPtr url;
			public int revision;
			public IntPtr copyfrom_url;
			public byte state_flags;
			public IntPtr wcprop_changes;
   		}

        #region Generic embedding functions of an IntPtr
        private SvnClientCommitItem(svn_client_commit_item_t *ptr)
        {
            mCommitItem = ptr;
        }
        
        public SvnClientCommitItem(IntPtr ptr)
        {
            mCommitItem = (svn_client_commit_item_t *) ptr.ToPointer();
        }

        public bool IsNull
        {
        	get
        	{
            	return( mCommitItem == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mCommitItem = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mCommitItem);
        }
        
        public static implicit operator IntPtr(SvnClientCommitItem clientCommit)
        {
            return new IntPtr(clientCommit.mCommitItem);
        }
        
        public static implicit operator SvnClientCommitItem(IntPtr ptr)
        {
            return new SvnClientCommitItem(ptr);
        }

        public override string ToString()
        {
            return("[svn_client_commit_item_t:"+(new IntPtr(mCommitItem)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public AprString Path
		{
			get
			{
				CheckPtr();
				return(new AprString(mCommitItem->path));
			}
		}
		
		public Svn.NodeKind Kind
		{
			get
			{
				CheckPtr();
				return((Svn.NodeKind)mCommitItem->kind);
			}
		} 

		public AprString Url
		{
			get
			{
				CheckPtr();
				return(new AprString(mCommitItem->url));
			}
		}

		public int Revision
		{
			get
			{
				CheckPtr();
				return(mCommitItem->revision);
			}
		} 

		public AprString CopyFromUrl
		{
			get
			{
				CheckPtr();
				return(new AprString(mCommitItem->copyfrom_url));
			}
		}

		public AprArray WCPropChanges
		{
			get
			{
				CheckPtr();
				return(new AprArray(mCommitItem->wcprop_changes));
			}
		}
		#endregion
	}
}
