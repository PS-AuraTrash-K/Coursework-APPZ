using System.Windows;
using System.Windows.Controls;
using APPZ.Decorator.Requests;
using APPZ.Enums;

namespace APPZ.Decorator.Users.Functions;

public class ChangeRequest: FuncDecorator
{
    private static byte _count;
    
    public ChangeRequest(IUser obj) : base(obj) => _count++;
    
    public override void CreateButton(Grid grid)
    {
        AddEditableButton(ButtonHandler(UserFunctions.ChangeRequest, grid, false));
        
        Wrapper.CreateButton(grid);
    }

    protected override Window GetWindow()
        => RequestWindowManager.Get(
            PoolRedo.GetInstance().PeekRequest(Wrapper.GetId())
            );

    public override int GetCount() => _count;
    
    public override void Reset()
    {
        _count = 0;
        base.Reset();
    }
}
