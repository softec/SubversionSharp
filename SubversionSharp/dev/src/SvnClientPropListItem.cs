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
    public unsafe class SvnClientPropListItem : IAprUnmanaged
    {
        private svn_client_proplist_item_t *mPropList;

        [StructLayout( LayoutKind.Sequential, Pack=4 )]
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
        
		public bool ReferenceEquals(IAprUnmanaged obj)
		{
			return(obj.ToIntPtr() == ToIntPtr());
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
