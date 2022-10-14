namespace TheNoobs.Results.UnitTests.Stubs;

public static class WhateverServiceStub
{
    public static Result<string> GetError()
    {
        var ex = new TimeoutException("This is an error example.");
        return new MyError(ex.Message, ex);
    }

    public static Result<string> GetSpecificError()
    {
        var ex = new TimeoutException("This is a specific error example.");
        return new MySpecificError(ex.Message, ex);
    }

    public static Result<string> GetSuccess()
    {
        return "This is my success message.";
    }

    public static Result<string> GetValidation()
    {
        return new MyValidation("The field must be filled.");
    }

    public static Result<int> GetSuccess(int value)
    {
        return value;
    }
}
