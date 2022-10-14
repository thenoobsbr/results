namespace TheNoobs.Results.Abstractions;

public interface IFail : IResult
{
    string Message { get; }
}
