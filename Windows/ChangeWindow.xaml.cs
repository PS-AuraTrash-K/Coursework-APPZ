using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Enums;

namespace APPZ.Windows;

public partial class ChangeWindow
{
    private UserPrivateProps _prop;
    private Control _oldBox;
    private Control _newBox;
    
    public ChangeWindow(UserPrivateProps prop)
    {
        _prop = prop;
        
        InitializeComponent();
        
        LblMain.Content = EnumLocalisation.Get(_prop) + ":";
        
        switch (_prop)
        {
            case UserPrivateProps.Login:
                _oldBox = new TextBox
                {
                    Name = "TxtBxOld",
                    Height = 25, Width = 160,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                
                _newBox = new TextBox
                {
                    Name = "TxtBxNew",
                    Height = 25, Width = 160,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                break;
            
            case UserPrivateProps.Password:
                _oldBox = new PasswordBox
                {
                    Name = "TxtBxOld",
                    Height = 25, Width = 160,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                
                _newBox = new PasswordBox
                {
                    Name = "TxtBxNew",
                    Height = 25, Width = 160,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                break;
        }

        Grid.SetColumn(_oldBox, 1);
        Grid.SetColumn(_newBox, 1);
        OldGrid.Children.Add(_oldBox);
        NewGrid.Children.Add(_newBox);
    }

    private void BtnAccept_OnClick(object sender, RoutedEventArgs e)
    {
        var database = Database.GetInstance();
        
        switch (_prop)
        {
            case UserPrivateProps.Login when !database.CheckLogin(((TextBox)_oldBox).Text):
            case UserPrivateProps.Password when !database.CheckPassword(((PasswordBox)_oldBox).Password):
                MessageBox.Show(
                    "Вибачте, ваш старий логін/пароль " +
                    "не збігається зі старим значенням у базі даних.\nСпробуйте ще раз.", 
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            case UserPrivateProps.Login when ((TextBox)_oldBox).Text == ((TextBox)_newBox).Text:
            case UserPrivateProps.Password when ((PasswordBox)_oldBox).Password == ((PasswordBox)_newBox).Password:
                MessageBox.Show(
                    "Вибачте, Ви не внесли ніякі зміни у данні.\nСпробуйте ще раз.", 
                    "Ніяких змін",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            case UserPrivateProps.Login when !database.CheckFreeLogin(((TextBox)_newBox).Text):
                MessageBox.Show(
                    "Вибачте, ваш новий логін " +
                    "вже занятий.\nСпробуйте інший.", 
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            case UserPrivateProps.Login:
                database.SetPrivateProperty(_prop, ((TextBox)_newBox).Text);
                BtnExit_OnClick(sender, e);
                break;
            case UserPrivateProps.Password:
                database.SetPrivateProperty(_prop, ((PasswordBox)_newBox).Password);
                BtnExit_OnClick(sender, e);
                break;
        }
    }
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}