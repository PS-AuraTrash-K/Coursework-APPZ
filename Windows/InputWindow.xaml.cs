using System;
using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users;

namespace APPZ.Windows;

public partial class InputWindow
{
    private TextBox _textBox;
    public bool Status;
    private IUser _user;
    
    public InputWindow(IUser user)
    {
        _user = user;
        
        InitializeComponent();
        
        LblMain.Content = "Коментар до запиту";
        
        _textBox = new TextBox
        {
            Name = "TextBox",
            Height = 25, Width = 160,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        Grid.SetColumn(_textBox, 1);
        OldGrid.Children.Add(_textBox);
    }

    private void BtnAccept_OnClick(object sender, RoutedEventArgs e)
    {
        if (_textBox.Text == "")
        {
            MessageBox.Show(
                "Вибачте, ваш коментар порожній.\nСпробуйте ще раз.",
                "Не вірні данні",
                MessageBoxButton.OK,
                MessageBoxImage.Hand
            );
            Status = false;
            return;
        }

        Status = true;
        PoolPending.GetInstance().SetRequestStatus(false, _textBox.Text, _user);
        
        BtnExit_OnClick(sender, e);
    }
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}