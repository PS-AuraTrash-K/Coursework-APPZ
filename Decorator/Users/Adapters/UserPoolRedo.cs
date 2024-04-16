using System;
using System.Windows;
using APPZ.Databases;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Adapters;
public class UserPoolRedo: UserPoolAccess
{
    public UserPoolRedo(IUser user) => _user = user;

    public override void Notify(int count)
    {
        if (!_user.GetEditableButtons().ContainsKey(UserFunctions.ChangeRequest)) return;
        _user.GetEditableButtons()[UserFunctions.ChangeRequest].Visibility = count > 0
            ? Visibility.Visible
            : Visibility.Collapsed;
        _user.GetEditableButtons()[UserFunctions.ChangeRequest]
                .Content = EnumLocalisation.Get(UserFunctions.ChangeRequest) +
                           (count == 0 ? "" : $". Залишилось запитів: {count}");
    }

    public override IUser GetUser() => _user;

    public override bool Equals(UserPoolAccess obj) => _user.Equals(obj?.GetUser());
}