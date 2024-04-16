using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Users.FunctionsWindows;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class AddUser: FuncDecorator
{
    private static byte _count;
    
    public AddUser(IUser obj) : base(obj) => _count++;

    public override void CreateButton(Grid grid)
    {
        AddEditableButton(ButtonHandler(UserFunctions.AddUser, grid));
        
        Wrapper.CreateButton(grid);
    }

    protected override Window GetWindow() => new AddUserWindow();

    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
