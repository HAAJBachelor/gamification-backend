namespace gamification_backend;

public class MockHttpSession : ISession
{
    private readonly Dictionary<string, object> sessionStorage = new();

    public object this[string name]
    {
        get => sessionStorage[name];
        set => sessionStorage[name] = value;
    }

    void ISession.Set(string key, byte[] value)
    {
        sessionStorage[key] = value;
    }

    bool ISession.TryGetValue(string key, out byte[] value)
    {
        if (sessionStorage[key] != null)
        {
            value = (byte[]) sessionStorage[key];
            return true;
        }

        value = null;
        return false;
    }

    // Not nessisary for mocking

    IEnumerable<string> ISession.Keys => throw new NotImplementedException();

    string ISession.Id => throw new NotImplementedException();

    bool ISession.IsAvailable => throw new NotImplementedException();

    void ISession.Clear()
    {
        throw new NotImplementedException();
    }

    void ISession.Remove(string key)
    {
        throw new NotImplementedException();
    }

    Task ISession.CommitAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task ISession.LoadAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}