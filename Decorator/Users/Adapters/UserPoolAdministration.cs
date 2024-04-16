using System;
using APPZ.Databases;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Adapters;

public class UserPoolAdministration: UserPoolAccess
{
    public UserPoolAdministration(IUser user) => _user = user;

    public override void Notify(int count)
    {
        if (!_user.GetEditableButtons().ContainsKey(UserFunctions.AddUser)) return;
        _user.GetEditableButtons()[UserFunctions.AddUser]
                .Content = EnumLocalisation.Get(UserFunctions.AddUser) +
                           (count == 0 ? "" : $". Залишилось запитів: {count}");
    }

    public override IUser GetUser() => _user;

    public override bool Equals(UserPoolAccess obj) => _user.Equals(obj?.GetUser());
}