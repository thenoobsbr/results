namespace TheNoobs.Results.UnitTests.Stubs;

public class MySpecificError : MyError
{
    public MySpecificError(string message, Exception ex) : base(message, ex)
    {
    }
}
