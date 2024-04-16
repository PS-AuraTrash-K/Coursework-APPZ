using System.Windows;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users.FunctionsWindows;
using APPZ.Enums;

namespace APPZ.Decorator.Users;

public static class RequestWindowManager
{
    public static Window Get(IUser user, RequestType type) => type switch
    {
        RequestType.Budget => new SendBudgetWindow(user),
        RequestType.Personnel => new SendPersonnelWindow(user),
        _ => null
    };

    public static Window Get(Request request) => request.GetRequestType() switch
    {
        RequestType.Budget => new SendBudgetWindow(request),
        RequestType.Personnel => new SendPersonnelWindow(request),
        _ => null
    };
}