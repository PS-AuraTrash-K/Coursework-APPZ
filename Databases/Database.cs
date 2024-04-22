using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Databases;

public class Database
{
    public const string Salt = "!!SALT!!";
    private static Database _instance;
    
    private readonly List<User> _users = FileManager.ReadJson();
    private User _currentUser;

    public const string DefaultPassword = "12345";

    private Database() { }

    public static Database GetInstance() => _instance ??= new Database();

    public User GetUser(string login, string password)
    {
        _currentUser =
            _users.FirstOrDefault(u => u.Login == login && u.Password == Cipher.Encode(password));
        return _currentUser;
    }

    public void ReloadRequests()
    {
        foreach (var requestText in FileManager.ReadBytes())
        {
            var parts = Regex.Split(requestText, Salt);
            
            var author = DecoratorHandler.Decorate(GetUser(parts[parts.Length - 1]));
            Request.RequestByText(parts, author);
            author.Reset();
        }
    }
    
    private User GetUser(string id)
    {
        return _users.FirstOrDefault(u => u.Id == int.Parse(id));;
    }
    
    public bool CheckFreeLogin(string login) => _users.All(u => u.Login != login);

    public bool CheckLogin(string login) => _currentUser.Login == login;

    public bool CheckPassword(string password) => _currentUser.Password == Cipher.Encode(password);

    private void RemoveUser(User target)
    {
        var removableUser = _users.FirstOrDefault(u => u.Id == target.Id);
        _users.Remove(removableUser);
    }

    public void SetPrivateProperty(UserPrivateProps prop, string newValue)
    {
        var newUser = prop == UserPrivateProps.Login
            ? new User(
                _currentUser.Id,
                new Dictionary<UserPrivateProps, string>
                {
                    { UserPrivateProps.Login , newValue},
                    { UserPrivateProps.Password , _currentUser.Password}
                },
                new Dictionary<UserPublicProps, string>
                {
                    {UserPublicProps.Name , _currentUser.Name},
                    {UserPublicProps.Surname , _currentUser.Surname},
                    {UserPublicProps.Age , _currentUser.Age},
                    {UserPublicProps.Number , _currentUser.Number}
                },
                _currentUser.Posts.Select(p => (int)p))
            : new User(
                _currentUser.Id,
                new Dictionary<UserPrivateProps, string>
                {
                    { UserPrivateProps.Login , _currentUser.Login},
                    { UserPrivateProps.Password , Cipher.Encode(newValue)}
                },
                new Dictionary<UserPublicProps, string>
                {
                    {UserPublicProps.Name , _currentUser.Name},
                    {UserPublicProps.Surname , _currentUser.Surname},
                    {UserPublicProps.Age , _currentUser.Age},
                    {UserPublicProps.Number , _currentUser.Number}
                },
                _currentUser.Posts.Select(p => (int)p));
    
        RemoveUser(_currentUser);
    
        SetUser(newUser);
    
        _currentUser = newUser;
    }
    
    public void SetUser(Dictionary<UserPublicProps, string> dictionary, IEnumerable<UserPosts> queue) =>
        SetUser(
            User.Count + 1,
            new Dictionary<UserPrivateProps, string>
            {
                {UserPrivateProps.Login, (User.Count + 1).ToString()},
                {UserPrivateProps.Password, Cipher.Encode(DefaultPassword)}
            },
            dictionary,
            queue
        );

    private void SetUser(User user) =>
        SetUser(
            user.Id,
            new Dictionary<UserPrivateProps, string>
            {
                {UserPrivateProps.Login , user.Login},
                {UserPrivateProps.Password , user.Password}
            },
            new Dictionary<UserPublicProps, string>
            {
                {UserPublicProps.Name , user.Name},
                {UserPublicProps.Surname , user.Surname},
                {UserPublicProps.Age , user.Age},
                {UserPublicProps.Number , user.Number}
            },
            user.Posts
        );

    private void SetUser(int id, Dictionary<UserPrivateProps, string> privateInfo,
        Dictionary<UserPublicProps, string> info,
        IEnumerable<UserPosts> posts)
    {
        _users.Add(new User(
            id,
            privateInfo,
            info,
            posts.Select(p => (int)p)
        ));
        
        _users.Sort((first, second) => first.Id - second.Id);
        
        FileManager.WriteJson(_users);
    }
}


internal static class Cipher
{
    private const string Salt = "keyMaybe";

    public static string Encode(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        using var sha = new SHA256Managed();
        
        var textBytes = System.Text.Encoding.UTF8.GetBytes(text + Salt);
        var hashBytes = sha.ComputeHash(textBytes);
            
        var hash = BitConverter
            .ToString(hashBytes)
            .Replace("-", string.Empty);

        return hash;
    }
    
    public static byte[] Encrypt(string text, byte[] key, byte[] iv)
    {
        byte[] cipheredtext;
        
        using var aes = Aes.Create();
        
        var encryptor = aes.CreateEncryptor(key, iv);

        using var memoryStream = new MemoryStream();
        
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(text);
        }
 
        cipheredtext = memoryStream.ToArray();

        return cipheredtext;
    }
    
    public static string Decrypt(byte[] cipheredtext, byte[] key, byte[] iv)
    {
        string text;
       
        using var aes = Aes.Create();
        
        var decryptor = aes.CreateDecryptor(key, iv);
        
        using var memoryStream = new MemoryStream(cipheredtext);
        
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        
        using var streamReader = new StreamReader(cryptoStream);
        
        text = streamReader.ReadToEnd();

        return text;
    }
}