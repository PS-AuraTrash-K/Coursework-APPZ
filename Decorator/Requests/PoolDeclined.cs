using System.Collections.Generic;
using APPZ.Databases;
using APPZ.Decorator.Users.Adapters;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class PoolDeclined: AbstractPool
{
    private readonly Dictionary<int, Queue<Request>> _declinedRequests = new();
    
    private static PoolDeclined _instance;
    private PoolDeclined() {}
    public static PoolDeclined GetInstance() => _instance ??= new PoolDeclined();

    public void AddRequest(Request request)
    {
        request.Status = RequestStatus.Declined;
        
        request.Save();
        
        AddListener(new UserPoolDeclined(request.GetAuthor()));
        
        if (!_declinedRequests.ContainsKey(request.GetAuthorId()))
            _declinedRequests.Add(request.GetAuthorId(), new Queue<Request>());
        
        _declinedRequests[request.GetAuthorId()].Enqueue(request);
    }
    
    protected override void SendUpdate(int id)
    {
        var listener = GetListenerId(id);
        if (!_declinedRequests.ContainsKey(id)) return;
        foreach (var request in _declinedRequests[id])
        {
            listener?.Notify(request.Id);
            FileManager.DeleteByte(request.Id.ToString());
        }

        _declinedRequests.Remove(id);
        RemoveListener(listener);
    }
}