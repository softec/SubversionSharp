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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;

namespace Softec.AprSharp
{
    public unsafe struct AprArray : IEnumerable
    {
        private apr_array_header_t *mArray;
        private Type mEltsType;

        [StructLayout( LayoutKind.Sequential )]
        private struct apr_array_header_t
        {
    		public IntPtr pool;
			public int elt_size;
			public int nelts;
			public int nalloc;
			public IntPtr elts;
        }

        #region Generic embedding functions of an IntPtr
        private AprArray(apr_array_header_t *ptr)
        {
            mArray = ptr;
            mEltsType = null;
        }

        public AprArray(IntPtr ptr)
        {
            mArray = (apr_array_header_t *)ptr.ToPointer();
            mEltsType = null;
        }

        public AprArray(IntPtr ptr, Type eltsType)
        {
            mArray = (apr_array_header_t *)ptr.ToPointer();
            mEltsType = null;
            if( eltsType != null )
            	ElementType = eltsType;
        }
        
        public bool IsNull
        {
        	get
        	{
            	return( mArray == null );
            }
        }

        private void CheckPtr()
        {
            if( mArray == null )
                throw new AprNullReferenceException(); 
        }

        public void ClearPtr()
        {
            mArray = null;
            mEltsType = null;
        }

        public static implicit operator IntPtr(AprArray array)
        {
            return new IntPtr(array.mArray);
        }
        
        public static implicit operator AprArray(IntPtr ptr)
        {
            return new AprArray(ptr);
        }
        
        public override string ToString()
        {
            return("[apr_array_header_t:"+(new IntPtr(mArray)).ToInt32().ToString("X")+"]");
        }
        
        public Type ElementType
        {
        	get
        	{
        		return(mEltsType);
        	}
        	set
        	{
	   			if( (value.IsPrimitive && Marshal.SizeOf(value) != mArray->elt_size)
	   			  ||(!value.IsPrimitive && Marshal.SizeOf(typeof(IntPtr)) != mArray->elt_size) )
	            		throw new AprArgumentException("Type does not match element size.");
	            		
	            if( value.IsPrimitive )
	            	mEltsType = value;
	            else {
	            	if( value.GetConstructor(new Type[] {typeof(IntPtr)}) == null )
	            		throw new AprInvalidOperationException("Type is not primitive and cannot be constructed from an IntPtr.");
	            	mEltsType = value;
	            }
        	}
        }
        #endregion
        
        #region Methods wrappers
        public static AprArray Make(AprPool pool, int nElts, Type eltsType )
        {
        	if(eltsType.IsPrimitive)
        		return(new AprArray(Make(pool, nElts, Marshal.SizeOf(eltsType)),eltsType));
        	else
        		return(new AprArray(Make(pool, nElts, Marshal.SizeOf(typeof(IntPtr))),eltsType));
        }

        public static AprArray Make(AprPool pool, int nElts, int eltSize )
        {
    	    IntPtr ptr;
            
            Debug.Write(String.Format("apr_array_make({0},{1},{2})...",pool,nElts,eltSize));
            ptr = Apr.apr_array_make(pool,nElts,eltSize);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_make: Can't create an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }

        public AprArray Copy(AprPool pool)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_copy({0},{1})...",pool,this));
            ptr = Apr.apr_array_copy(pool,(IntPtr)mArray);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_copy: Can't copy an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(new AprArray(ptr,mEltsType));
        }

        public AprArray CopyHdr(AprPool pool)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_copy_hdr({0},{1})...",pool,this));
            ptr = Apr.apr_array_copy_hdr(pool,new IntPtr(mArray));
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_copy_hdr: Can't copy an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(new AprArray(ptr,mEltsType));
        }

