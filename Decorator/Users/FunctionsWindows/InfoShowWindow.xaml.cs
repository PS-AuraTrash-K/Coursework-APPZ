using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;

namespace APPZ.Decorator.Users.FunctionsWindows;

public partial class InfoShowWindow
{
    public InfoShowWindow(IUser user)
    {
        InitializeComponent();
        
        LblHeader.Content = "Інформація";

        foreach (var pair in user.GetProperties())
        {
            CreateLeftLabel(EnumLocalisation.Get(pair.Key) + ":");
            CreateRightLabel(pair.Value);
        }


        var posts = "Пости:";
        foreach (var post in user.GetPosts())
        {
            CreateLeftLabel(posts);
            CreateRightLabel(EnumLocalisation.Get(post));
            posts = "";
        }
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
    
    private void CreateRightLabel(string content)
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
    
    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}