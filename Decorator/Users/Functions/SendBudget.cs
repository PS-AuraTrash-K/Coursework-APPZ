using System.Windows;
using System.Windows.Controls;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class SendBudget: FuncDecorator
{
    private static byte _count;
    
    public SendBudget(IUser obj) : base(obj) => _count++;
    
    public override void CreateButton(Grid grid)
    {
        ButtonHandler(UserFunctions.SendBudget, grid);
        
        Wrapper.CreateButton(grid);
    }
    
    protected override Window GetWindow() => RequestWindowManager.Get(Wrapper, RequestType.Budget);
    
    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
