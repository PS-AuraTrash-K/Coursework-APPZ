using System.Windows;
using APPZ.Enums;
using APPZ.Windows;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class SettingsWindow
{
    public SettingsWindow()
    {
        InitializeComponent();
    }
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnLoginChange_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ChangeWindow(UserPrivateProps.Login);
        window.ShowDialog();
    }

    private void BtnPasswordChange_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ChangeWindow(UserPrivateProps.Password);
        window.ShowDialog();
    }
}