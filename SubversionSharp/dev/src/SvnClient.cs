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
using System.Collections;
using Softec.AprSharp;

namespace Softec.SubversionSharp
{
    public class SvnClient : SvnClientBase
    {
    	AprPool mGlobalPool;
    	AprPool mPool;
    	SvnClientContext mContext;
    	ArrayList mAuthObjs;
    	
    	#region ctors and finalize
    	static SvnClient()
    	{
			SvnConfig.Ensure();
    	}
    	
    	public SvnClient()
    	{
    		mGlobalPool = Svn.PoolCreate();
    		mPool = Svn.PoolCreate(mGlobalPool);
	        mContext = SvnClientContext.Create(mGlobalPool);
	        mContext.Config = SvnConfig.GetConfig(mGlobalPool);
	        mAuthObjs = null;
    	}
    	
    	public SvnClient(AprPool pool)
    	{
	        mGlobalPool = pool;
    		mPool = Svn.PoolCreate(mGlobalPool);
	        mContext = SvnClientContext.Create(mGlobalPool);
	        mContext.Config = SvnConfig.GetConfig(mGlobalPool);
	        mAuthObjs = null;
    	}
    	
    	public SvnClient(SvnClientContext ctx, AprPool pool)
    	{
    		mGlobalPool = pool;
    		mPool = Svn.PoolCreate(mGlobalPool);
    		mContext = ctx;
	        mAuthObjs = null;
    	}
       	#endregion
    	
    	#region Methods
    	public void Clear()
    	{
    		mPool.Clear();
    	}
    	
    	public void Reset()
    	{
    		mPool.Destroy();
    		mPool = Svn.PoolCreate(mGlobalPool);
    	}
    	
    	public IAprUnmanaged PathOrUrl(string str)
    	{
    		try {
    			return(new SvnUrl(str, mPool));
    		}
    		catch (SvnException e) {}
    		return(new SvnPath(str, mPool));
    	}
    	
    	private Type PathOrUrlArrayType(ICollection coll)
    	{
    		IEnumerator it = coll.GetEnumerator();
    		it.MoveNext();
    		Type t = it.Current.GetType();
    		
    		if(t == typeof(SvnUrl) || t == typeof(SvnPath))
    			return t;
    		
    		if(t == typeof(Uri))
    			return typeof(SvnUrl);
    			
    		try {
    			new Uri(it.Current.ToString());
    			return(typeof(SvnUrl));
    		}
    		catch (SvnException e) {}
    		
    		return typeof(SvnPath);
    	}
       	#endregion
    	
    	#region Client methods
		public int Checkout(SvnUrl url, SvnPath path, SvnRevision revision, bool recurse)
		{
			return Checkout(url, path, revision.ToSvnOpt(mPool), recurse, 
							mContext, mPool);
		}
		