        public AprArray Append(AprPool pool, AprArray array)
        {
    	    IntPtr ptr;
    	    Type arrType;
            
            CheckPtr();
            if (mEltsType != null && array.mEltsType != null && mEltsType != array.mEltsType)
            	throw new AprInvalidOperationException("Array type mismatch.");
            
            if(mEltsType == null && array.mEltsType != null)
            	arrType = array.mEltsType;
           	arrType = mEltsType;
            	
            Debug.Write(String.Format("apr_array_append({0},{1},{2})...",pool,array,this));
            ptr = Apr.apr_array_append(pool,(IntPtr)mArray,array);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_append: Can't append an apr_array_header_t");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(new AprArray(ptr,arrType));
        }

        public void Cat(AprArray array)
        {
    	    Type arrType;
            
            CheckPtr();
            if (mEltsType != null && array.mEltsType != null && mEltsType != array.mEltsType)
            	throw new AprInvalidOperationException("Array type mismatch.");
            
            if(mEltsType == null && array.mEltsType != null)
            	mEltsType = array.mEltsType;
            	
            Debug.WriteLine(String.Format("apr_array_cat({0},{1})",this,array));
            Apr.apr_array_cat((IntPtr)mArray,array);
        }        

        public string StrCat(AprPool pool, char sep)
        {
        	return(pStrCat(pool,sep).ToString());
        }

        public AprString pStrCat(AprPool pool, char sep)
        {
    	    IntPtr ptr;
            
            CheckPtr();
            if(mEltsType != null && mEltsType != typeof(AprString))
            	throw new AprInvalidOperationException("Not an AprString array.");
            	
            Debug.Write(String.Format("apr_array_pstrcat({0},{1},{2})...",pool,this,sep));
            ptr = Apr.apr_array_pstrcat(pool,new IntPtr(mArray),sep);
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_pstrcat: Can't convert an apr_array_header_t to AprString");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }        

        public void Push(byte o)
        {
            CheckPtr();
        	Marshal.WriteByte(Push(),o);
        }

        public void Push(short o)
        {
            CheckPtr();
	        Marshal.WriteInt16(Push(),o);
	    }
	    
        public void Push(int o)
        {
            CheckPtr();
	        Marshal.WriteInt32(Push(),o);
	    }

        public void Push(long o)
        {
            CheckPtr();
	    	Marshal.WriteInt64(Push(),o);
		}
		
		public void Push(IntPtr ptr)
		{
            CheckPtr();
		    Marshal.WriteIntPtr(Push(),ptr);
        }
        
        public IntPtr Push()
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_push({0})...",this));
            ptr = Apr.apr_array_push(new IntPtr(mArray));
            if(ptr == IntPtr.Zero )
                throw new AprException("apr_array_push: Can't push an element");
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }
        
        public object PopObject()
		{
			if(mEltsType == null)        
            	throw new AprInvalidOperationException("Array not typed.");
            	
			if(mEltsType.IsPrimitive)
			{
				object val;
	           	switch(ElementSize)
	           	{
	           		case 1:
	           			val = Marshal.ReadByte(Pop());
	           			break;
	           		case 2:
	           			val = Marshal.ReadInt16(Pop());
	           			break;
	           		case 4:
	           			val = Marshal.ReadInt32(Pop());
	           			break;
	           		case 8:
	           			val = Marshal.ReadInt64(Pop());
	           			break;
	           		default:
            			throw new AprInvalidOperationException("Invalid element size.");
		        }
		        return(Convert.ChangeType(val,mEltsType));
			}
			
        	ConstructorInfo ctor = mEltsType.GetConstructor(new Type[] {typeof(IntPtr)});
        	if( ctor == null )
            	throw new AprInvalidOperationException("Type is not primitive and cannot be constructed from an IntPtr.");
            
            IntPtr ptr = Marshal.ReadIntPtr(Pop());
			return(ctor.Invoke(new Object[] { ptr }));
		}        
        
        public IntPtr Pop()
        {
    	    IntPtr ptr;
            
            CheckPtr();
            Debug.Write(String.Format("apr_array_pop({0})...",this));
            ptr = Apr.apr_array_pop(new IntPtr(mArray));
            Debug.WriteLine(String.Format("Done({0:X})",((Int32)ptr)));

            return(ptr);
        }        
        
