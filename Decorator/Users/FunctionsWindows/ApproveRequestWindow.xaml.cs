using System;
using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Enums;
using APPZ.Windows;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class ApproveRequestWindow
{
    private readonly Request _request;
    private IUser _user;
    
    public ApproveRequestWindow(IUser user)
    {
        _request = PoolPending.GetInstance().PeekRequest();
        _user = user;
        
        InitializeComponent();

        LblHeader.Content = $"Інформація про запит №{_request.Id}";

        foreach (var pair in _request.GetProperties())
        {
            if (pair.Key == RequestProps.PersonInfo)
            {
                foreach (var personInfo in StringCoding.DecodeToDictionary(pair.Value))
                {
                    CreateLLabel(EnumLocalisation.Get(personInfo.Key) + ":");
                    CreateRLabel(personInfo.Value);
                }
                
                continue;
            }

            CreateLLabel(EnumLocalisation.Get(pair.Key) + ":");

            switch (pair.Key)
            {
                case RequestProps.DesiredPosts:
                    var list = StringCoding.DecodeToList(pair.Value);
                    for (var i = 0; i < list.Count; i++)
                    {
                        CreateRLabel(list[i]);
                        if (i < list.Count - 2)
                            CreateLLabel("");
                    }
                    continue;
                case RequestProps.IsAccountNeeded:
                    Enum.TryParse(pair.Value, out YesNo yesNoValue);
                    CreateRLabel(EnumLocalisation.Get(yesNoValue));
                    continue;
                case RequestProps.BudgetType:
                    Enum.TryParse(pair.Value, out RequestBudgetType budgetTypeValue);
                    CreateRLabel(EnumLocalisation.Get(budgetTypeValue));
                    continue;
                default:
                    CreateRLabel(pair.Value);
                    break;
            }
        }
        
        CreateLLabel("Автор запиту:");
        CreateRLabel($"{_request.GetAuthorProperties()[UserPublicProps.Surname] + " " +
                              _request.GetAuthorProperties()[UserPublicProps.Name]}");

        if (_request.Comment != "")
            CreateCommentButton(BtnComment_OnClick);
    }
    
    private void CreateCommentButton(RoutedEventHandler commentClick)
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

        Button btn = CreateButton("BtnComment", "Попер. коментар", commentClick);
        
        Grid.SetRow(btn, 2);
        ParentGrid.Children.Add(btn);
    }

    private void CreateLLabel(string content)
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
    
    private void CreateRLabel(string content)
    {
        Label infoLabel = new Label
        {
            Content = content,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 18,
            FontWeight = FontWeights.Light,
        };
        
        Grid.SetRow(infoLabel, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoLabel, 2);
        GridInfo.Children.Add(infoLabel);
    }

    private void BtnAccept_OnClick(object sender, RoutedEventArgs e)
    {
        PoolPending.GetInstance().SetRequestStatus(true);
        BtnExit_OnClick(sender, e);
    }
    
    private void BtnComment_OnClick(object sender, RoutedEventArgs e) =>
        MessageBox.Show(_request.Comment, 
            "Коментар щодо помилок",
            MessageBoxButton.OK, 
            MessageBoxImage.Hand);

    private void BtnRedo_OnClick(object sender, RoutedEventArgs e)
    {
        InputWindow window = new InputWindow(_user);
        window.ShowDialog();

        if (window.Status)
            BtnExit_OnClick(sender, e);
    }
    
    private void BtnReject_OnClick(object sender, RoutedEventArgs e)
    {
        PoolPending.GetInstance().SetRequestStatus(false);
        BtnExit_OnClick(sender, e);
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e) => Close();
}