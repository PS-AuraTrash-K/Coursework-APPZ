using System;
using System.Windows;

namespace APPZ.Decorator.Users.Adapters;

public class UserPoolApproved: UserPoolAccess
{
    public UserPoolApproved(IUser user) => _user = user;

    public override void Notify(int count)
    {
        MessageBox.Show(
            $"Ваш запит під №{count} був успішно розглянутий та прийнятий. Наші вітання!", 
            "Успіх!",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    public override IUser GetUser() => _user;

    public override bool Equals(UserPoolAccess obj) => _user.Equals(obj?.GetUser());
}