using System.Collections.Generic;
using System.Windows.Controls;
using APPZ.Enums;
using Newtonsoft.Json;

namespace APPZ.Decorator.Users;


public class User: IUser
{
    public static int Count { get; private set; }
    public int Id { get; }
    public string Login { get; }
    public string Password { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Age { get; }
    public string Number { get; }
    public List<UserPosts> Posts { get; }

    [JsonConstructor]
    public User(int id, string login, string password,
        string name, string surname, string age, string number, IEnumerable<int> posts)
    : this(id, new Dictionary<UserPrivateProps, string>
    { {UserPrivateProps.Login, login}, {UserPrivateProps.Password, password} },
        new Dictionary<UserPublicProps, string>
    { {UserPublicProps.Name, name}, {UserPublicProps.Surname, surname},
        {UserPublicProps.Age, age}, {UserPublicProps.Number, number} },
        posts) 
    { }

    public User(int id,
        Dictionary<UserPrivateProps, string> privateInfo,
        Dictionary<UserPublicProps, string> info,
        IEnumerable<int> posts)
    {
        Id = id;
    
        if (Count < Id)
            Count = Id;
        
        Login = privateInfo[UserPrivateProps.Login];
        Password = privateInfo[UserPrivateProps.Password];
        Name = info[UserPublicProps.Name];
        Surname = info[UserPublicProps.Surname];
        Age = info[UserPublicProps.Age];
        Number = info[UserPublicProps.Number];
        
        Posts = new List<UserPosts>();
        foreach (var post in posts)
            Posts.Add((UserPosts)post);
        
        Posts.Sort((_, _) => 1);
    }

    public int GetCount() => 0;

    public void Reset() {}

    public List<UserPosts> GetPosts() => Posts;

    public Dictionary<UserFunctions, Button> GetEditableButtons() => null;

    public int GetId() => Id;

    public void CreateButton(Grid grid) { }

    public Dictionary<UserPublicProps, string> GetProperties() => new()
    {
        {UserPublicProps.Name, Name},
        {UserPublicProps.Surname, Surname},
        {UserPublicProps.Age, Age},
        {UserPublicProps.Number, Number}
    };

    public bool Equals(IUser obj) => Id == obj?.GetId();
}
