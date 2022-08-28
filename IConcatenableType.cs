using System.Runtime.CompilerServices;

namespace DataSizeEstimator;

public interface IConcatenableType
{
    IConcatenableType Concat(IConcatenableType toConcatWith);
    object GetValue();
    Type GetUnderlyingType();
}