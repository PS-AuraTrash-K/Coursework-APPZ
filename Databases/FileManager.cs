using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using APPZ.Decorator.Users;
using Newtonsoft.Json;

namespace APPZ.Databases;

public static class FileManager
{
    public static List<User> ReadJson()
    {
        var path = GetPath() + @"\Users\Users.json";
            
        var result = new List<User>();
            
        result.AddRange(JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(path)));
            
        return result;
    }
    
    public static void WriteJson<T>(List<T> users)
    {
        var path = GetPath() + @"\Users\Users.json";
        
        File.WriteAllText(path,JsonConvert.SerializeObject(users));
    }
    
    public static void WriteBytes(string text, string fileName)
    {
        var key = new byte[16];
        var iv = new byte[16];
 
        using(var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(key);
            rng.GetBytes(iv);
        }

        var encrypted = Cipher.Encrypt(text, key, iv);;
        var byteArray = new byte[key.Length + iv.Length + encrypted.Length];
        
        for (int i = 0; i < byteArray.Length; i++)
        {
            if (i < key.Length)
                byteArray[i] = key[i];
            else if (i < key.Length + encrypted.Length)
                byteArray[i] = encrypted[i - key.Length];
            else if (i < key.Length + encrypted.Length + iv.Length)
                byteArray[i] = iv[i - (key.Length + encrypted.Length)];
        }
        
        var path = GetPath() + $@"\Requests\{fileName}";
        
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        fs.Write(byteArray, 0, byteArray.Length);
    }

    public static List<string> ReadBytes()
    {
        var path = GetPath() + @"\Requests\";
        var files = Directory.GetFiles(path);

        return files.Select(ReadByte).ToList();
    }

    public static void DeleteByte(string fileName)
    {
        var path = GetPath() + $@"\Requests\{fileName}";
        
        if (File.Exists(path))
            File.Delete(path);
    }
    
    private static string ReadByte(string fileName)
    {
        var byteArray = File.ReadAllBytes(fileName);
        var key = new byte[16];
        var iv = new byte[16];
        
        var encrypted = new byte[byteArray.Length - (key.Length + iv.Length)];
        
        for (int i = 0; i < byteArray.Length; i++)
        {
            if (i < key.Length)
                key[i] = byteArray[i];
            else if (i < key.Length + encrypted.Length)
                encrypted[i - key.Length] = byteArray[i];
            else if (i < key.Length + encrypted.Length + iv.Length)
                iv[i - (key.Length + encrypted.Length)] = byteArray[i];
        }
        
        return Cipher.Decrypt(encrypted, key, iv);
    }

    private static string GetPath()
    {
        var dir = Directory.GetParent(Environment.CurrentDirectory);
        if (dir == null)
            throw new Exception("Database doesn't exist");

        return dir.Parent!.FullName + @"\Databases\Files";
    }
}
