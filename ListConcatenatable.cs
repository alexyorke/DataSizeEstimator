using System.Collections;
using System.Collections.Generic;

namespace DataSizeEstimator;

class ListConcatenatable<T> : IConcatenableType, IEnumerable<T>
{
    private List<T> val = new();
    private readonly int max;
    private readonly int min;

    public ListConcatenatable(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    private ListConcatenatable() { }

    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        var t = toConcatWith.GetUnderlyingType().Name;
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
}