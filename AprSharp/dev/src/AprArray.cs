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
    public unsafe struct AprArray : IEnumerable, ICollection, IAprUnmanaged
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

        private void CheckPtr(Type type)
        {
            if( mArray == null )
                throw new AprNullReferenceException();
            if( mEltsType != null && mEltsType != type )
            	throw new AprInvalidOperationException(String.Format("Types mismatch between array ({0}) and value {1}.",mEltsType.Name,type.Name));
        }
        
        public void ClearPtr()
        {
            mArray = null;
            mEltsType = null;
        }

        public IntPtr ToIntPtr()
        {
            return new IntPtr(mArray);
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
	            	if( value != typeof(IntPtr) && value.GetConstructor(new Type[] {typeof(IntPtr)}) == null )
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

        public static AprArray Make(AprPool pool, ICollection list )
        {
        	if(list is AprArray)
        		//return(((AprArray)list).Copy(pool));
        		return (AprArray)list;
        	else
        	{
	            IEnumerator it = list.GetEnumerator();
	            it.MoveNext();
	            
	            AprArray a = Make(pool, list.Count, it.Current.GetType());
	            it.Reset();
	            
	            while(it.MoveNext()) {
	                a.Push(it.Current);
	            }
	            return(a);
			}
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
            CheckPtr();
            if (mEltsType != null && array.mEltsType != null && mEltsType != array.mEltsType)
            	throw new AprInvalidOperationException("Array types mismatch.");
            
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

        public void Push(bool o)
        {
            CheckPtr(typeof(bool));
        	Marshal.WriteByte(Push(),(byte)((o) ? 1 : 0));
        }

        public void Push(byte o)
        {
            CheckPtr(typeof(byte));
        	Marshal.WriteByte(Push(),o);
        }

		[CLSCompliant(false)]
        public void Push(sbyte o)
        {
            CheckPtr(typeof(sbyte));
        	Marshal.WriteByte(Push(),unchecked((byte)o));
        }
        
        public void Push(short o)
        {
            CheckPtr(typeof(short));
        	Marshal.WriteInt16(Push(),o);
        }

		[CLSCompliant(false)]
        public void Push(ushort o)
        {
            CheckPtr(typeof(ushort));
	        Marshal.WriteInt16(Push(),unchecked((short)o));
	    }
	    
        public void Push(int o)
        {
            CheckPtr(typeof(int));
	        Marshal.WriteInt32(Push(),o);
	    }

		[CLSCompliant(false)]
        public void Push(uint o)
        {
            CheckPtr(typeof(uint));
	        Marshal.WriteInt32(Push(),unchecked((int)o));
	    }

        public void Push(long o)
        {
            CheckPtr(typeof(long));
	    	Marshal.WriteInt64(Push(),o);
		}
		
		[CLSCompliant(false)]
        public void Push(ulong o)
        {
            CheckPtr(typeof(ulong));
	    	Marshal.WriteInt64(Push(),unchecked((long)o));
		}
		
		public void Push(IntPtr ptr)
		{
            CheckPtr();
		    Marshal.WriteIntPtr(Push(),ptr);
        }
        
        public void Push(object o)
        {
        	if(mEltsType.IsPrimitive)
        	{
	        	if(mEltsType == typeof(bool))
	        		Push((bool)o);
	        	else if(mEltsType == typeof(byte))
	        		Push((byte)o);
	        	else if(mEltsType == typeof(sbyte))
	        		Push((sbyte)o);
	        	else if(mEltsType == typeof(short))
	        		Push((short)o);
	        	else if(mEltsType == typeof(ushort))
	        		Push((ushort)o);
	        	else if(mEltsType == typeof(int))
	        		Push((int)o);
	        	else if(mEltsType == typeof(uint))
	        		Push((uint)o);
	        	else if(mEltsType == typeof(long))
	        		Push((long)o);
	        	else if(mEltsType == typeof(ulong))
	        		Push((ulong)o);
	        	else
	            	throw new AprInvalidOperationException("Array type not supported.");
	    	}
	    	else
	    	{
	        	if(mEltsType == typeof(IntPtr))
	        		Push((IntPtr)o);
	        	else
	        	{
	    			IAprUnmanaged obj = o as IAprUnmanaged;
	    			if( o == null )
	            		throw new AprInvalidOperationException("Array type should implement IAprUnmanaged.");
	            	Push(obj.ToIntPtr());
	            }
	        }
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
		        if(mEltsType == val.GetType())
		        	return(val);
		        else if (mEltsType == typeof(bool))
		        	return(Convert.ChangeType(val,typeof(bool)));
		        else if (mEltsType == typeof(sbyte))
		        	return(unchecked((sbyte)((byte)val)));
		        else if (mEltsType == typeof(ushort))
		        	return(unchecked((ushort)((short)val)));
		        else if (mEltsType == typeof(uint))
		        	return(unchecked((uint)((int)val)));
		        else if (mEltsType == typeof(ulong))
		        	return(unchecked((ulong)((long)val)));
		        return(val);
			}
			if(mEltsType == typeof(IntPtr))
			{
	            return(Marshal.ReadIntPtr(Pop()));
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
			        
			        if(mArray.ElementType == val.GetType())
			        	return(val);
			        else if (mArray.ElementType == typeof(bool))
			        	return(unchecked(Convert.ChangeType(val,typeof(bool))));
			        else if (mArray.ElementType == typeof(sbyte))
			        	return(unchecked((sbyte)((byte)val)));
			        else if (mArray.ElementType == typeof(ushort))
			        	return(unchecked((ushort)((short)val)));
			        else if (mArray.ElementType == typeof(uint))
			        	return(unchecked((uint)((int)val)));
			        else if (mArray.ElementType == typeof(ulong))
			        	return(unchecked((ulong)((long)val)));
			        return(val);
				}
				if(mArray.ElementType == typeof(IntPtr))
				{
		            return(Marshal.ReadIntPtr(mArray.Data,mIndex*mArray.ElementSize));
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