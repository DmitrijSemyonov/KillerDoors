using UnityEngine;

namespace KillerDoors.Data.Extensions
{
    public static class DataExtensions
    {
        public static T DeserializeTo<T>(this string json) =>
            JsonUtility.FromJson<T>(json);
        public static string ToJson<T>(this T data) =>
            JsonUtility.ToJson(data);
    }
}