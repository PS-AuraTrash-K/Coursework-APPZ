using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Enums;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class AddUserWindow
{
    private int _count;
    private Request _request;
    private Button _btnMinus;
    private Button _btnAdd;
    private readonly List<ComboBoxItem> _hiddenComboBox = new();
  
    public AddUserWindow()
    {
        _request = PoolAdministration.GetInstance().PeekRequest();
        
        InitializeComponent();
        
        LblHeader.Content = "Додати користувача";

        if (_request == null)
            DefaultCreate();
        else
            SetCreate();
    }

    private void DefaultCreate()
    {
        foreach (var prop in Enum.GetValues(typeof(UserPublicProps)).OfType<UserPublicProps>())
        {
            CreateLeftLabel(EnumLocalisation.Get(prop) + ":");
            CreateRightTextBox(prop.ToString());
        }
                
        CreateLeftLabel("Пости:");
        CreateRightComboBox(Enum.GetValues(typeof(UserPosts)).OfType<UserPosts>());
        CreateManageButton();
        CreateResultButton();
    }
    
    private void SetCreate()
    {
        BtnPosts.Visibility = Visibility.Visible;
        
        foreach (var prop in
                 StringCoding.DecodeToDictionary(_request.GetProperties()[RequestProps.PersonInfo]))
        {
            CreateLeftLabel(EnumLocalisation.Get(prop.Key) + ":");
            CreateRightTextBox(prop.Key.ToString(), prop.Value);
        }
                
        CreateLeftLabel("Пости:");
        CreateRightComboBox(Enum.GetValues(typeof(UserPosts)).OfType<UserPosts>());
        CreateManageButton();
        CreateResultButton();
    }

    private void CreateManageButton()
    {
        _btnMinus = new Button
        {
            Name = "MinusPost",
            Content = "-",
            Height = 24,
            Width = 24,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5,5,5,5),
            IsEnabled = false
        };

        _btnMinus.Click += (_, _) =>
        {
            var lastComboBox = GridInfo.Children.OfType<ComboBox>().Last();
            
            GridInfo.Children.RemoveAt(GridInfo.Children.Count - 1);
            GridInfo.Children.RemoveAt(GridInfo.Children.Count - 1);
            GridInfo.RowDefinitions.RemoveAt(GridInfo.RowDefinitions.Count - 1);
            _count--;

            if (_hiddenComboBox.Count > 0 && lastComboBox.SelectedItem != null)
            {
                OnItemSelected(_hiddenComboBox.Last(), Visibility.Visible);
                _hiddenComboBox.Remove(_hiddenComboBox.Last());
            }

            _btnMinus.IsEnabled = _count > 1;
            _btnAdd.IsEnabled = _count < Enum.GetValues(typeof(UserPosts)).Length;
        };
        
        _btnAdd = new Button
        {
            Name = "AddPost",
            Content = "+",
            Height = 24,
            Width = 24,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5,5,5,5),
        };

        _btnAdd.Click += (_, _) =>
        {
            CreateLeftLabel("");
            CreateRightComboBox(Enum.GetValues(typeof(UserPosts)).OfType<UserPosts>());

            foreach (var temp in _hiddenComboBox)
            {
                OnItemSelected(temp, Visibility.Collapsed);
            }

            _btnMinus.IsEnabled = _count > 1;
            _btnAdd.IsEnabled = _count < Enum.GetValues(typeof(UserPosts)).Length;
        };

        Grid.SetRow(_btnAdd, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(_btnAdd, 3);
        GridInfo.Children.Add(_btnAdd);


        Grid.SetRow(_btnMinus, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(_btnMinus, 3);
        GridInfo.Children.Add(_btnMinus);
    }

    private void CreateResultButton()
    {
        Button btn = new Button
        {
            Name="BtnResult",
            Content = "Додати",
            Height = 24,
            Width = 120,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        
        btn.Content = "Додати";
        btn.Click += BtnResult_OnClick;
        
        Grid.SetRow(btn, ParentGrid.RowDefinitions.Count - 2);
        ParentGrid.Children.Add(btn);
    }

    private void CreateLeftLabel(string content)
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
            FontSize = 18,
            FontWeight = FontWeights.DemiBold,
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
            Width = 130
        };
        
        Grid.SetRow(infoTextBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoTextBox, 2);
        GridInfo.Children.Add(infoTextBox);
    }
    
    private void CreateRightComboBox(IEnumerable<UserPosts> posts)
    {
        ComboBox comboBox = new ComboBox
        {
            Name = $"ComboBox{_count++}",
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Width = 130
        };

        foreach (var post in posts)
        {
            ComboBoxItem item = new ComboBoxItem
            {
                Name = post.ToString(),
                Content = EnumLocalisation.Get(post),
            };
            
            comboBox.Items.Add(item);
            
            item.Selected += (_, _) =>
            {
                OnItemSelected(item, Visibility.Collapsed);
                _hiddenComboBox.Add(item);
            };
            
            item.Unselected += (_, _) =>
            {
                OnItemSelected(item, Visibility.Visible);
                _hiddenComboBox.Remove(item);
            };
        }
        
        Grid.SetRow(comboBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(comboBox, 2);
        GridInfo.Children.Add(comboBox);
    }
    
    private void OnItemSelected(ComboBoxItem target, Visibility visibility)
    {
        foreach (var comboBox in GridInfo.Children.OfType<ComboBox>())
        {
            if (comboBox == target.Parent) continue;

            Enum.TryParse(target.Name, out UserPosts index);
            ((ComboBoxItem)comboBox.Items.GetItemAt((int)index)).Visibility = visibility;
        }
    }

    private void BtnResult_OnClick(object sender, RoutedEventArgs e)
    {
        var dictionary = new Dictionary<UserPublicProps, string>();
        var queue = new Queue<UserPosts>();
        
        foreach (var textBox in GridInfo.Children.OfType<TextBox>())
        {
            if (!Enum.TryParse(textBox.Name, out UserPublicProps props)) continue;

            if (textBox.Text == "")
            {
                MessageBox.Show(
                    "Вибачте, у вас наявне таке порожнє поле, як:\n" +
                    $"{EnumLocalisation.Get(props)}\n" +
                    "Спробуйте заповнити його!",
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            }

            dictionary.Add(props,textBox.Text);
        }
        
        foreach (var comboBox in GridInfo.Children.OfType<ComboBox>())
        {
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Вибачте, у вас наявне таке порожнє поле посади.\n" +
                    "Спробуйте заповнити його!", 
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            }

            if (!Enum.TryParse(((ComboBoxItem)comboBox.SelectedItem).Name, out UserPosts post)) continue;
            
            queue.Enqueue(post);
        }
        
        if (_request != null)
        {
            PoolAdministration.GetInstance().RemoveRequest();
        }
        
        Database.GetInstance().SetUser(dictionary, queue);
        MessageBox.Show(
            $"Логін: {User.Count}\n" + $"Пароль: {Database.DefaultPassword}",
            "Інформація про нового користувача",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
        BtnExit_OnClick(sender, e);
    }

    private void BtnPosts_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            $"Пости: {_request.GetProperties()[RequestProps.DesiredPosts]}",
            "Інформація про нового користувача",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}