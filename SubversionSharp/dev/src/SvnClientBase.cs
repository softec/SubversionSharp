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
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public class SvnClientBase
    {
    	public delegate SvnError GetCommitLog(out AprString logMessage, out SvnPath tmpFile,
         						 		   	  AprArray commitItems, IntPtr baton,
         								      AprPool pool);

 		public delegate SvnError LogMessageReceiver(IntPtr baton, AprHash changed_paths, 
 													int revision, AprString author,
 													AprString date, AprString message,
 													AprPool pool);

 		public delegate SvnError BlameReceiver(IntPtr baton, long line_no, int revision, 
 											   AprString author, AprString date, AprString line, 
 											   AprPool pool);
         								                								                								       
		public static int Checkout(SvnUrl url, SvnPath path, 
								   SvnOptRevision revision, 
								   bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_checkout({0},{1},{2},{3},{4},{5})...",url,path,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_checkout(out rev, url, path, 
												   revision, 
												   (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		
		public static int Update(SvnPath path, 
								 SvnOptRevision revision, 
								 bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_update({0},{1},{2},{3},{4})...",path,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_update(out rev, path, 
												 revision,
												 (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static int Switch(SvnPath path, SvnUrl url, 
								 SvnOptRevision revision, 
								 bool recurse, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_switch({0},{1},{2},{3},{4},{5})...",path,url,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_switch(out rev, path, url, 
												 revision, 
												 (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static void Add(SvnPath path,
							   bool recurse, 
							   SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_add({0},{1},{2},{3},{4})",path,recurse,ctx,pool));
			SvnError err = Svn.svn_client_add(path, 
											  (recurse ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static SvnClientCommitInfo Mkdir(AprArray paths,  
							   					SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_mkdir({0},{1},{2})...",paths,ctx,pool));
			SvnError err = Svn.svn_client_mkdir(out commitInfo, paths, ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
		
		public static SvnClientCommitInfo Delete(AprArray paths, bool force,
							   					 SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_delete({0},{1},{2},{3})...",paths,force,ctx,pool));
			SvnError err = Svn.svn_client_delete(out commitInfo, paths, (force ? 1 : 0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
		
		public static SvnClientCommitInfo Import(SvnPath path, SvnUrl url, bool nonrecursive,  
							   					 SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_import({0},{1},{2},{3},{4})...",path,url,nonrecursive,ctx,pool));
			SvnError err = Svn.svn_client_import(out commitInfo, path, url, (nonrecursive ? 1 : 0), 
												 ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
		
		public static SvnClientCommitInfo Commit(AprArray targets, bool nonrecursive,
							   					 SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_commit({0},{1},{2},{3})...",targets,nonrecursive,ctx,pool));
			SvnError err = Svn.svn_client_commit(out commitInfo, targets, (nonrecursive ? 1 : 0),
												 ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
		
		public static int Status(SvnPath path,
								 SvnOptRevision revision,
								 SvnWcStatus.Func statusFunc, IntPtr statusBaton,
								 bool descend, bool getAll, bool update, bool noIgnore,
			   					 SvnClientContext ctx, AprPool pool)
		{
			int rev;
			SvnDelegate statusDelegate = new SvnDelegate(statusFunc);
			Debug.Write(String.Format("svn_client_status({0},{1},{2},{3},{4:X},{5},{6},{7},{8},{9})...",path,revision,statusFunc.Method.Name,statusBaton.ToInt32(),descend,getAll,update,noIgnore,ctx,pool));
			SvnError err = Svn.svn_client_status(out rev, path, revision,
												 (Svn.svn_wc_status_func_t) statusDelegate.Wrapper,
												 statusBaton,
												 (descend ? 1 : 0), (getAll ? 1 : 0),
												 (update ? 1 : 0), (noIgnore ? 1 : 0),
												 ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			if( update )
				return(rev);
			else
				return(-1);
		}
		
		public static void Log(AprArray targets,
							   SvnOptRevision start, SvnOptRevision end,
							   bool discoverChangedPaths, bool strictNodeHistory,
							   LogMessageReceiver receiver, IntPtr baton,
							   SvnClientContext ctx, AprPool pool)
		{
			SvnDelegate receiverDelegate = new SvnDelegate(receiver);
			Debug.WriteLine(String.Format("svn_client_log({0},{1},{2},{3},{4},{5},{6:X},{7},{8})",targets,start,end,discoverChangedPaths,strictNodeHistory,receiver.Method.Name,baton.ToInt32(),ctx,pool));
			SvnError err = Svn.svn_client_log(targets, start, end,
											  (discoverChangedPaths ? 1 :0),
											  (strictNodeHistory ? 1 :0),
											  (Svn.svn_log_message_receiver_t)receiverDelegate.Wrapper,
											  baton,
											  ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Blame(SvnPath pathOrUrl,
								 SvnOptRevision start, SvnOptRevision end, 
								 BlameReceiver receiver, IntPtr baton,
							     SvnClientContext ctx, AprPool pool)
		{
			InternalBlame(pathOrUrl, start, end, receiver, baton, ctx, pool);
		}
		
		public static void Blame(SvnUrl pathOrUrl,
								 SvnOptRevision start, SvnOptRevision end, 
								 BlameReceiver receiver, IntPtr baton,
							     SvnClientContext ctx, AprPool pool)
		{
			InternalBlame(pathOrUrl, start, end, receiver, baton, ctx, pool);
		}
		
		protected static void InternalBlame(IAprUnmanaged pathOrUrl,
								 		  SvnOptRevision start, SvnOptRevision end, 
										  BlameReceiver receiver, IntPtr baton,
										  SvnClientContext ctx, AprPool pool)
		{
			SvnDelegate receiverDelegate = new SvnDelegate(receiver);
			Debug.WriteLine(String.Format("svn_client_blame({0},{1},{2},{3},{4:X},{5},{6})",pathOrUrl,start,end,receiver.Method.Name,baton.ToInt32(),ctx,pool));
			SvnError err = Svn.svn_client_blame(pathOrUrl.ToIntPtr(), start, end,
											    (Svn.svn_client_blame_receiver_t)receiverDelegate.Wrapper,
											    baton,
											    ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Diff(AprArray diffOptions,
								SvnPath path1, SvnOptRevision revision1,
								SvnPath path2, SvnOptRevision revision2,
								bool recurse, bool ignoreAncestry, bool noDiffDeleted,
								AprFile outFile, AprFile errFile,  
							    SvnClientContext ctx, AprPool pool)
		{
			InternalDiff(diffOptions, path1, revision1, path2, revision2,
						 recurse, ignoreAncestry, noDiffDeleted, outFile, errFile, ctx, pool);
		}
		
		public static void Diff(AprArray diffOptions,
								SvnUrl path1, SvnOptRevision revision1,
								SvnPath path2, SvnOptRevision revision2,
								bool recurse, bool ignoreAncestry, bool noDiffDeleted,
								AprFile outFile, AprFile errFile,  
							    SvnClientContext ctx, AprPool pool)
		{
			InternalDiff(diffOptions, path1, revision1, path2, revision2,
						 recurse, ignoreAncestry, noDiffDeleted, outFile, errFile, ctx, pool);
		}
		
		public static void Diff(AprArray diffOptions,
								SvnPath path1, SvnOptRevision revision1,
								SvnUrl path2, SvnOptRevision revision2,
								bool recurse, bool ignoreAncestry, bool noDiffDeleted,
								AprFile outFile, AprFile errFile,  
							    SvnClientContext ctx, AprPool pool)
		{
			InternalDiff(diffOptions, path1, revision1, path2, revision2,
						 recurse, ignoreAncestry, noDiffDeleted, outFile, errFile, ctx, pool);
		}
		
		public static void Diff(AprArray diffOptions,
								SvnUrl path1, SvnOptRevision revision1,
								SvnUrl path2, SvnOptRevision revision2,
								bool recurse, bool ignoreAncestry, bool noDiffDeleted,
								AprFile outFile, AprFile errFile,  
							    SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_diff({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",diffOptions,path1,revision1,path2,revision2,recurse,ignoreAncestry,noDiffDeleted,outFile,errFile,ctx,pool));
			SvnError err = Svn.svn_client_diff(diffOptions, path1, revision1, path2, revision2,
											   (recurse ? 1 : 0), (ignoreAncestry ? 1 : 0),
											   (noDiffDeleted ? 1 : 0), outFile, errFile,
											   ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		protected static void InternalDiff(AprArray diffOptions,
								IAprUnmanaged path1, SvnOptRevision revision1,
								IAprUnmanaged path2, SvnOptRevision revision2,
								bool recurse, bool ignoreAncestry, bool noDiffDeleted,
								AprFile outFile, AprFile errFile,  
							    SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_diff({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",diffOptions,path1,revision1,path2,revision2,recurse,ignoreAncestry,noDiffDeleted,outFile,errFile,ctx,pool));
			SvnError err = Svn.svn_client_diff(diffOptions, path1.ToIntPtr(), revision1, 
											   path2.ToIntPtr(), revision2,
											   (recurse ? 1 : 0), (ignoreAncestry ? 1 : 0),
											   (noDiffDeleted ? 1 : 0), outFile, errFile,
											   ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Merge(SvnPath source1, SvnOptRevision revision1,
								 SvnPath source2, SvnOptRevision revision2,
								 SvnPath targetWCPath, bool recurse,
								 bool ignoreAncestry, bool force, bool dryRun,
							     SvnClientContext ctx, AprPool pool)
		{
			InternalMerge(source1, revision1, source2, revision2, targetWCPath, recurse,
						  ignoreAncestry, force, dryRun, ctx, pool);
		}
		
		public static void Merge(SvnUrl source1, SvnOptRevision revision1,
								 SvnUrl source2, SvnOptRevision revision2,
								 SvnPath targetWCPath, bool recurse,
								 bool ignoreAncestry, bool force, bool dryRun,
							     SvnClientContext ctx, AprPool pool)
		{
			InternalMerge(source1, revision1, source2, revision2, targetWCPath, recurse,
						  ignoreAncestry, force, dryRun, ctx, pool);
		}
		
		protected static void InternalMerge(IAprUnmanaged source1, SvnOptRevision revision1,
										  IAprUnmanaged source2, SvnOptRevision revision2,
										  SvnPath targetWCPath, bool recurse,
										  bool ignoreAncestry, bool force, bool dryRun,
										  SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_merge({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",source1,revision1,source2,revision2,targetWCPath,recurse,ignoreAncestry,force,dryRun,ctx,pool));
			SvnError err = Svn.svn_client_merge(source1.ToIntPtr(), revision1, 
												source2.ToIntPtr(), revision2,
											    targetWCPath, (recurse ? 1 : 0),
											    (ignoreAncestry ? 1 : 0), (force ? 1 : 0),
											    (dryRun ? 1 : 0),
											    ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void CleanUp(SvnPath dir,
							       SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_cleanup({0},{1},{2})",dir,ctx,pool));
			SvnError err = Svn.svn_client_cleanup(dir, ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Relocate(SvnPath dir, SvnUrl from, SvnUrl to,
									bool recurse,
							        SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_relocate({0},{1},{2},{3},{4},{5})",dir,from,to,recurse,ctx,pool));
			SvnError err = Svn.svn_client_relocate(dir, from, to, (recurse ? 1 : 0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Revert(AprArray paths, bool recurse, 								  
							      SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_revert({0},{1},{2},{3})",paths,recurse,ctx,pool));
			SvnError err = Svn.svn_client_revert(paths, (recurse ? 1 : 0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Resolved(SvnPath path, bool recurse, 								  
							        SvnClientContext ctx, AprPool pool)
		{
			Debug.WriteLine(String.Format("svn_client_resolved({0},{1},{2},{3})",path,recurse,ctx,pool));
			SvnError err = Svn.svn_client_resolved(path, (recurse ? 1 : 0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static void Copy(SvnPath srcPath, SvnOptRevision srcRevision,
								SvnPath dstPath,
								SvnClientContext ctx, AprPool pool)
		{
			InternalCopy(srcPath, srcRevision, dstPath, ctx, pool);
		}
							        
		public static SvnClientCommitInfo Copy(SvnUrl srcPath, SvnOptRevision srcRevision,
											   SvnUrl dstPath,
							   				   SvnClientContext ctx, AprPool pool)
		{
			return InternalCopy(srcPath, srcRevision, dstPath, ctx, pool);
		}
							        
		protected static SvnClientCommitInfo InternalCopy(IAprUnmanaged srcPath, SvnOptRevision srcRevision,
														IAprUnmanaged dstPath,
							   							SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_copy({0},{1},{2},{3},{4})...",srcPath,srcRevision,dstPath,ctx,pool));
			SvnError err = Svn.svn_client_copy(out commitInfo, srcPath.ToIntPtr(), srcRevision,
											   dstPath.ToIntPtr(),
											   ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
							        
		public static void Move(SvnPath srcPath, SvnOptRevision srcRevision,
								SvnPath dstPath, bool force,
								SvnClientContext ctx, AprPool pool)
		{
			InternalMove(srcPath, srcRevision, dstPath, force, ctx, pool);
		}
		
		public static SvnClientCommitInfo Move(SvnUrl srcPath, SvnOptRevision srcRevision,
											   SvnUrl dstPath, bool force,
							   				   SvnClientContext ctx, AprPool pool)
		{
			return InternalMove(srcPath, srcRevision, dstPath, force, ctx, pool);
		}
		
		public static SvnClientCommitInfo InternalMove(IAprUnmanaged srcPath, 
													   SvnOptRevision srcRevision,
													   IAprUnmanaged dstPath, bool force,
							   						   SvnClientContext ctx, AprPool pool)
		{
			IntPtr commitInfo;
			Debug.Write(String.Format("svn_client_move({0},{1},{2},{3},{4},{5})...",srcPath,srcRevision,dstPath,force,ctx,pool));
			SvnError err = Svn.svn_client_move(out commitInfo, srcPath.ToIntPtr(), srcRevision,
											   dstPath.ToIntPtr(), (force ? 1 : 0),
											   ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",commitInfo));
			return(commitInfo);
		}
		
		public static void PropSet(string propName, SvnString propVal, SvnPath target, 
								   bool recurse, AprPool pool)		
		{
			InternalPropSet(propName, propVal, target, recurse, pool);		
		}
		
		public static void PropSet(string propName, SvnString propVal, SvnUrl target, 
								   bool recurse, AprPool pool)		
		{
			InternalPropSet(propName, propVal, target, recurse, pool);		
		}
		
		protected static void InternalPropSet(string propName, SvnString propVal, IAprUnmanaged target, 
											bool recurse, AprPool pool)		
		{
			Debug.WriteLine(String.Format("svn_client_propset({0},{1},{2},{3},{4})",propName,propVal,target,recurse,pool));
			SvnError err = Svn.svn_client_propset(propName, propVal, target.ToIntPtr(),
												  (recurse ? 1 : 0), pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static int RevPropSet(string propName, SvnString propVal,
								  	 SvnUrl url, SvnOptRevision revision, bool force,
								  	 SvnClientContext ctx, AprPool pool)		
		{
			int rev;
			Debug.Write(String.Format("svn_client_revprop_set({0},{1},{2},{3},{4},{5},{6})...",propName,propVal,url,revision,force,ctx,pool));
			SvnError err = Svn.svn_client_revprop_set(propName, propVal, url, revision,
													  out rev, (force ? 1 : 0),
													  ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static AprHash PropGet(string propName, SvnPath target,
									  SvnOptRevision revision, bool recurse, 
								  	  SvnClientContext ctx, AprPool pool)		
		{
			return InternalPropGet(propName, target, revision, recurse, ctx, pool);		
		}
		
		public static AprHash PropGet(string propName, SvnUrl target,
									  SvnOptRevision revision, bool recurse, 
								  	  SvnClientContext ctx, AprPool pool)		
		{
			return InternalPropGet(propName, target, revision, recurse, ctx, pool);		
		}
		
		protected static AprHash InternalPropGet(string propName, IAprUnmanaged target,
											   SvnOptRevision revision, bool recurse, 
								  			   SvnClientContext ctx, AprPool pool)		
		{
			IntPtr h;
			Debug.Write(String.Format("svn_client_propget({0},{1},{2},{3},{4},{5})...",propName,target,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_propget(out h, propName, target.ToIntPtr(), revision,
												  (recurse ? 1 : 0),
												  ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",h));
			return(h);
		}
		
		public static SvnString RevPropGet(string propName, SvnUrl url,
										   SvnOptRevision revision, out int setRev, 
										   SvnClientContext ctx, AprPool pool)		
		{
			IntPtr s;
			Debug.Write(String.Format("svn_client_revprop_get({0},{1},{2},{3},{4})...",propName,url,revision,ctx,pool));
			SvnError err = Svn.svn_client_revprop_get(propName, out s, url, revision, out setRev,
												      ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0},{1})",s,setRev));
			return(s);
		}
		
		public static AprHash PropList(SvnPath target,
									   SvnOptRevision revision, bool recurse, 
								  	   SvnClientContext ctx, AprPool pool)		
		{
			return InternalPropList(target, revision, recurse, ctx, pool);		
		}
		
		public static AprHash PropList(SvnUrl target,
									   SvnOptRevision revision, bool recurse, 
								  	   SvnClientContext ctx, AprPool pool)		
		{
			return InternalPropList(target, revision, recurse, ctx, pool);		
		}
		
		protected static AprHash InternalPropList(IAprUnmanaged target,
									   			SvnOptRevision revision, bool recurse, 
								  	   			SvnClientContext ctx, AprPool pool)		
		{
			IntPtr h;
			Debug.Write(String.Format("svn_client_proplist({0},{1},{2},{3},{4})...",target,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_proplist(out h, target.ToIntPtr(), revision,
												   (recurse ? 1 : 0),
												   ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",h));
			return(h);
		}
		
		public static AprHash RevPropList(SvnUrl url,
										  SvnOptRevision revision, out int setRev, 
										  SvnClientContext ctx, AprPool pool)		
		{
			IntPtr h;
			Debug.Write(String.Format("svn_client_revprop_list({0},{1},{2},{3})...",url,revision,ctx,pool));
			SvnError err = Svn.svn_client_revprop_list(out h, url, revision, out setRev,
												       ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0},{1})",h,setRev));
			return(h);
		}

		public static void Export(SvnPath from, SvnPath to, 
								  SvnOptRevision revision, 
								  bool force, SvnClientContext ctx, AprPool pool)
		{
			InternalExport(from, to, revision, force, ctx, pool);
		}
		
		public static int Export(SvnUrl from, SvnPath to, 
								 SvnOptRevision revision, 
								 bool force, SvnClientContext ctx, AprPool pool)
		{
			return InternalExport(from, to, revision, force, ctx, pool);
		}
		
		protected static int InternalExport(IAprUnmanaged from, SvnPath to, 
										  SvnOptRevision revision, 
										  bool force, SvnClientContext ctx, AprPool pool)
		{
			int rev;
			Debug.Write(String.Format("svn_client_export({0},{1},{2},{3},{4},{5})...",from,to,revision,force,ctx,pool));
			SvnError err = Svn.svn_client_export(out rev, from.ToIntPtr(), to, 
												 revision, 
												 (force ? 1 :0), ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",rev));
			return(rev);
		}
		
		public static AprHash List(SvnPath pathOrUrl,
								   SvnOptRevision revision, bool recurse, 
								   SvnClientContext ctx, AprPool pool)		
		{
			return InternalList(pathOrUrl, revision, recurse, ctx, pool);		
		}
		
		public static AprHash List(SvnUrl pathOrUrl,
								   SvnOptRevision revision, bool recurse, 
								   SvnClientContext ctx, AprPool pool)		
		{
			return InternalList(pathOrUrl, revision, recurse, ctx, pool);		
		}
		
		protected static AprHash InternalList(IAprUnmanaged pathOrUrl,
								   			SvnOptRevision revision, bool recurse, 
											SvnClientContext ctx, AprPool pool)		
		{
			IntPtr h;
			Debug.Write(String.Format("svn_client_list({0},{1},{2},{3},{4})...",pathOrUrl,revision,recurse,ctx,pool));
			SvnError err = Svn.svn_client_ls(out h, pathOrUrl.ToIntPtr(), revision, (recurse ? 1 : 0),
											 ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",h));
			return(h);
		}
		
		public static void Cat(SvnStream stream, SvnPath pathOrUrl,
							   SvnOptRevision revision, 
							   SvnClientContext ctx, AprPool pool)		
		{		
			InternalCat(stream, pathOrUrl, revision, ctx, pool);		
		}
		
		public static void Cat(SvnStream stream, SvnUrl pathOrUrl,
							   SvnOptRevision revision, 
							   SvnClientContext ctx, AprPool pool)		
		{		
			InternalCat(stream, pathOrUrl, revision, ctx, pool);		
		}
		
		protected static void InternalCat(SvnStream stream, IAprUnmanaged pathOrUrl,
										SvnOptRevision revision, 
										SvnClientContext ctx, AprPool pool)		
		{		
			Debug.WriteLine(String.Format("svn_client_cat({0},{1},{2},{3},{4})",stream,pathOrUrl,revision,ctx,pool));
			SvnError err = Svn.svn_client_cat(stream, pathOrUrl.ToIntPtr(), revision, ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
		}
		
		public static SvnUrl UrlFromPath(SvnPath pathOrUrl, AprPool pool)
		{
			return InternalUrlFromPath(pathOrUrl, pool);
		}
		
		public static SvnUrl UrlFromPath(SvnUrl pathOrUrl, AprPool pool)
		{
			return InternalUrlFromPath(pathOrUrl, pool);
		}
		
		protected static SvnUrl InternalUrlFromPath(IAprUnmanaged pathOrUrl, AprPool pool)
		{
			IntPtr s;
			Debug.Write(String.Format("svn_client_url_from_path({0},{1})...",pathOrUrl,pool));
			SvnError err = Svn.svn_client_url_from_path(out s, pathOrUrl.ToIntPtr(), pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",s));
			return(s);
		}
		
		public static AprString UuidFromUrl(SvnUrl url, SvnClientContext ctx, AprPool pool)
		{
			IntPtr s;
			Debug.Write(String.Format("svn_client_uuid_from_url({0},{1})...",url,ctx,pool));
			SvnError err = Svn.svn_client_uuid_from_url(out s, url, ctx, pool);
			if( !err.IsNoError )
				throw new SvnException(err);
			Debug.WriteLine(String.Format("Done({0})",s));
			return(s);
		}
	}
}