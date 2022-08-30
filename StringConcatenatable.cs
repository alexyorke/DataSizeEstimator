namespace DataSizeEstimator;

class StringConcatenatable : IConcatenableType
{
    private string str;
    private readonly int min;
    private readonly int max;
    private readonly Random random;

    public StringConcatenatable(int min, int max, Random random)
    {
        this.min = min;
        this.max = max;
        this.random = random ?? Random.Shared;
    }
    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        return toConcatWith.Concat(this);
    }

    public object GetValue()
    {
        return NextStrings(random, (min, max)).First();
    }

    public Type GetUnderlyingType()
    {
        return typeof(string);
    }

    public static IEnumerable<string> NextStrings(
        Random rnd,
        (int Min, int Max) length,
        int count = 1)
    {
        var allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        ISet<string> usedRandomStrings = new HashSet<string>();
        (int min, int max) = length;
        char[] chars = new char[max];
        int setLength = allowedChars.Length;

        while (count-- > 0)
        {
            int stringLength = rnd.Next(min, max + 1);

            for (int i = 0; i < stringLength; ++i)
            {
                chars[i] = allowedChars[rnd.Next(setLength)];
            }

            string randomString = new string(chars, 0, stringLength);

            if (usedRandomStrings.Add(randomString))
            {
                yield return randomString;
            }
            else
            {
                count++;
            }
        }
    }
}