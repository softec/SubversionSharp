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
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public class SvnWcNotify
    {
    	 public enum Action
    	 {
			Add = 0,
			Copy,
			Delete,
			Restore,
			Revert,
			FailedRevert,
			Resolved,
			Skip,
			UpdateDelete,
			UpdateAdd,
			UpdateUpdate,
			UpdateCompleted,
			UpdateExternal,
			StatusCompleted,
			StatusExternal,
			CommitModified,
			CommitAdded,
			CommitDeleted,
			Replaced,
			PostfixTxdelta,
			BlameRevision
    	 }
         
    	 public enum State
    	 {
			Inapplicable = 0,
			Unknown,
			Unchanged,
			Missing,
			Obstructed,
			Changed,
			Merged,
			Conflicted
    	 }

         public delegate void Func(IntPtr baton, AprString Path,  
        				 		   Action action, Svn.NodeKind kind,
        				 		   AprString mimeType, State contentState,
        				 		   State propState, int revNum);
	}
}