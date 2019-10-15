using System;

public static class ArrayExtension
{
    public static string[] ToStringArray<T>(this T[] arr) // where T : PlayerData
    {
        if (arr == null)
            return null;

        string[] sarr = new string[arr.Length];
        for (int i = 0; i < sarr.Length; i++)
        {
            if (typeof(T) == typeof(PlayerData))
                sarr[i] = (arr[i] as PlayerData).Name;
            else
                sarr[i] = arr[i].ToString();
        }

        return sarr;
    }

    public static int FindIndex<T>(this T[] arr, Predicate<T> predicate)
    {
        if (arr == null)
            return -1;

        for (int i = 0; i < arr.Length; i++)
        {
            if (predicate(arr[i]))
                return i;
        }

        return -1;
    }
}