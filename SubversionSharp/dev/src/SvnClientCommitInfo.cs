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
    public unsafe class SvnClientCommitInfo : IAprUnmanaged
    {
        private svn_client_commit_info_t *mCommitInfo;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_client_commit_info_t
		{
     		public int revision;
     		public IntPtr date;
     		public IntPtr author;
   		}

        #region Generic embedding functions of an IntPtr
        private SvnClientCommitInfo(svn_client_commit_info_t *ptr)
        {
            mCommitInfo = ptr;
        }
        
        public SvnClientCommitInfo(IntPtr ptr)
        {
            mCommitInfo = (svn_client_commit_info_t *) ptr.ToPointer();
        }

        public bool IsNull
        {
        	get
        	{
            	return( mCommitInfo == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mCommitInfo = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mCommitInfo);
        }
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
		}
		
        public static implicit operator IntPtr(SvnClientCommitInfo commitInfo)
        {
            return new IntPtr(commitInfo.mCommitInfo);
        }
        
        public static implicit operator SvnClientCommitInfo(IntPtr ptr)
        {
            return new SvnClientCommitInfo(ptr);
        }

        public override string ToString()
        {
            return("[svn_client_commit_info_t:"+(new IntPtr(mCommitInfo)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public int Revision
		{
			get
			{
				CheckPtr();
				return(mCommitInfo->revision);
			}
		} 

		public AprString Date
		{
			get
			{
				CheckPtr();
				return(new AprString(mCommitInfo->date));
			}
		}

		public AprString Author
		{
			get
			{
				CheckPtr();
				return(new AprString(mCommitInfo->date));
			}
		}
		#endregion
	}
}
