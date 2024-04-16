using System.Collections.Generic;

namespace APPZ.Decorator.Requests;

public class PoolAdministration: AbstractPool
{
    private readonly Queue<Request> _accountNeeded = new();
    
    private static PoolAdministration _instance;
    private PoolAdministration() {}
    public static PoolAdministration GetInstance() => _instance ??= new PoolAdministration();
    
    public void AddRequest(Request request)
    {
        _accountNeeded.Enqueue(request);
        
        SendUpdate(CurrentId);
    }
    
    public Request PeekRequest() => _accountNeeded.Count == 0 ? null : _accountNeeded.Peek();

    public void RemoveRequest()
    {
        if (_accountNeeded.Count > 0)
            _accountNeeded.Dequeue();
        SendUpdate(CurrentId);
    }
    
    protected override void SendUpdate(int id)
    {
        var listener = GetListenerId(id);
        listener?.Notify(_accountNeeded.Count);
    }
}