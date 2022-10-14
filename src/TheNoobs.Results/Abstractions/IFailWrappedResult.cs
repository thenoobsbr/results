namespace TheNoobs.Results.Abstractions;

internal interface IFailWrappedResult : IResult
{
    IFail GetFail();
}
