using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Users.FunctionsWindows;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class InfoShow: FuncDecorator
{
    private static byte _count;
    
    public InfoShow(IUser obj) : base(obj) => _count++;
    
    public override void CreateButton(Grid grid)
    {
        ButtonHandler(UserFunctions.InfoShow, grid);
        
        Wrapper.CreateButton(grid);
    }

    protected override Window GetWindow() => new InfoShowWindow(Wrapper);
    
    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
