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
    public unsafe class SvnClientCommitInfo
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
