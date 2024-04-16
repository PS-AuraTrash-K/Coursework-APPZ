using System;
using System.Collections.Generic;
using APPZ.Databases;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class RequestBudget: Request
{
    private readonly string _value;

    private readonly string _description;

    private readonly RequestBudgetType _budgetType;

    public RequestBudget(string value, string description, RequestBudgetType budgetType, IUser author)
        : base(author)
    {
        _budgetType = budgetType;
        _value = value;
        _description = description;
    }
    
    public RequestBudget(Dictionary<string, string> dictionary, IUser author)
        : base(author)
    {
        Enum.TryParse(dictionary["BudgetType"], out RequestBudgetType budgetType);
        _budgetType = budgetType;
        _value = dictionary["Value"].Substring(0, dictionary["Value"].Length - 1);
        _description = dictionary["Description"];
    }

    public override Dictionary<RequestProps, string> GetProperties() => new()
    {
        {RequestProps.BudgetType, _budgetType.ToString()},
        {RequestProps.Value, _value + "₴"},
        {RequestProps.Description, _description},
    };

    public static List<RequestProps> GetRawProperties() => new()
    {
        RequestProps.BudgetType,
        RequestProps.Value,
        RequestProps.Description,
    };

    public override RequestType GetRequestType() => RequestType.Budget;

    public override string ToString()
    {
        return $"-- Запит №{Id} --\n" + 
               $"Бюджет: {EnumLocalisation.Get(_budgetType)} у вигляді " +
               $"{GetProperties()[RequestProps.Value]}\n" +
               $"Ціль: {_description}\n" +
               $"Автор запиту: {GetAuthorProperties()[UserPublicProps.Surname] + " " +
                                GetAuthorProperties()[UserPublicProps.Name]}";
    }
}