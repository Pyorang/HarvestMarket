public static class NumberExtensions
{
    private static readonly string[] _suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    public static string ToFormattedString(this double value)
    {
        if (value < 1000)
        {
            return value.ToString("F0");
        }

        int suffixIndex = 0;
        double displayValue = value;

        while (displayValue >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000;
            suffixIndex++;
        }

        if (displayValue < 10)
        {
            return $"{displayValue:F2}{_suffixes[suffixIndex]}";
        }
        else if (displayValue < 100)
        {
            return $"{displayValue:F1}{_suffixes[suffixIndex]}";
        }
        else
        {
            return $"{displayValue:F0}{_suffixes[suffixIndex]}";
        }
    }
}
