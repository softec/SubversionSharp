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
       	#endregion
    	
    	#region Client methods
		public int Checkout(string url, string path, SvnRevision revision, bool recurse)
		{
			return Checkout(url, path, revision.ToSvnOpt(mPool), recurse, 
							mContext, mPool);
		}
		
		public int Update(string path, SvnRevision revision, bool recurse)
		{
			return Update(path, revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public int Switch(string path, string url, SvnRevision revision, bool recurse)
		{
			return Switch(path, url, revision.ToSvnOpt(mPool), recurse,
						  mContext, mPool);
		}
		
		public void Add(string path, bool recurse)
		{
			Add(path, recurse, mContext, mPool);
		}

        public SvnClientCommitInfo Mkdir(ICollection paths)
		{
			return Mkdir(AprArray.CastMake(mPool,paths), mContext, mPool);
		}
		
		public SvnClientCommitInfo Delete(ICollection paths, bool force)
		{
			return Delete(AprArray.CastMake(mPool,paths), force, mContext, mPool);
		}
		
		public SvnClientCommitInfo Import(string path, string url, bool nonrecursive)
		{
			return Import(path, url, nonrecursive, mContext, mPool);
		}
		
		public SvnClientCommitInfo Commit(ICollection targets, bool nonrecursive)
		{
			return Commit(AprArray.CastMake(mPool,targets), nonrecursive, mContext, mPool);
		}
		
		public int Status(string path, SvnRevision revision,
						  SvnWcStatus.Func statusFunc, IntPtr statusBaton,
						  bool descend, bool getAll, bool update, bool noIgnore)
		{
			return Status(path, revision.ToSvnOpt(mPool), statusFunc, statusBaton,
						  descend, getAll, update, noIgnore, mContext, mPool);
		}

		public void Log(ICollection targets, 
						SvnRevision start, SvnRevision end,
						bool discoverChangedPaths, bool strictNodeHistory,
						LogMessageReceiver receiver, IntPtr baton)
		{
			Log(AprArray.CastMake(mPool,targets), 
				start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				discoverChangedPaths, strictNodeHistory, receiver, baton,
				mContext, mPool);
		}

		public void Blame(string pathOrUrl,
						  SvnRevision start, SvnRevision end, 
						  BlameReceiver receiver, IntPtr baton)
		{
			Blame(pathOrUrl, 
				  start.ToSvnOpt(mPool), end.ToSvnOpt(mPool),
				  receiver, baton, mContext, mPool);
		}

		public void Diff(ICollection diffOptions,
						 string path1, SvnRevision revision1,
						 string path2, SvnRevision revision2,
						 bool recurse, bool ignoreAncestry, bool noDiffDeleted,
						 AprFile outFile, AprFile errFile)
		{
			Diff(AprArray.CastMake(mPool,diffOptions),
				 path1, revision1.ToSvnOpt(mPool),
				 path2, revision2.ToSvnOpt(mPool),
				 recurse, ignoreAncestry, noDiffDeleted,
				 outFile, errFile, mContext, mPool);
		}
		
		public void Merge(string source1, SvnRevision revision1,
						  string source2, SvnRevision revision2,
						  string targetWCPath, bool recurse,
						  bool ignoreAncestry, bool force, bool dryRun)
		{
			Merge(source1, revision1.ToSvnOpt(mPool),
				  source2, revision2.ToSvnOpt(mPool),
				  targetWCPath, recurse, ignoreAncestry, force, dryRun, mContext, mPool);
		}
		
		public void CleanUp(string dir)
		{
			CleanUp(dir, mContext, mPool);
		}
		
		public void Relocate(string dir, string from, string to, bool recurse)
		{
			Relocate(dir, from, to, recurse, mContext, mPool);
		}
		
		public void Revert(ICollection paths, bool recurse)
		{
			Revert(AprArray.CastMake(mPool,paths), recurse, mContext, mPool);
		}
		
		public void Resolved(string path, bool recurse)
		{
			Resolved(path, recurse, mContext, mPool);
		}
		
		public SvnClientCommitInfo Copy(string srcPath, SvnRevision srcRevision, string dstPath)
		{
			return Copy(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, mContext, mPool);
		}
							        
		public SvnClientCommitInfo Move(string srcPath, SvnRevision srcRevision,
										string dstPath, bool force)
		{
			return Move(srcPath, srcRevision.ToSvnOpt(mPool), dstPath, force,
						mContext, mPool);
		}
		
		public void PropSet(string propName, SvnString propVal, string target, bool recurse)		
		{
			PropSet(propName, propVal, target, recurse, mPool);
		}
		
		public int RevPropSet(string propName, SvnString propVal,
							  string url, SvnRevision revision, bool force)		
		{
			return RevPropSet(propName, propVal, url, revision.ToSvnOpt(mPool), force,
							  mContext, mPool);
		}
		
		public AprHash PropGet(string propName, string target,
							   SvnRevision revision, bool recurse)		
		{
			return PropGet(propName, target, revision.ToSvnOpt(mPool), recurse,
						   mContext, mPool);
		}
		
		public SvnString RevPropGet(string propName, string url,
									SvnRevision revision, out int setRev)		
		{
			return RevPropGet(propName, url, revision.ToSvnOpt(mPool), out setRev,
							  mContext, mPool);
		}
		
		public AprHash PropList(string target, SvnRevision revision, bool recurse)		
		{
			return PropList(target, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public AprHash RevPropList(string url, SvnRevision revision, out int setRev)		
		{
			return RevPropList(url, revision.ToSvnOpt(mPool), out setRev, mContext, mPool);
		}

		public int Export(string from, string to, SvnRevision revision, bool force)
		{
			return Export(from, to, revision.ToSvnOpt(mPool), force, mContext, mPool);
		}
		
		public AprHash List(string pathOrUrl, SvnRevision revision, bool recurse)		
		{
			return List(pathOrUrl, revision.ToSvnOpt(mPool), recurse, mContext, mPool);
		}
		
		public void Cat(SvnStream stream, string pathOrUrl, SvnRevision revision)		
		{
			Cat(stream, pathOrUrl, revision.ToSvnOpt(mPool), mContext, mPool);
		}
		
		public AprString UrlFromPath(string pathOrUrl)
		{
			return UrlFromPath(pathOrUrl, mPool);
		}
		
		public AprString UuidFromUrl(string url)
		{
			return UuidFromUrl(url, mContext, mPool);
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