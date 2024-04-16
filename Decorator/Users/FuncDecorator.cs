using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using APPZ.Databases;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users.Adapters;
using APPZ.Enums;

namespace APPZ.Decorator.Users;

public abstract class FuncDecorator: IUser
{
    private const float BtnHeight = 24;
    private const float BtnMinWidth = 160;

    private static bool _isCreated;
    private static Dictionary<UserFunctions, Button> _editableButtons = new();
    
    protected readonly IUser Wrapper;
    
    protected FuncDecorator(IUser obj)
    {
        Wrapper = obj;
    }

    public abstract int GetCount();
    
    public virtual void Reset()
    {
        _isCreated = false;
        _editableButtons.Clear();
        PoolPending.GetInstance().RemoveListener(new UserPoolPending(Wrapper));
        Wrapper.Reset();
    }

    public List<UserPosts> GetPosts() => Wrapper.GetPosts();

    public Dictionary<UserFunctions, Button> GetEditableButtons() => _editableButtons;

    protected void AddEditableButton(Button dependant)
    {
        Enum.TryParse(dependant.Name, out UserFunctions func);
        _editableButtons.Add(func, dependant);
    }

    public int GetId() => Wrapper.GetId();

    public abstract void CreateButton(Grid grid);

    public Dictionary<UserPublicProps, string> GetProperties() => Wrapper.GetProperties();

    protected abstract Window GetWindow();

    protected Button ButtonHandler(UserFunctions func, Grid grid, bool isVisible = true)
    {
        if (!_isCreated)
        {
            foreach (var _ in Enum.GetValues(typeof(UserFunctions)))
                grid.RowDefinitions.Insert(grid.RowDefinitions.Count, new RowDefinition
                {
                    Height = GridLength.Auto
                });

            _isCreated = true;
        }
        
        Button btn = new Button
        {
            Name = func.ToString(),
            Content = EnumLocalisation.Get(func),
            Height = BtnHeight,
            MinWidth = BtnMinWidth,
            Margin = new Thickness(0,5,0,5),
            Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed
        };
        
        btn.Click += (_, _) => GetWindow().ShowDialog();
        
        Grid.SetColumn(btn, 1);
        grid.Children.Add(btn);
        
        Enum.TryParse(btn.Name, out UserFunctions index);
        Grid.SetRow(btn, (int)index);

        return btn;
    }

    public bool Equals(IUser obj)
    {
        return Wrapper.Equals(obj);
    }
}