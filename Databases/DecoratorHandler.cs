using System;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users;
using APPZ.Decorator.Users.Adapters;
using APPZ.Decorator.Users.Functions;
using APPZ.Enums;

namespace APPZ.Databases;

public static class DecoratorHandler {
    
    public static IUser Decorate(IUser user)
    {
        IUser result = user;
        
        DecorateHandler(obj => new InfoShow(obj), ref result);
        DecorateHandler(obj => new Settings(obj), ref result);
        
        foreach (var post in user.GetPosts())
        {
            switch (post)
            {
                case UserPosts.Accountant:
                    DecorateHandler(obj => new InfoBudget(obj), ref result);
                    DecorateHandler(obj => new SendBudget(obj), ref result);
                    DecorateHandler(obj => new ChangeRequest(obj), ref result);
                    break;
                case UserPosts.Administrator:
                    DecorateHandler(obj => new InfoPersonnel(obj), ref result);
                    DecorateHandler(obj => new AddUser(obj), ref result);
                    
                    PoolAdministration.GetInstance().AddListener(new UserPoolAdministration(result));
                    break;
                case UserPosts.Fop:
                    DecorateHandler(obj => new ApproveRequest(obj), ref result);
                    DecorateHandler(obj => new InfoPersonnel(obj), ref result);
                    DecorateHandler(obj => new InfoBudget(obj), ref result);
                    
                    PoolPending.GetInstance().AddListener(new UserPoolPending(result));
                    break;
                case UserPosts.HumanRes:
                    DecorateHandler(obj => new InfoPersonnel(obj), ref result);
                    DecorateHandler(obj => new SendPersonnel(obj), ref result);
                    DecorateHandler(obj => new ChangeRequest(obj), ref result);
                    break;
                default:
                    return null;
            }
        }
        
        return result;
    }

    private static void DecorateHandler(Func<IUser, IUser> func, ref IUser result)
    {
        var copy = func(result);
        
        if (copy.GetCount() > 1) return;
        
        result = copy;
    }
}

