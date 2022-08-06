namespace DataSizeEstimator;

interface IConcatenableType
{
    IConcatenableType Concat(IConcatenableType toConcatWith);
    string GetType();
    object GetValue();
}