using System;
using System.Windows;

namespace APPZ.Decorator.Users.Adapters;

public class UserPoolDeclined: UserPoolAccess
{
    public UserPoolDeclined(IUser user) => _user = user;

    public override void Notify(int count)
    {
        MessageBox.Show(
            $"Ваш запит під №{count} був розглянутий та відхилений.", 
            "Відмова!",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }

    public override IUser GetUser() => _user;

    public override bool Equals(UserPoolAccess obj) => _user.Equals(obj?.GetUser());
}