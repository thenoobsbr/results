namespace TheNoobs.Results.UnitTests.Stubs;

public class MyError : Fail
{
    public MyError(string message, Exception ex) : base(message)
    {
        Ex = ex;
    }

    public Exception Ex { get; }
}
