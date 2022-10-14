using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results;

public sealed class Success<TResult> : Result<TResult>, ISuccess
    where TResult : notnull
{
    public Success(TResult result)
        : base(result)
    {
    }
}
