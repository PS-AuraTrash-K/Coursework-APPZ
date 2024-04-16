using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Enums;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class SendBudgetWindow
{
    private readonly IUser _user;
    private readonly Request _request;

    public SendBudgetWindow(IUser user)
    {
        _user = user;
        _request = null;
        
        InitializeComponent();
        
        LblHeader.Content = "Надіслати запит на бюджет";

        foreach (var prop in RequestBudget.GetRawProperties())
        {
            CreateLeftLabel(EnumLocalisation.Get(prop) + ":");

            switch (prop)
            {
                case RequestProps.Description:
                    CreateRightTextBox(prop.ToString(), true);
                    continue;
                case RequestProps.BudgetType:
                    CreateRightComboBox(
                        Enum.GetValues(typeof(RequestBudgetType)).OfType<RequestBudgetType>(),
                        RequestBudgetType.Income);
                    continue;
                default:
                    CreateRightTextBox(prop.ToString());
                    break;
            }
        }
    }
    
    public SendBudgetWindow(Request request)
    {
        _user = request.GetAuthor();
        _request = request;
        
        InitializeComponent();
        
        LblHeader.Content = "Надіслати запит на бюджет";

        foreach (var prop in request.GetProperties())
        {
            CreateLeftLabel(EnumLocalisation.Get(prop.Key) + ":");

            switch (prop.Key)
            {
                case RequestProps.Description:
                    CreateRightTextBox(prop.Key.ToString(), true, prop.Value);
                    continue;
                case RequestProps.BudgetType:
                    Enum.TryParse(prop.Value, out RequestBudgetType value);
                    CreateRightComboBox(
                        Enum.GetValues(typeof(RequestBudgetType)).OfType<RequestBudgetType>(),
                        value);
                    continue;
                default:
                    CreateRightTextBox(prop.Key.ToString(), false,
                        prop.Value.Substring(0, prop.Value.Length - 1));
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
            FontSize = 14,
        };
        
        Grid.SetRow(label, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(label, 1);
        GridInfo.Children.Add(label);
    }

    private void CreateRightTextBox(string name, bool isRichTextBox = false, string text = "")
    {
        TextBox infoTextBox = new TextBox
        {
            Name = name,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Width = 130,
            Height = isRichTextBox ? 80 : 22,
            Margin = new Thickness(0,0,5,0),
            Text = text,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            TextWrapping = isRichTextBox ? TextWrapping.Wrap : TextWrapping.NoWrap 
        };
        
        Grid.SetRow(infoTextBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoTextBox, 2);
        GridInfo.Children.Add(infoTextBox);
    }

    private void CreateRightComboBox(IEnumerable<RequestBudgetType> posts, RequestBudgetType selected)
    {
        ComboBox comboBox = new ComboBox
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Width = 130,
            Margin = new Thickness(0,0,5,0)
        };

        foreach (var post in posts)
        {
            ComboBoxItem item = new ComboBoxItem
            {
                Name = post.ToString(),
                Content = EnumLocalisation.Get(post),
            };
            
            comboBox.Items.Add(item);

            if (post == selected)
                comboBox.SelectedItem = item;
        }
        
        Grid.SetRow(comboBox, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(comboBox, 2);
        GridInfo.Children.Add(comboBox);
    }

    private void BtnAddRequest_OnClick(object sender, RoutedEventArgs e)
    {
        var dictionary = new Dictionary<RequestProps, string>();
        
        string temp = "";
        
        foreach (var comboBox in GridInfo.Children.OfType<ComboBox>())
        {
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Вибачте, у вас наявне таке порожнє поле типу доходу.\n" +
                    "Спробуйте заповнити його!", 
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            }
            
            if (!Enum.TryParse(((ComboBoxItem)comboBox.SelectedItem).Name, out RequestBudgetType type)) continue;

            temp = ((ComboBoxItem)comboBox.SelectedItem).Name;

            dictionary.Add(RequestProps.BudgetType, type.ToString());
        }
        
        foreach (var textBox in GridInfo.Children.OfType<TextBox>())
        {
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
        
        foreach (var richTextBox in GridInfo.Children.OfType<RichTextBox>())
        {
            if (!Enum.TryParse(richTextBox.Name, out RequestProps prop)) continue;
            
            var text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

            if (text == "\r\n")
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

            dictionary.Add(prop, text.Substring(0, text.Length - 2));
        }

        Enum.TryParse(temp, out RequestBudgetType budgetType);
        
        RequestBudget request = new RequestBudget(
            dictionary[RequestProps.Value],
            dictionary[RequestProps.Description], budgetType, _user);

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