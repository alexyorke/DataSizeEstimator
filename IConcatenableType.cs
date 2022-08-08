using System.Runtime.CompilerServices;

namespace DataSizeEstimator;

interface IConcatenableType
{
    IConcatenableType Concat(IConcatenableType toConcatWith);
    object GetValue();
    Type GetUnderlyingType();
}