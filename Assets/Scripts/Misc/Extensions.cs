using System;
using System.Collections.Generic;


public static class Extensions
{
    // https://stackoverflow.com/questions/273313/randomize-a-listt
    static Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while(n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string FormatLargeNumbers(this int amount)
    {
        ulong value = (ulong)amount;
        return value switch
        {
            < 1000 => value.ToString(),
            < 10000 => (value / 1000f).ToString("N2") + "K",
            < 100000 => (value / 1000f).ToString("N1") + "K",
            < 1000000 => (value / 1000f).ToString("N0") + "K",
            < 10000000 => (value / 1000000f).ToString("N2") + "M",
            < 100000000 => (value / 1000000f).ToString("N1") + "M",
            < 1000000000 => (value / 1000000f).ToString("N0") + "M",
            < 10000000000 => (value / 1000000000f).ToString("N2") + "G",
            < 100000000000 => (value / 1000000000f).ToString("N1") + "G",
            < 1000000000000 => (value / 1000000000f).ToString("N0") + "G",
            < 10000000000000 => (value / 1000000000000f).ToString("N2") + "T",
            < 100000000000000 => (value / 1000000000000f).ToString("N1") + "T",
            < 1000000000000000 => (value / 1000000000000f).ToString("N0") + "T",
            < 10000000000000000 => (value / 1000000000000000f).ToString("N2") + "P",
            < 100000000000000000 => (value / 1000000000000000f).ToString("N1") + "P",
            < 1000000000000000000 => (value / 1000000000000000f).ToString("N0") + "P",
            < 10000000000000000000 => (value / 1000000000000000000f).ToString("N2") + "E",
            _ => value.ToString(),
        };
    }
}
