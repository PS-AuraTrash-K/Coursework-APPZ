using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Enums;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class InfoRequestWindow
{
    private List<Request> _list;
    private int _page;
    private int _inPage = 3;
    private readonly int _maxPage;
    
    public InfoRequestWindow(RequestType type)
    {
        _list = PoolApproved.GetInstance().GetRequestsList(type);

        InitializeComponent();
        
        LblHeader.Content = $"Інформація про {EnumLocalisation.Get(type)}:";

        if (_list.Count == 0)
        {
            CreateLeftLabel("");
            CreateRightLabel("Порожньо...");
            CheckBtnState();
            return;
        }
        
        _maxPage = (int)Math.Ceiling(_list.Count / (_inPage * 1.0));
        _page = _maxPage;

        GetPage();
    }

    private void GetPage()
    {
        CheckBtnState();
        
        GridInfo.Children.Clear();
        GridInfo.RowDefinitions.Clear();

        int inPage;
        if (_page == _maxPage
            && _list.Count % _inPage != 0)
            inPage = _list.Count % _inPage;
        else
            inPage = _inPage;
        
        foreach (var request in _list.GetRange(_inPage * (_page - 1), inPage))
        {
            CreateLeftLabel("");
            CreateRightLabel("---");
            foreach (var pair in request.GetProperties()
                         .Where(p => p.Key != RequestProps.IsAccountNeeded))
            {
                if (pair.Key == RequestProps.PersonInfo)
                {
                    foreach (var infoPair in StringCoding.DecodeToDictionary(pair.Value))
                    {
                        CreateLeftLabel(EnumLocalisation.Get(infoPair.Key) + ":");
                        CreateRightLabel(infoPair.Value);
                    }
                    continue;
                }
                
                if (pair.Key == RequestProps.BudgetType)
                {
                    CreateLeftLabel(EnumLocalisation.Get(pair.Key) + ":");
                    Enum.TryParse(pair.Value, out RequestBudgetType value);
                    CreateRightLabel(EnumLocalisation.Get(value));
                    continue;
                }
                CreateLeftLabel(EnumLocalisation.Get(pair.Key) + ":");
                CreateRightLabel(pair.Value);
            }
            
            CreateLeftLabel("Автор запиту:");
            CreateRightLabel(request.GetAuthorProperties()[UserPublicProps.Surname]
                             + " " + request.GetAuthorProperties()[UserPublicProps.Name]);
        }
        CreateLeftLabel("");
        CreateRightLabel("---");

        LblPageCount.Content = _page;
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
            FontSize = 13,
            FontWeight = FontWeights.DemiBold,
        };
        
        Grid.SetRow(label, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(label, 1);
        GridInfo.Children.Add(label);
    }
    
    private void CreateRightLabel(string content)
    {
        Label infoLabel = new Label
        {
            Content = content,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 13,
            FontWeight = FontWeights.Light,
        };
        
        Grid.SetRow(infoLabel, GridInfo.RowDefinitions.Count - 1);
        Grid.SetColumn(infoLabel, 2);
        GridInfo.Children.Add(infoLabel);
    }
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnPrevious_OnClick(object sender, RoutedEventArgs e)
    {
        _page--;
        GetPage();
    }

    private void BtnNext_OnClick(object sender, RoutedEventArgs e)
    {
        _page++;
        GetPage();
    }

    private void CheckBtnState()
    {
        if (_maxPage < 2)
        {
            BtnNext.IsEnabled = false;
            BtnPrevious.IsEnabled = false;
        }
        else if (_page == 1)
        {
            BtnNext.IsEnabled = true;
            BtnPrevious.IsEnabled = false;
        }
        else if (_page == _maxPage)
        {
            BtnNext.IsEnabled = false;
            BtnPrevious.IsEnabled = true;
        }
        else
        {
            BtnNext.IsEnabled = true;
            BtnPrevious.IsEnabled = true;
        }
    }
}