using TheNoobs.Results.Abstractions;
using TheNoobs.Results.Internals;

namespace TheNoobs.Results;

public abstract class Result : IResult
{
    private readonly object? _result;

    protected Result(object? result)
    {
        _result = result;
    }

    public abstract bool IsFail();

    public abstract bool IsSuccess();

    public Switch Switch()
    {
        return new Switch(this);
    }

    public Switch<T> Switch<T>()
    {
        return new Switch<T>(this);
    }

    public SwitchAsync SwitchAsync()
    {
        return new SwitchAsync(this);
    }

    public SwitchAsync<T> SwitchAsync<T>()
    {
        return new SwitchAsync<T>(this);
    }

    public bool TryGetResult<T>(out T? result)
    {
        if (_result is T r)
        {
            result = r;
            return true;
        }

        result = default;
        return false;
    }
}

public abstract class Result<T> : Result, IResult<T>
    where T : notnull
{
    private readonly T _result;

    protected Result(T result)
        : base(result)
    {
        _result = result;
    }

    public static implicit operator Result<T>(T val)
    {
        return new Success<T>(val);
    }

    public static implicit operator Result<T>(Fail fail)
    {
        return new FailWrappedResult(fail);
    }

    public static implicit operator T(Result<T> val)
    {
        return val.GetResult();
    }

    public virtual T GetResult()
    {
        return _result;
    }

    public class FailWrappedResult : Result<T>, IFailWrappedResult
    {
        private readonly IFail _fail;

        public FailWrappedResult(IFail fail) : base(default!)
        {
            _fail = fail ?? throw new ArgumentNullException(nameof(fail));
        }

        public IFail GetFail()
        {
            return _fail;
        }

        public override bool IsFail()
        {
            return _fail.IsFail();
        }

        public override bool IsSuccess()
        {
            return _fail.IsSuccess();
        }
    }
}
