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
    public unsafe class SvnClientPropListItem : IAprUnmanaged
    {
        private svn_client_proplist_item_t *mPropList;

        [StructLayout( LayoutKind.Sequential )]
		private struct svn_client_proplist_item_t
		{
			public IntPtr node_name;
			public IntPtr prop_hash;
   		}

        #region Generic embedding functions of an IntPtr
        private SvnClientPropListItem(svn_client_proplist_item_t *ptr)
        {
            mPropList = ptr;
        }
        
        public SvnClientPropListItem(IntPtr ptr)
        {
            mPropList = (svn_client_proplist_item_t *) ptr.ToPointer();
        }

        public bool IsNull
        {
        	get
        	{
            	return( mPropList == null );
            }
        }

        private void CheckPtr()
        {
            if( IsNull )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mPropList = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mPropList);
        }
        
        public static implicit operator IntPtr(SvnClientPropListItem clientPropList)
        {
            return new IntPtr(clientPropList.mPropList);
        }
        
        public static implicit operator SvnClientPropListItem(IntPtr ptr)
        {
            return new SvnClientPropListItem(ptr);
        }

        public override string ToString()
        {
            return("[svn_client_proplist_item_t:"+(new IntPtr(mPropList)).ToInt32().ToString("X")+"]");
        }
        #endregion
		
		#region Properties wrappers
		public SvnStringBuf NodeName
		{
			get
			{
				CheckPtr();
				return(new SvnStringBuf(mPropList->node_name));
			}
		}

		public AprHash PropHash
		{
			get
			{
				CheckPtr();
				return(new AprHash(mPropList->prop_hash));
			}
		}
		#endregion
	}
}
