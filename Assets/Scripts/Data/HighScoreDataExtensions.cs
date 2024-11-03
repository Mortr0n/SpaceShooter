using System;
using UnityEngine;


public static class HighScoreDataExtensions
{
    public static HighScoreData[] SortHighScores(this HighScoreData[] data)
    {
        Array.Sort(data, (a, b) =>
        {
            if (a == null && b == null) return 0;
            if (a == null) return 1;
            if (b == null) return -1;
            return b.Level.CompareTo(a.Level);
        });
        return data;
    }
}
