using System.Collections.Generic;
using System.Linq;
using APPZ.Decorator.Users.Adapters;

namespace APPZ.Decorator.Requests;

public abstract class AbstractPool
{
    private readonly List<UserPoolAccess> _listeners = new();
    protected int CurrentId;
    
    public void AddListener(UserPoolAccess listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void RemoveListener(UserPoolAccess listener)
    {
        var targetUser = _listeners.FirstOrDefault(u => u.Equals(listener));
        _listeners.Remove(targetUser);
    }

    public void Update(int id)
    {
        CurrentId = id;
        SendUpdate(id);
    }

    protected abstract void SendUpdate(int id);
    
    protected UserPoolAccess GetListenerId(int id)
        => _listeners.FirstOrDefault(user => user.GetUser().GetId() == id);
}