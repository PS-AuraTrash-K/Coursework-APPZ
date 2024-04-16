using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Users.FunctionsWindows;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class InfoBudget: FuncDecorator
{
    private static byte _count;
    
    public InfoBudget(IUser obj) : base(obj) => _count++;
    
    public override void CreateButton(Grid grid)
    {
        ButtonHandler(UserFunctions.InfoBudget, grid);
        
        Wrapper.CreateButton(grid);
    }

    protected override Window GetWindow() => new InfoRequestWindow(RequestType.Budget);
    
    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