        public bool IsEmpty()
        {
        	bool isEmpty;
            CheckPtr();
            Debug.Write(String.Format("apr_is_empty_array({0})...",this));
            isEmpty = Apr.apr_is_empty_array(new IntPtr(mArray));
            Debug.WriteLine(String.Format("Done({0:X})",isEmpty));
            return(isEmpty);
        }
        #endregion

        #region Wrapper Properties
        public AprPool Pool
        {
            get {
                return(mArray->pool);
            }
        }        

        public int AllocatedCount
        {
            get {
                return(mArray->nalloc);
            }
        }
        
        public int ElementSize
        {
            get {
                return(mArray->elt_size);
            }
        }
        
        public IntPtr Data
        {
            get {
                return(mArray->elts);
            }
        }
        #endregion
        
        #region ICollection
        public void CopyTo(Array array, int arrayIndex)
        {
            if(null == array)
                throw new AprArgumentNullException("array");
            if(arrayIndex < 0 || arrayIndex > array.Length)
                throw new AprArgumentOutOfRangeException("arrayIndex");
            if(array.Rank > 1)
                throw new AprArgumentException("array is multidimensional");
            if((array.Length - arrayIndex) < Count)
                throw new AprArgumentException("Not enough room from arrayIndex to end of array for this AprArray");
            
            int i = arrayIndex;
            IEnumerator it = GetEnumerator();
            while(it.MoveNext()) {
                array.SetValue(it.Current, i++);
            }
        }
        
        public bool IsSynchronized 
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot 
        {
            get
            {
                return this;
            }
        }

        public int Count
        {
            get {
                return(mArray->nelts);
            }
        }
        #endregion       
        
        #region IEnumerable
	    public IEnumerator GetEnumerator()
        {
            return (IEnumerator) new AprArrayEnumerator(this);
        }
        #endregion

  	}
  	
  	public class AprArrayEnumerator : IEnumerator
    {
    	AprArray mArray;
    	int mIndex;
    
        public AprArrayEnumerator(AprArray array)
        {
            mArray = array;
            mIndex = -1;
        }
        
 		#region IEnumerator
  		public bool MoveNext()
  		{
  			if (++mIndex >= mArray.Count) {
  				mIndex = mArray.Count;
  				return(false);
  			}
  			return(true);
  		}
  		
  		public void Reset()
  		{
  		    mIndex = -1;
  		}
  		  		
  		public object Current
  	    {
            get
            {
            	if (mIndex < 0 || mIndex >= mArray.Count)
            		throw new AprInvalidOperationException("No current item.");
                
	   			if(mArray.ElementType == null)
	            	throw new AprInvalidOperationException("Array not typed.");
	            	            
				if(mArray.ElementType.IsPrimitive)
				{
					object val;
		           	switch(mArray.ElementSize)
		           	{
		           		case 1:
		           			val = Marshal.ReadByte(mArray.Data,mIndex);
		           			break;
		           		case 2:
		           			val = Marshal.ReadInt16(mArray.Data,mIndex*2);
		           			break;
		           		case 4:
		           			val = Marshal.ReadInt32(mArray.Data,mIndex*4);
		           			break;
		           		case 8:
		           			val = Marshal.ReadInt64(mArray.Data,mIndex*8);
		           			break;
		           		default:
	            			throw new AprInvalidOperationException("Invalid element size.");
			        }
			        return(Convert.ChangeType(val,mArray.ElementType));
				}
				
	        	ConstructorInfo ctor = mArray.ElementType.GetConstructor(new Type[] {typeof(IntPtr)});
	        	if( ctor == null )
	            	throw new AprInvalidOperationException("Type is not primitive and cannot be constructed from an IntPtr.");
	            
	            IntPtr ptr = Marshal.ReadIntPtr(mArray.Data,mIndex*mArray.ElementSize);
				return(ctor.Invoke(new Object[] { ptr }));
            }
  	    }
  	    #endregion

  	}
}