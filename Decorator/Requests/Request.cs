using System;
using System.Collections.Generic;
using System.Linq;
using APPZ.Databases;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public abstract class Request
{
    private readonly IUser _author;
    public string Comment { get; set; }
    public RequestStatus Status { get; set; }

    private static int _count;
    
    public int Id { get; private set; }

    protected Request(IUser author)
    {
        _count++;
        Id = _count;
        _author = author;
        Comment = "";
    }

    public static void RequestByText(string[] parts, IUser author)
    {
        Dictionary<string, string> combined = new Dictionary<string, string>();
        
        Request req;

        for (int i = 0; i < parts.Length; i++)
        {
            if (i % 2 != 0) continue;
            combined.Add(parts[i], parts[i + 1]);
        }

        req = combined["Type"] switch
        {
            "Budget" => new RequestBudget(combined, author),
            "Personnel" => new RequestPersonnel(combined, author),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Enum.TryParse(combined["Status"], out RequestStatus status);
        req.Status = status;
        req.Comment = combined["Comment"];
        req.Id = int.Parse(combined["Id"]);
        _count = req.Id;
        
        PoolManager.GetInstance().SetRequestPool(req);
    }

    public abstract Dictionary<RequestProps, string> GetProperties();

    public Dictionary<UserPublicProps, string> GetAuthorProperties() => _author.GetProperties();

    public int GetAuthorId() => _author.GetId();

    public IUser GetAuthor() => _author;
    
    public abstract RequestType GetRequestType();

    public void Save()
    {
        var saved = $"Type{Database.Salt}" + GetRequestType();
        saved = GetProperties().Aggregate(saved, (current, pair) => current + (Database.Salt + pair.Key + Database.Salt + pair.Value));
        saved +=
            $"{Database.Salt}Status{Database.Salt}" + Status + $"{Database.Salt}Comment{Database.Salt}" + Comment
            + $"{Database.Salt}Id{Database.Salt}" + Id + $"{Database.Salt}AuthorId{Database.Salt}" + _author.GetId();
        
        FileManager.WriteBytes(saved, Id.ToString());
    }

    public abstract override string ToString();
}