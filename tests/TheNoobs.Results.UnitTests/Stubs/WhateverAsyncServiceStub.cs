namespace TheNoobs.Results.UnitTests.Stubs;

public static class WhateverAsyncServiceStub
{
    public static async Task<Result<string>> GetErrorAsync()
    {
        var ex = new TimeoutException("This is an error example.");
        return await Task.FromResult(new MyError(ex.Message, ex));
    }

    public static async ValueTask<Result<string>> GetSpecificErrorAsync()
    {
        var ex = new TimeoutException("This is a specific error example.");
        return await Task.FromResult(new MySpecificError(ex.Message, ex));
    }

    public static async Task<Result<string>> GetSuccessAsync()
    {
        return await Task.FromResult("This is my success message.");
    }

    public static async Task<Result<int>> GetSuccessAsync(int value)
    {
        return await Task.FromResult(value);
    }
}
