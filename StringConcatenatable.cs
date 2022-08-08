namespace DataSizeEstimator;

class StringConcatenatable : IConcatenableType
{
    public int max;
    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        return toConcatWith.Concat(this);
    }

    public object GetValue()
    {
        var str = Extensions.NextStrings(Random.Shared , (0, max)).First();
        return str;
    }

    public Type GetUnderlyingType()
    {
        return typeof(string);
    }
}