using System.Windows;
using System.Windows.Controls;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class SendPersonnel: FuncDecorator
{
    private static byte _count;
    
    public SendPersonnel(IUser obj) : base(obj) => _count++;

    public override void CreateButton(Grid grid)
    {
        ButtonHandler(UserFunctions.SendPersonnel, grid);
        
        Wrapper.CreateButton(grid);
    }
    
    protected override Window GetWindow() => RequestWindowManager.Get(Wrapper, RequestType.Personnel);
    
    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
