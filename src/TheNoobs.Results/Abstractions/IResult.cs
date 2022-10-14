using TheNoobs.Results.Internals;

namespace TheNoobs.Results.Abstractions;

public interface IResult
{
    bool IsFail();

    bool IsSuccess();

    Switch Switch();

    Switch<T> Switch<T>();

    bool TryGetResult<T>(out T? result);
}

public interface IResult<out T> : IResult
{
    T GetResult();
}
