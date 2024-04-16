using System.Collections.Generic;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class PoolPending: AbstractPool
{
    private readonly Queue<Request> _pendingRequests = new();
    
    private static PoolPending _instance;
    private PoolPending() { }
    public static PoolPending GetInstance() => _instance ??= new PoolPending();

    public void AddRequest(Request request)
    {
        request.Status = RequestStatus.Pending;
        
        request.Save();
        
        _pendingRequests.Enqueue(request);
        SendUpdate(CurrentId);
    }
    
    public Request PeekRequest() => _pendingRequests.Count == 0 ? null : _pendingRequests.Peek();

    public void SetRequestStatus(bool status, string comment = "", IUser checkUser = null)
    {
        if (status) // Approve
            PoolApproved.GetInstance().AddRequest(RemoveRequest());
        else if (checkUser == null) // Demolish
            PoolDeclined.GetInstance().AddRequest(RemoveRequest());
        else // Redo
        {
            var request = RemoveRequest();
            request.Comment = comment + $"\n\nПеревірив/ла {checkUser.GetProperties()[UserPublicProps.Surname]} " +
                  $"{checkUser.GetProperties()[UserPublicProps.Name]}.";
            PoolRedo.GetInstance().AddRequest(request);
        }
        
        SendUpdate(CurrentId);
    }

    private Request RemoveRequest()
        => _pendingRequests.Count == 0 ? null : _pendingRequests.Dequeue();
    
    protected override void SendUpdate(int id)
    {
        var listener = GetListenerId(id);
        listener?.Notify(_pendingRequests.Count);
    }
}