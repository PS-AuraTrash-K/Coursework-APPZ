using System;
using System.Collections.Generic;
using System.Linq;
using APPZ.Enums;

namespace APPZ.Databases;

public static class StringCoding
{
    public static string Encode(Dictionary<UserPublicProps, string> obj) =>
        obj.Aggregate("", (result, pair) =>
            result + $"{pair.Value.Replace(" ", "")} ");

    public static string Encode(List<string> obj) =>
        obj.Aggregate("", (result, str) =>
            result + $"{str.Replace(" ", "")} ");

    public static Dictionary<UserPublicProps, string> DecodeToDictionary(string obj)
    {
        var dictionary = new Dictionary<UserPublicProps, string>();
        var array = obj.Split(' ');
        
        var index = 0;
        foreach (var userProp in Enum.GetValues(typeof(UserPublicProps)).OfType<UserPublicProps>())
        {
            dictionary.Add(userProp, array[index]);
            index++;
        }
        
        return dictionary;
    }
    
    public static List<string> DecodeToList(string obj) =>
        obj.Split(' ').ToList();
}