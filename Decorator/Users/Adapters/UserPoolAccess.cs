namespace APPZ.Decorator.Users.Adapters;

public abstract class UserPoolAccess
{
    protected IUser _user;
    
    public abstract void Notify(int count);

    public abstract IUser GetUser();

    public abstract bool Equals(UserPoolAccess obj);
}