using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Enums;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class SendPersonnelWindow
{
    private readonly IUser _user;
    private readonly Request _request;

    public SendPersonnelWindow(IUser user)
    {
        _user = user;
        _request = null;

        InitializeComponent();

        LblHeader.Content = "Надіслати запит на персонал";
        LblSecondHeader.Content = EnumLocalisation.Get(RequestProps.PersonInfo);
        
        foreach (var prop in Enum.GetValues(typeof(UserPublicProps)).OfType<UserPublicProps>())
        {
            CreateLeftLabel(EnumLocalisation.Get(prop) + ":");
            CreateRightTextBox(prop.ToString());
        }

        foreach (var prop in RequestPersonnel.GetRawProperties()
                     .Where(prop => prop != RequestProps.PersonInfo))
        {
            CreateLeftLabel(EnumLocalisation.Get(prop) + ":");

            if (prop == RequestProps.IsAccountNeeded)
            {
                CreateRightCheckBox(prop.ToString());
                continue;
            }
            
            CreateRightTextBox(prop.ToString());
        }
    }
    
    public SendPersonnelWindow(Request request)
    {
        _user = request.GetAuthor();
        _request = request;
        
        InitializeComponent();
        
        LblHeader.Content = "Надіслати запит на персонал";
        LblSecondHeader.Content = EnumLocalisation.Get(RequestProps.PersonInfo);

        foreach (var prop in
                 StringCoding.DecodeToDictionary(request.GetProperties()[RequestProps.PersonInfo]))
        {
            CreateLeftLabel(EnumLocalisation.Get(prop.Key) + ":");
            CreateRightTextBox(prop.Key.ToString(), prop.Value);
        }

        foreach (var prop in request.GetProperties()
                     .Where(prop => prop.Key != RequestProps.PersonInfo))
        {
            CreateLeftLabel(EnumLocalisation.Get(prop.Key) + ":");

            switch (prop.Key)
            {
                case RequestProps.IsAccountNeeded:
                    Enum.TryParse(prop.Value, out YesNo value);
                    CreateRightCheckBox(prop.Key.ToString(), 
                        value == YesNo.Yes);
                    continue;
                default:
                    CreateRightTextBox(prop.Key.ToString(), prop.Value);
                    break;
            }
        }
        
        CreateRedoButton(BtnComment_OnClick, BtnRemove_OnClick);
    }

    private void CreateRedoButton(RoutedEventHandler commentClick, RoutedEventHandler removeClick)
    {
        Button CreateButton(string name, string caption, RoutedEventHandler func)
        {
            var btn = new Button
            {
                Name = name,
                Content = caption,
                Height = 24,
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            btn.Click += func;
            return btn;
        }

        Button btn = CreateButton("BtnComment", "Коментар", commentClick);
        
        Button remove = CreateButton("BtnRemove", "У кошик", removeClick);
        
        Grid.SetRow(btn, ParentGrid.RowDefinitions.Count - 4);
        Grid.SetRow(remove, ParentGrid.RowDefinitions.Count - 2);
        ParentGrid.Children.Add(remove);
        ParentGrid.Children.Add(btn);
    }
    
    private void CreateLeftLabel(string content, bool header = false)
    {
        GridInfo.RowDefinitions.Insert(GridInfo.RowDefinitions.Count, new RowDefinition
        {
            Height = GridLength.Auto
        });
        
        Label label = new Label
        {
            Content = content,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = header ? 16 : 14,
        };

        Grid.SetRow(label, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(label, 1);
        GridInfo.Children.Add(label);
    }
    
    private void CreateRightTextBox(string name, string text = "")
    {
        TextBox infoTextBox = new TextBox
        {
            Name = name,
            Text = text,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Width = 130,
            Margin = new Thickness(0,0,5,0)
        };
        
        Grid.SetRow(infoTextBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoTextBox, 2);
        GridInfo.Children.Add(infoTextBox);
    }
    
    private void CreateRightCheckBox(string name, bool isChecked = false)
    {
        CheckBox infoTextBox = new CheckBox
        {
            Name = name,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            IsChecked = isChecked
        };
        
        Grid.SetRow(infoTextBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoTextBox, 2);
        GridInfo.Children.Add(infoTextBox);
    }
    
    private void BtnAddRequest_OnClick(object sender, RoutedEventArgs e)
    {
        var dictionary = new Dictionary<RequestProps, string>();
        var personInfo = new Dictionary<UserPublicProps, string>();
        
        foreach (var textBox in GridInfo.Children.OfType<TextBox>())
        {
            if (Enum.TryParse(textBox.Name, out UserPublicProps userProp))
            {
                if (textBox.Text == "")
                {
                    MessageBox.Show(
                        "Вибачте, у вас наявне таке порожнє поле, як:\n" +
                        $"\"{EnumLocalisation.Get(userProp)}\"\n" +
                        "Спробуйте заповнити його!",
                        "Не вірні данні",
                        MessageBoxButton.OK,
                        MessageBoxImage.Hand
                    );
                    return;
                }

                personInfo.Add(userProp, textBox.Text);
                continue;
            }
            
            if (!Enum.TryParse(textBox.Name, out RequestProps prop)) continue;
            
            if (textBox.Text == "")
            {
                MessageBox.Show(
                    "Вибачте, у вас наявне таке порожнє поле, як:\n" +
                    $"\"{EnumLocalisation.Get(prop)}\"\n" +
                    "Спробуйте заповнити його!",
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            }

            dictionary.Add(prop, textBox.Text);
        }
        
        foreach (var textBox in GridInfo.Children.OfType<CheckBox>())
        {
            if (!Enum.TryParse(textBox.Name, out RequestProps prop)) continue;

            dictionary.Add(prop, textBox.IsChecked.ToString());
        }

        Boolean.TryParse(dictionary[RequestProps.IsAccountNeeded], out bool isAccountNeeded);
        
        RequestPersonnel request = new RequestPersonnel(
            personInfo,
            StringCoding.DecodeToList(dictionary[RequestProps.DesiredPosts]),
            isAccountNeeded,
            _user
            );
        
        if (_request != null)
        {
            request.Comment = _request.Comment;
            PoolRedo.GetInstance().RemoveRequest(_request.GetAuthorId());
        }
        
        PoolPending.GetInstance().AddRequest(request);
        
        MessageBox.Show(
            "Інформація щодо поданого вами запиту:\n" +
            request,
            "Інформація про новий запит",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
        BtnExit_OnClick(sender, e);
    }
    
    private void BtnComment_OnClick(object sender, RoutedEventArgs e) =>
        MessageBox.Show(_request.Comment, 
            "Коментар щодо помилок",
            MessageBoxButton.OK, 
            MessageBoxImage.Hand);

    private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
    {
        PoolRedo.GetInstance().RemoveRequest(_request.GetAuthorId());
        Close();
    }
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e) => Close();
}