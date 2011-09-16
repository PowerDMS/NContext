// Type: System.Collections.ObjectModel.Collection`1
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
  [ComVisible(false)]
  [DebuggerTypeProxy(typeof (Mscorlib_CollectionDebugView<>))]
  [DebuggerDisplay("Count = {Count}")]
  [Serializable]
  public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
  {
    private IList<T> items;
    [NonSerialized]
    private object _syncRoot;

    public int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        return this.items.Count;
      }
    }

    protected IList<T> Items
    {
      get
      {
        return this.items;
      }
    }

    public T this[int index]
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        return this.items[index];
      }
      set
      {
        if (this.items.IsReadOnly)
          ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        if (index < 0 || index >= this.items.Count)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        this.SetItem(index, value);
      }
    }

    bool ICollection<T>.IsReadOnly
    {
      get
      {
        return this.items.IsReadOnly;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
        {
          ICollection collection = this.items as ICollection;
          if (collection != null)
            this._syncRoot = collection.SyncRoot;
          else
            Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        }
        return this._syncRoot;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return this.items.IsReadOnly;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        IList list = this.items as IList;
        if (list != null)
          return list.IsFixedSize;
        else
          return this.items.IsReadOnly;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public Collection()
    {
      this.items = (IList<T>) new List<T>();
    }

    public Collection(IList<T> list)
    {
      if (list == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
      this.items = list;
    }

    public void Add(T item)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      this.InsertItem(this.items.Count, item);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public void Clear()
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      this.ClearItems();
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public void CopyTo(T[] array, int index)
    {
      this.items.CopyTo(array, index);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public bool Contains(T item)
    {
      return this.items.Contains(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.items.GetEnumerator();
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOf(T item)
    {
      return this.items.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      if (index < 0 || index > this.items.Count)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
      this.InsertItem(index, item);
    }

    public bool Remove(T item)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      int index = this.items.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveItem(index);
      return true;
    }

    public void RemoveAt(int index)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      if (index < 0 || index >= this.items.Count)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      this.RemoveItem(index);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    protected virtual void ClearItems()
    {
      this.items.Clear();
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    protected virtual void InsertItem(int index, T item)
    {
      this.items.Insert(index, item);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    protected virtual void RemoveItem(int index)
    {
      this.items.RemoveAt(index);
    }

    protected virtual void SetItem(int index, T item)
    {
      this.items[index] = item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.items.GetEnumerator();
    }

    void ICollection.CopyTo(Array array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      if (array.GetLowerBound(0) != 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      T[] array1 = array as T[];
      if (array1 != null)
      {
        this.items.CopyTo(array1, index);
      }
      else
      {
        Type elementType = array.GetType().GetElementType();
        Type c = typeof (T);
        if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        object[] objArray = array as object[];
        if (objArray == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        int count = this.items.Count;
        try
        {
          for (int index1 = 0; index1 < count; ++index1)
            objArray[index++] = (object) this.items[index1];
        }
        catch (ArrayTypeMismatchException ex)
        {
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        }
      }
    }

    object IList.get_Item(int index)
    {
      return (object) this.items[index];
    }

    void IList.set_Item(int index, object value)
    {
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
      try
      {
        this[index] = (T) value;
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (T));
      }
    }

    int IList.Add(object value)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
      try
      {
        this.Add((T) value);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (T));
      }
      return this.Count - 1;
    }

    bool IList.Contains(object value)
    {
      if (Collection<T>.IsCompatibleObject(value))
        return this.Contains((T) value);
      else
        return false;
    }

    int IList.IndexOf(object value)
    {
      if (Collection<T>.IsCompatibleObject(value))
        return this.IndexOf((T) value);
      else
        return -1;
    }

    void IList.Insert(int index, object value)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
      try
      {
        this.Insert(index, (T) value);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (T));
      }
    }

    void IList.Remove(object value)
    {
      if (this.items.IsReadOnly)
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      if (!Collection<T>.IsCompatibleObject(value))
        return;
      this.Remove((T) value);
    }

    private static bool IsCompatibleObject(object value)
    {
      if (value is T)
        return true;
      if (value == null)
        return (object) default (T) == null;
      else
        return false;
    }
  }
}
