using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results;

public abstract class Fail : Result, IFail
{
    protected Fail(string message)
        : base(null)
    {
        Message = message;
    }

    public string Message { get; }
}
