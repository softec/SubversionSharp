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
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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
		public SvnPath Path
		{
			get
			{
				CheckPtr();
				return(new SvnPath(mCommitItem->path));
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

		public SvnUrl Url
		{
			get
			{
				CheckPtr();
				return(mCommitItem->url);
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

		public SvnUrl CopyFromUrl
		{
			get
			{
				CheckPtr();
				return(mCommitItem->copyfrom_url);
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
