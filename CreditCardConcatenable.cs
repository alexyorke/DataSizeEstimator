namespace DataSizeEstimator;

class CreditCardConcatenable : IConcatenableType
{
    private string str;

    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        return toConcatWith.Concat(this);
    }

    public object GetValue()
    {
        return string.Join("", Enumerable.Repeat("1", 16));
    }

    public Type GetUnderlyingType()
    {
        return typeof(string);
    }
}