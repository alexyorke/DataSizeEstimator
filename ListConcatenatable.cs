using System.Collections;
using System.Collections.Generic;

namespace DataSizeEstimator;

class ListConcatenatable<T> : IConcatenableType, IList<T>
{
    public List<T> val = new List<T>();
    public int max;
    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        var t = toConcatWith.GetUnderlyingType().Name;
        var u = toConcatWith.GetValue();
        return t switch
        {
            "String" => new ListConcatenatable<List<string>>
            {
                Enumerable.Range(0, max).Select(x => (string)toConcatWith.GetValue()).ToList()
            },
            "Integer" => new ListConcatenatable<List<int>>
            {
                Enumerable.Range(0, max).Select(x => (int)toConcatWith.GetValue()).ToList()
            },
            _ => new ListConcatenatable<object>
            {
                Enumerable.Range(0, max).Select(x => toConcatWith.GetValue()).ToList()
            }
        };
    }

    public IEnumerator<T> GetEnumerator()
    {
        return val.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public object GetValue()
    {
        return val;
    }

    public Type GetUnderlyingType()
    {
        return typeof(IList<T>);
    }

    public void Add(T item)
    {
        val.Add(item);
    }

    public void Clear()
    {
        val.Clear();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public T this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
}