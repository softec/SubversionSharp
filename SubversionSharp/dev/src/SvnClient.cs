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
    	AprPool mPool;
    	SvnClientContext mContext;
    	ArrayList mAuthObjs;
    	
    	#region ctors
    	static SvnClient()
    	{
			SvnConfig.Ensure();
    	}
    	
    	public SvnClient()
    	{
    		mPool = Svn.PoolCreate();
	        mContext = SvnClientContext.Create(mPool);
	        mContext.Config = SvnConfig.GetConfig(mPool);
	        mAuthObjs = null;
    	}
    	
    	public SvnClient(AprPool pool)
    	{
	        mPool = pool;
	        mContext = SvnClientContext.Create(mPool);
	        mContext.Config = SvnConfig.GetConfig(mPool);
	        mAuthObjs = null;
    	}
    	
    	public SvnClient(SvnClientContext ctx, AprPool pool)
    	{
    		mPool = pool;
    		mContext = ctx;
	        mAuthObjs = null;
    	}
       	#endregion
    	
    	#region Checkout methods
		public int Checkout(string url, string path, SvnOptRevision.RevisionKind revision, 
							bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Kind = revision;
			return Checkout(url, path, optRev, recurse, mContext, pool);
		}
		
		public int Checkout(string url, string path, SvnOptRevision.RevisionKind revision, 
							bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Checkout(url, path, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}

		public int Checkout(string url, string path, int revision, bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Number = revision;
			return Checkout(url, path, optRev, recurse, mContext, pool);
		}
		
		public int Checkout(string url, string path, int revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Checkout(url, path, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}

		public int Checkout(string url, string path, long revision,	bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Date = revision;
			return Checkout(url, path, optRev, recurse, mContext, pool);
		}
		
		public int Checkout(string url, string path, long revision,	bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Checkout(url, path, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}
		#endregion
    	
    	#region Update methods
		public int Update(string path, SvnOptRevision.RevisionKind revision, bool recurse,
						  AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Kind = revision;
			return Update(path, optRev, recurse, mContext, pool);
		}
		
		public int Update(string path, SvnOptRevision.RevisionKind revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Update(path, revision, recurse, pool);
			return(rev);
		}

		public int Update(string path, int revision, bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Number = revision;
			return Update(path, optRev, recurse, mContext, pool);
		}
		
		public int Update(string path, int revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Update(path, revision, recurse, pool);
			return(rev);
		}

		public int Update(string path, long revision, bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Date = revision;
			return Update(path, optRev, recurse, mContext, pool);
		}
		
		public int Update(string path, long revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Update(path, revision, recurse, pool);
			return(rev);
		}

		#endregion
    	
    	#region Switch methods
		public int Switch(string path, string url, 
						  SvnOptRevision.RevisionKind revision, bool recurse,
						  AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Kind = revision;
			return Switch(path, url, optRev, recurse, mContext, pool);
		}
		
		public int Switch(string path, string url, 
						  SvnOptRevision.RevisionKind revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Switch(path, url, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}

		public int Switch(string path, string url, int revision, bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Number = revision;
			return Switch(path, url, optRev, recurse, mContext, pool);
		}
		
		public int Switch(string path, string url, int revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Switch(path, url, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}

		public int Switch(string path, string url, long revision, bool recurse, AprPool pool)
		{
	        SvnOptRevision optRev = SvnOptRevision.Alloc(pool);
			optRev.Date = revision;
			return Switch(path, url, optRev, recurse, mContext, pool);
		}
		
		public int Switch(string path, string url, long revision, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			int rev = Switch(path, url, revision, recurse, pool);
			pool.Destroy();
			return(rev);
		}
		#endregion
    	
    	#region Add method
		public void Add(string path, bool recurse, AprPool pool)
		{
			Add(path, recurse, mContext, pool);
		}
		
		public void Add(string path, bool recurse)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			Add(path, recurse, mContext, pool);
			pool.Destroy();
		}
		#endregion
    	
    	#region Mkdir methods
        public SvnClientCommitInfo Mkdir(AprArray paths, AprPool pool)
		{
			return Mkdir(paths, mContext, pool);
		}
		
        public void Mkdir(AprArray paths)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			Mkdir(paths, mContext, pool);
			pool.Destroy();
		}
		#endregion
    	
    	#region Delete methods
		public SvnClientCommitInfo Delete(AprArray paths, bool force, AprPool pool)
		{
			return Delete(paths, force, mContext, pool);
		}
		
        public void Delete(AprArray paths, bool force)
		{
			AprPool pool = Svn.PoolCreate(mPool);
			Delete(paths, force, mContext, pool);
			pool.Destroy();
		}
		#endregion
    	
    	#region Import methods
		public SvnClientCommitInfo Import(string path, string url, bool nonrecursive,  
										  AprPool pool)
		{
			return Import(path, url, nonrecursive, mContext, pool);
		}
		#endregion
    	
    	#region Authentication methods
		public void AddSimpleProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSimpleProvider(mPool));
        }
        
        public void AddUsernameProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetUsernameProvider(mPool));
        }
        
        public void AddSslServerTrustFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
       		mAuthObjs.Add(SvnAuthProviderObject.GetSslServerTrustFileProvider(mPool));
        }
        
        public void AddSslClientCertFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSslClientCertFileProvider(mPool));
        }
        
        public void AddSslClientCertPwFileProvider()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetSslClientCertPwFileProvider(mPool));
        }

        public void AddPromptProvider(SvnAuthProviderObject.SimplePrompt promptFunc,
        							  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.UsernamePrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslServerTrustPrompt promptFunc,
									  IntPtr promptBaton)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  mPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslClientCertPrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mPool));
        }

		public void AddPromptProvider(SvnAuthProviderObject.SslClientCertPwPrompt promptFunc,
									  IntPtr promptBaton, int retryLimit)
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mAuthObjs.Add(SvnAuthProviderObject.GetPromptProvider(promptFunc, promptBaton, 
        														  retryLimit, mPool));
        }
        
        public void OpenAuth()
        {
        	if( mAuthObjs == null )
				mAuthObjs = new ArrayList();
        	mContext.AuthBaton = SvnAuthBaton.Open(mAuthObjs,mPool);
        	mAuthObjs = null;
        }
       	#endregion
    	
    	#region Member access throught properties
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