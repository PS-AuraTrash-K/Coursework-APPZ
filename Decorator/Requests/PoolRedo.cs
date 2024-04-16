using System.Collections.Generic;
using APPZ.Databases;
using APPZ.Decorator.Users.Adapters;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class PoolRedo: AbstractPool
{
    private readonly Dictionary<int, Queue<Request>> _awaitRequests = new();
    
    private static PoolRedo _instance;
    private PoolRedo() {}
    public static PoolRedo GetInstance() => _instance ??= new PoolRedo();
    
    public void AddRequest(Request request)
    {
        request.Status = RequestStatus.Redo;
        
        request.Save();
        
        var authorId = request.GetAuthorId();
        
        if (!_awaitRequests.ContainsKey(request.GetAuthorId()))
            _awaitRequests.Add(authorId, new Queue<Request>());

        AddListener(new UserPoolRedo(request.GetAuthor()));
        
        _awaitRequests[authorId].Enqueue(request);
        
        SendUpdate(CurrentId);
    }
    
    public Request PeekRequest(int id)
        => _awaitRequests[id].Count == 0 ? null : _awaitRequests[id].Peek();

    public void RemoveRequest(int id)
    {
        if (_awaitRequests[id].Count == 0) return;

        var temp = _awaitRequests[id].Dequeue();
        
        FileManager.DeleteByte(temp.Id.ToString());

        if (_awaitRequests[id].Count == 0)
            SendRemoveUpdate(id);
        else
            SendUpdate(id);
    }

    protected override void SendUpdate(int id)
    {
        var listener = GetListenerId(id);
        listener?.Notify(_awaitRequests[id].Count);
    }
    
    private void SendRemoveUpdate(int id)
    {
        SendUpdate(id);
        RemoveListener(GetListenerId(id));
    }
}