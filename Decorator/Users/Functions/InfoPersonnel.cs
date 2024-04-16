using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Users.FunctionsWindows;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class InfoPersonnel: FuncDecorator
{
    private static byte _count;
    
    public InfoPersonnel(IUser obj) : base(obj) => _count++;

    public override void CreateButton(Grid grid)
    {
        ButtonHandler(UserFunctions.InfoPersonnel, grid);
        
        Wrapper.CreateButton(grid);
    }

    protected override Window GetWindow() => new InfoRequestWindow(RequestType.Personnel);
    
    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
