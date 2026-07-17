using Mercury.Abstractions.Logging;

namespace Mercury.Demo.WinForms.v1.Logging;

public class DemoLogger : IMercuryLogger
{
    public void Trace(string message)
    {
        throw new NotImplementedException();
    }

    public void Info(string message)
    {
        throw new NotImplementedException();
    }

    public void Warn(string message)
    {
        throw new NotImplementedException();
    }

    public void Error(string message, Exception? exception = null)
    {
        throw new NotImplementedException();
    }
}