		public int Checkout(string url, string path, SvnRevision revision, bool recurse)
		{
			return Checkout(new SvnUrl(url, mPool), new SvnPath(path, mPool), 
							revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}

		public int Update(SvnPath path, SvnRevision revision, bool recurse)
		{
			return Update(path, revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public int Update(string path, SvnRevision revision, bool recurse)
		{
			return Update(new SvnPath(path, mPool), revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public int Switch(SvnPath path, SvnUrl url, SvnRevision revision, bool recurse)
		{
			return Switch(path, url, revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public int Switch(string path, SvnUrl url, SvnRevision revision, bool recurse)
		{
			return Switch(new SvnPath(path,mPool), url, revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public void Add(SvnPath path, bool recurse)
		{
			Add(path, recurse, mContext, mPool);
		}

		public void Add(string path, bool recurse)
		{
			Add(new SvnPath(path, mPool), recurse, mContext, mPool);
		}
		
        public SvnClientCommitInfo Mkdir(ICollection paths)
		{
			return Mkdir(AprArray.LazyMake(mPool,paths,PathOrUrlArrayType(paths)), mContext, mPool);
		}
		
		public SvnClientCommitInfo Delete(ICollection paths, bool force)
		{
			return Delete(AprArray.LazyMake(mPool,paths,PathOrUrlArrayType(paths)), force, mContext, mPool);
		}
		
		public SvnClientCommitInfo Import(SvnPath path, SvnUrl url, bool nonrecursive)
		{
			return Import(path, url, nonrecursive, mContext, mPool);
		}
		
		public SvnClientCommitInfo Import(string path, SvnUrl url, bool nonrecursive)
		{
			return Import(new SvnPath(path,mPool), url, nonrecursive, mContext, mPool);
		}
		
		public SvnClientCommitInfo Commit(ICollection targets, bool nonrecursive)
		{
			return Commit(AprArray.LazyMake(mPool,targets,typeof(SvnPath)), nonrecursive,
						  mContext, mPool);
		}
		
		public int Status(SvnPath path, SvnRevision revision,
						  SvnWcStatus.Func statusFunc, IntPtr statusBaton,
						  bool descend, bool getAll, bool update, bool noIgnore)
		{
			return Status(path, revision.ToSvnOpt(mPool), statusFunc, statusBaton,
						  descend, getAll, update, noIgnore, mContext, mPool);
		}

		public int Status(string path, SvnRevision revision,
						  SvnWcStatus.Func statusFunc, IntPtr statusBaton,
						  bool descend, bool getAll, bool update, bool noIgnore)
		{
			return Status(new SvnPath(path,mPool), revision.ToSvnOpt(mPool), statusFunc, statusBaton,
						  descend, getAll, update, noIgnore, mContext, mPool);
		}

		public void Log(ICollection targets, 
						SvnRevision start, SvnRevision end,
						bool discoverChangedPaths, bool strictNodeHistory,
						LogMessageReceiver receiver, IntPtr baton)
		{
			Log(AprArray.LazyMake(mPool,targets,typeof(SvnPath)), 
				start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				discoverChangedPaths, strictNodeHistory, receiver, baton,
				mContext, mPool);
		}

		public void Blame(SvnPath pathOrUrl,
						  SvnRevision start, SvnRevision end, 
						  BlameReceiver receiver, IntPtr baton)
		{
			Blame(pathOrUrl, 
				  start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				  receiver, baton, mContext, mPool);
		}

		public void Blame(SvnUrl pathOrUrl,
						  SvnRevision start, SvnRevision end, 
						  BlameReceiver receiver, IntPtr baton)
		{
			Blame(pathOrUrl, 
				  start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				  receiver, baton, mContext, mPool);
		}
		
		public void Blame(string pathOrUrl,
						  SvnRevision start, SvnRevision end, 
						  BlameReceiver receiver, IntPtr baton)
		{
			InternalBlame(PathOrUrl(pathOrUrl), 
				  		  start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				  		  receiver, baton, mContext, mPool);
		}

		public void Diff(ICollection diffOptions,
						 SvnPath path1, SvnRevision revision1,
						 SvnPath path2, SvnRevision revision2,
						 bool recurse, bool ignoreAncestry, bool noDiffDeleted,
						 AprFile outFile, AprFile errFile)
		{
			Diff(AprArray.LazyMake(mPool,diffOptions,typeof(AprString)),
				 path1, revision1.ToSvnOpt(mPool),
				 path2, revision2.ToSvnOpt(mPool),
				 recurse, ignoreAncestry, noDiffDeleted,
				 outFile, errFile, mContext, mPool);
		}
		
		public void Diff(ICollection diffOptions,
						 SvnPath path1, SvnRevision revision1,
						 SvnUrl path2, SvnRevision revision2,
						 bool recurse, bool ignoreAncestry, bool noDiffDeleted,
						 AprFile outFile, AprFile errFile)
		{
			Diff(AprArray.LazyMake(mPool,diffOptions,typeof(AprString)),
				 path1, revision1.ToSvnOpt(mPool),
				 path2, revision2.ToSvnOpt(mPool),
				 recurse, ignoreAncestry, noDiffDeleted,
				 outFile, errFile, mContext, mPool);
		}
		
		public void Diff(ICollection diffOptions,
						 SvnUrl path1, SvnRevision revision1,
						 SvnPath path2, SvnRevision revision2,
						 bool recurse, bool ignoreAncestry, bool noDiffDeleted,
						 AprFile outFile, AprFile errFile)
		{
			Diff(AprArray.LazyMake(mPool,diffOptions,typeof(AprString)),
				 path1, revision1.ToSvnOpt(mPool),
				 path2, revision2.ToSvnOpt(mPool),
				 recurse, ignoreAncestry, noDiffDeleted,
				 outFile, errFile, mContext, mPool);
		}
		
		public void Diff(ICollection diffOptions,
						 SvnUrl path1, SvnRevision revision1,
						 SvnUrl path2, SvnRevision revision2,
						 bool recurse, bool ignoreAncestry, bool noDiffDeleted,
						 AprFile outFile, AprFile errFile)
		{
			Diff(AprArray.LazyMake(mPool,diffOptions,typeof(AprString)),
				 path1, revision1.ToSvnOpt(mPool),
				 path2, revision2.ToSvnOpt(mPool),
				 recurse, ignoreAncestry, noDiffDeleted,
				 outFile, errFile, mContext, mPool);
		}
		
		public void Merge(SvnPath source1, SvnRevision revision1,
						  SvnPath source2, SvnRevision revision2,
						  SvnPath targetWCPath, bool recurse,
						  bool ignoreAncestry, bool force, bool dryRun)
		{
			Merge(source1, revision1.ToSvnOpt(mPool),
				  source2, revision2.ToSvnOpt(mPool),
				  targetWCPath, recurse, ignoreAncestry, force, dryRun, mContext, mPool);
		}
		
		public void Merge(SvnUrl source1, SvnRevision revision1,
						  SvnUrl source2, SvnRevision revision2,
						  SvnPath targetWCPath, bool recurse,
						  bool ignoreAncestry, bool force, bool dryRun)
		{
			Merge(source1, revision1.ToSvnOpt(mPool),
				  source2, revision2.ToSvnOpt(mPool),
				  targetWCPath, recurse, ignoreAncestry, force, dryRun, mContext, mPool);
		}
		
		public void Merge(string source1, SvnRevision revision1,
						  string source2, SvnRevision revision2,
						  string targetWCPath, bool recurse,
						  bool ignoreAncestry, bool force, bool dryRun)
		{
			InternalMerge(PathOrUrl(source1), revision1.ToSvnOpt(mPool),
				  		  PathOrUrl(source2), revision2.ToSvnOpt(mPool),
				  		  new SvnPath(targetWCPath,mPool),
				  		  recurse, ignoreAncestry, force, dryRun, mContext, mPool);
		}
		
		public void CleanUp(SvnPath dir)
		{
			CleanUp(dir, mContext, mPool);
		}
		
		public void CleanUp(string dir)
		{
			CleanUp(new SvnPath(dir, mPool), mContext, mPool);
		}
		
		public void Relocate(SvnPath dir, SvnUrl from, SvnUrl to, bool recurse)
		{
			Relocate(dir, from, to, recurse, mContext, mPool);
		}
		
		public void Relocate(string dir, string from, string to, bool recurse)
		{
			Relocate(new SvnPath(dir, mPool), new SvnUrl(from, mPool), new SvnUrl(to, mPool),
					 recurse, mContext, mPool);
		}
		
		public void Revert(ICollection paths, bool recurse)
		{
			Revert(AprArray.LazyMake(mPool,paths,typeof(SvnPath)), recurse, mContext, mPool);
		}
		
		public void Resolved(SvnPath path, bool recurse)
		{
			Resolved(path, recurse, mContext, mPool);
		}
		
		public void Resolved(string path, bool recurse)
		{
			Resolved(new SvnPath(path, mPool), recurse, mContext, mPool);
		}
		
		public void Copy(SvnPath srcPath, SvnRevision srcRevision, SvnPath dstPath)
		{
			Copy(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, mContext, mPool);
		}
							        
		public SvnClientCommitInfo Copy(SvnUrl srcPath, SvnRevision srcRevision, SvnUrl dstPath)
		{
			return Copy(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, mContext, mPool);
		}
							        
		public SvnClientCommitInfo Copy(string srcPath, SvnRevision srcRevision, string dstPath)
		{
			return InternalCopy(PathOrUrl(srcPath), srcRevision.ToSvnOpt(mPool), 
								PathOrUrl(dstPath), mContext, mPool);
		}
		
		public void Move(SvnPath srcPath, SvnRevision srcRevision,
						 SvnPath dstPath, bool force)
		{
			Move(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, force, mContext, mPool);
		}
		
		public SvnClientCommitInfo Move(SvnUrl srcPath, SvnRevision srcRevision,
										SvnUrl dstPath, bool force)
		{
			return Move(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, force, mContext, mPool);
		}
		
		public SvnClientCommitInfo Move(string srcPath, SvnRevision srcRevision,
										string dstPath, bool force)
		{
			return InternalMove(PathOrUrl(srcPath), srcRevision.ToSvnOpt(mPool), 
								PathOrUrl(dstPath), force, mContext, mPool);
		}
		
		public void PropSet(string propName, SvnString propVal, SvnPath target, bool recurse)		
		{
			PropSet(propName, propVal, target, recurse, mPool);
		}
		
		public void PropSet(string propName, string propVal, SvnPath target, bool recurse)		
		{
			PropSet(propName, new SvnString(propVal, mPool), target, recurse, mPool);
		}
		
		public void PropSet(string propName, SvnString propVal, SvnUrl target, bool recurse)		
		{
			PropSet(propName, propVal, target, recurse, mPool);
		}
		
		public void PropSet(string propName, string propVal, SvnUrl target, bool recurse)		
		{
			PropSet(propName, new SvnString(propVal, mPool), target, recurse, mPool);
		}
		
		public void PropSet(string propName, SvnString propVal, string target, bool recurse)		
		{
			InternalPropSet(propName, propVal, PathOrUrl(target), recurse, mPool);
		}
		
		public void PropSet(string propName, string propVal, string target, bool recurse)		
		{
			InternalPropSet(propName, new SvnString(propVal, mPool), PathOrUrl(target), recurse,
							mPool);
		}
		
		public int RevPropSet(string propName, SvnString propVal,
							  SvnUrl url, SvnRevision revision, bool force)		
		{
			return RevPropSet(propName, propVal, url, revision.ToSvnOpt(mPool), force,
							  mContext, mPool);
		}
		
		public int RevPropSet(string propName, string propVal,
							  SvnUrl url, SvnRevision revision, bool force)		
		{
			return RevPropSet(propName, new SvnString(propVal, mPool), url,
							  revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public int RevPropSet(string propName, SvnString propVal,
							  string url, SvnRevision revision, bool force)		
		{
			return RevPropSet(propName, propVal, new SvnUrl(url, mPool),
							  revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public int RevPropSet(string propName, string propVal,
							  string url, SvnRevision revision, bool force)		
		{
			return RevPropSet(propName, new SvnString(propVal,mPool), new SvnUrl(url, mPool),
							  revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public AprHash PropGet(string propName, SvnPath target,
							   SvnRevision revision, bool recurse)		
		{
			return PropGet(propName, target, revision.ToSvnOpt(mPool), recurse,
						   mContext, mPool);
		}
		
		public AprHash PropGet(string propName, SvnUrl target,
							   SvnRevision revision, bool recurse)		
		{
			return PropGet(propName, target, revision.ToSvnOpt(mPool), recurse,
						   mContext, mPool);
		}
		
		public AprHash PropGet(string propName, string target,
							   SvnRevision revision, bool recurse)		
		{
			return InternalPropGet(propName, PathOrUrl(target), revision.ToSvnOpt(mPool), recurse,
						   		   mContext, mPool);
		}
		
		public SvnString RevPropGet(string propName, SvnUrl url,
									SvnRevision revision, out int setRev)		
		{
			return RevPropGet(propName, url, revision.ToSvnOpt(mPool), out setRev,
							  mContext, mPool);
		}
		
		public SvnString RevPropGet(string propName, string url,
									SvnRevision revision, out int setRev)		
		{
			return RevPropGet(propName, new SvnUrl(url, mPool), revision.ToSvnOpt(mPool),
							  out setRev, mContext, mPool);
		}
		
		public AprHash PropList(SvnPath target, SvnRevision revision, bool recurse)		
		{
			return PropList(target, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public AprHash PropList(SvnUrl target, SvnRevision revision, bool recurse)		
		{
			return PropList(target, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public AprHash PropList(string target, SvnRevision revision, bool recurse)		
		{
			return InternalPropList(PathOrUrl(target), revision.ToSvnOpt(mPool), recurse,
									mContext, mPool);
		}
		
		public AprHash RevPropList(SvnUrl url, SvnRevision revision, out int setRev)		
		{
			return RevPropList(url, revision.ToSvnOpt(mPool), out setRev, mContext, mPool);
		}

		public AprHash RevPropList(string url, SvnRevision revision, out int setRev)		
		{
			return RevPropList(new SvnUrl(url, mPool), revision.ToSvnOpt(mPool), out setRev,
							   mContext, mPool);
		}

		public void Export(SvnPath from, SvnPath to, SvnRevision revision, bool force)
		{
			Export(from, to, revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public int Export(SvnUrl from, SvnPath to, SvnRevision revision, bool force)
		{
			return Export(from, to, revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public int Export(string from, string to, SvnRevision revision, bool force)
		{
			return InternalExport(PathOrUrl(from), new SvnPath(to,mPool), revision.ToSvnOpt(mPool),
						  		  force, mContext, mPool);
		}
		
		public AprHash List(SvnPath pathOrUrl, SvnRevision revision, bool recurse)		
		{
			return List(pathOrUrl, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public AprHash List(SvnUrl pathOrUrl, SvnRevision revision, bool recurse)		
		{
			return List(pathOrUrl, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public AprHash List(string pathOrUrl, SvnRevision revision, bool recurse)		
		{
			return InternalList(PathOrUrl(pathOrUrl), revision.ToSvnOpt(mPool), recurse,
								mContext, mPool);
		}
		
		public void Cat(SvnStream stream, SvnPath pathOrUrl, SvnRevision revision)		
		{
			Cat(stream, pathOrUrl, revision.ToSvnOpt(mPool), mContext, mPool);
		}
		
		public void Cat(SvnStream stream, SvnUrl pathOrUrl, SvnRevision revision)		
		{
			Cat(stream, pathOrUrl, revision.ToSvnOpt(mPool), mContext, mPool);
		}
		
		public void Cat(SvnStream stream, string pathOrUrl, SvnRevision revision)		
		{
			InternalCat(stream, PathOrUrl(pathOrUrl), revision.ToSvnOpt(mPool), mContext, mPool);
		}
		
		public SvnUrl UrlFromPath(SvnPath pathOrUrl)
		{
			return UrlFromPath(pathOrUrl, mPool);
		}
		
		public SvnUrl UrlFromPath(SvnUrl pathOrUrl)
		{
			return UrlFromPath(pathOrUrl, mPool);
		}
		
		public SvnUrl UrlFromPath(string pathOrUrl)
		{
			return InternalUrlFromPath(PathOrUrl(pathOrUrl), mPool);
		}
		
		public AprString UuidFromUrl(SvnUrl url)
		{
			return UuidFromUrl(url, mContext, mPool);
		}

		public AprString UuidFromUrl(string url)
		{
			return UuidFromUrl(new SvnUrl(url, mPool), mContext, mPool);
		}

		#endregion
    	
    	#region Authentication methods
		public void AddSimpleProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSimpleProvider(mGlobalPool));
        }
        
        public void AddUsernameProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetUsernameProvider(mGlobalPool));
        }
        
        public void AddSslServerTrustFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
       		mAuthObjs.Add(SvnAuthProviderObject.GetSslServerTrustFileProvider(mGlobalPool));
        }
        
        public void AddSslClientCertFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSslClientCertFileProvider(mGlobalPool));
        }
        
        public void AddSslClientCertPwFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSslClientCertPwFileProvider(mGlobalPool));
        }

        public void AddPromptProvider(SvnAuthProviderObject.SimplePrompt promptFunc,
        							  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mGlobalPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.UsernamePrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mGlobalPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslServerTrustPrompt promptFunc,
									  IntPtr promptBaton)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  mGlobalPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslClientCertPrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mGlobalPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslClientCertPwPrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mGlobalPool));
        }
        
        public void OpenAuth()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mContext.AuthBaton = SvnAuthBaton.Open(mAuthObjs,mGlobalPool);
        	mAuthObjs = null;
        }
       	#endregion
    	
    	#region Member access throught properties
    	public AprPool GlobalPool
    	{
    		get
    		{
    			return(mGlobalPool);
    		}
    	}
    	public AprPool Pool
    	{
    		get
    		{
    			return(mPool);
    		}
    	}
    	public SvnClientContext Context
    	{
    		get
    		{
    			return(mContext);
    		}
    	}
    	#endregion
	}
}