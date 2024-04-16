using System;
using System.Collections.Generic;
using System.Linq;
using APPZ.Decorator.Users.Adapters;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class PoolApproved: AbstractPool
{
    private readonly Dictionary<int, Queue<Request>> _tempApprovedRequests = new();
    private readonly Queue<Request> _approvedRequests = new();
    
    private static PoolApproved _instance;
    private PoolApproved() {}
    public static PoolApproved GetInstance() => _instance ??= new PoolApproved();

    public void AddRequest(Request request)
    {
        if (request.Status != RequestStatus.ApprovedSeen)
        {
            if (!_tempApprovedRequests.ContainsKey(request.GetAuthorId()))
                _tempApprovedRequests.Add(request.GetAuthorId(), new Queue<Request>());

            _tempApprovedRequests[request.GetAuthorId()].Enqueue(request);
        }

        AddListener(new UserPoolApproved(request.GetAuthor()));

        if (request.GetProperties().ContainsKey(RequestProps.IsAccountNeeded))
        {
            Enum.TryParse(request.GetProperties()[RequestProps.IsAccountNeeded], out YesNo value);
            if (value == YesNo.Yes)
                PoolAdministration.GetInstance().AddRequest(request);
        }
        
        if (request.Status != RequestStatus.ApprovedSeen)
        {
            request.Status = RequestStatus.ApprovedUnseen;
            request.Save();
        }
        
        _approvedRequests.Enqueue(request);
    }

    protected override void SendUpdate(int id)
    {
        var listener = GetListenerId(id);
        if (!_tempApprovedRequests.ContainsKey(id)) return;
        foreach (var request in _tempApprovedRequests[id])
        {
            listener?.Notify(request.Id);
            
            request.Status = RequestStatus.ApprovedSeen;
            request.Save();
        }

        _tempApprovedRequests.Remove(id);
    }

    public List<Request> GetRequestsList(RequestType requestType)
        => _approvedRequests?.Where(req => req.GetRequestType() == requestType).ToList();
}