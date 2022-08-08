namespace DataSizeEstimator;

class StringConcatenatable : IConcatenableType
{
    private string str;
    private readonly int min;
    private readonly int max;

    public StringConcatenatable(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        return toConcatWith.Concat(this);
    }

    public object GetValue()
    {
        return Random.Shared.NextStrings((min, max)).First();
    }

    public Type GetUnderlyingType()
    {
        return typeof(string);
    }
}