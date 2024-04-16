using System;
using APPZ.Databases;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Adapters;

public class UserPoolPending: UserPoolAccess
{
    public UserPoolPending(IUser user) => _user = user;

    public override void Notify(int count)
    {
        if (!_user.GetEditableButtons().ContainsKey(UserFunctions.ApproveRequest)) return;
        _user.GetEditableButtons()[UserFunctions.ApproveRequest].IsEnabled = count > 0;
        _user.GetEditableButtons()[UserFunctions.ApproveRequest]
                .Content = EnumLocalisation.Get(UserFunctions.ApproveRequest) +
                           (count == 0 ? "" : $". Залишилось запитів: {count}");
    }

    public override IUser GetUser() => _user;

    public override bool Equals(UserPoolAccess obj) => _user.Equals(obj?.GetUser());
}