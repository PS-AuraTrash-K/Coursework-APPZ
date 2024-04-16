using System;

using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class PoolManager
{
    private static PoolManager _instance;

    private PoolManager() { }
    
    public static PoolManager GetInstance() => _instance ??= new PoolManager();
    
    public void PreloadUpdate(int id)
    {
        PoolPending.GetInstance().Update(id);
        PoolRedo.GetInstance().Update(id);
        PoolAdministration.GetInstance().Update(id);
    }

    public void LoadUpdate(int id)
    {
        PoolApproved.GetInstance().Update(id);
        PoolDeclined.GetInstance().Update(id);
    }
    
    public void SetRequestPool(Request request)
    {
        switch (request.Status)
        {
            case RequestStatus.Pending:
                PoolPending.GetInstance().AddRequest(request);
                break;
            case RequestStatus.Redo:
                PoolRedo.GetInstance().AddRequest(request);
                break;
            case RequestStatus.ApprovedUnseen:
            case RequestStatus.ApprovedSeen:
                PoolApproved.GetInstance().AddRequest(request);
                break;
            case RequestStatus.Declined:
                PoolDeclined.GetInstance().AddRequest(request);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}