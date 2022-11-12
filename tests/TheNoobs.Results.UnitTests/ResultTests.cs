using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TheNoobs.Results.Abstractions;
using TheNoobs.Results.Extensions;
using TheNoobs.Results.UnitTests.CustomData;
using TheNoobs.Results.UnitTests.Stubs;
using Xunit;

namespace TheNoobs.Results.UnitTests;

[Trait("Category", "UnitTests")]
[Trait("Class", nameof(Result))]
[ExcludeFromCodeCoverage]
public class ResultTests
{
    [Fact]
    public async Task Given_AnErrorResponse_When_DoAResultSwitchAsyncWithInheritance_Then_OnlyTheFirstShouldBeExecuted()
    {
        var response = await WhateverAsyncServiceStub.GetSpecificErrorAsync();
        var executed = false;
        var x = await response
            .SwitchAsync<int>()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MySpecificError>(DoOnMySpecificError)
            .Case<MyError>(DoOnMyError)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        x.Should().Be(default);

        Task<int> DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        Task<int> DoOnMyError(MyError error)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task<int> DoOnMySpecificError(MySpecificError error)
        {
            await Task.Delay(1);
            executed = true;
            error.Message.Should().Be("This is a specific error example.");
            return default;
        }

        Task<int> DoOnElse(IResult def)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public void Given_AnErrorResponse_When_DoAResultSwitchWithInheritance_Then_OnlyTheFirstShouldBeExecuted()
    {
        var response = WhateverServiceStub.GetSpecificError();
        var executed = false;
        var x = response
            .Switch<int>()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MySpecificError>(DoOnMySpecificError)
            .Case<MyError>(DoOnMyError)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        x.Should().Be(default);

        int DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        int DoOnMyError(MyError error)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        int DoOnMySpecificError(MySpecificError error)
        {
            executed = true;
            error.Message.Should().Be("This is a specific error example.");
            return default;
        }

        int DoOnElse(IResult def)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public void Given_AnErrorResponse_When_DoASwitch_Then_ItShouldWorks()
    {
        var response = WhateverServiceStub.GetError();
        response.IsFail().Should().BeTrue();
        response.IsSuccess().Should().BeFalse();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Case<MyError>(DoOnMyError)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMyError(MyError error)
        {
            executed = true;
            error.Message.Should().Be("This is an error example.");
            error.Ex.Should().BeOfType<TimeoutException>();
        }

        void DoOnElse(IResult fail)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public async Task Given_AnErrorResponse_When_DoASwitchAsyncWithInheritance_Then_OnlyTheFirstShouldBeExecuted()
    {
        var executed = false;
        await WhateverAsyncServiceStub.GetSpecificErrorAsync()
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MySpecificError>(DoOnMySpecificError)
            .Case<MyError>(DoOnMyError)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        Task DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        Task DoOnMyError(MyError error)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task DoOnMySpecificError(MySpecificError error)
        {
            await Task.Delay(1);
            executed = true;
            error.Message.Should().Be("This is a specific error example.");
        }

        Task DoOnElse(IResult def)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public async Task
        Given_AnErrorResponse_When_DoASwitchAsyncWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var executed = false;
        await WhateverAsyncServiceStub.GetErrorAsync()
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        Task DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        Task DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task DoOnElse(IResult def)
        {
            await Task.Delay(10);
            executed = true;
            def.IsSuccess().Should().BeFalse();
            def.IsFail().Should().BeTrue();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeFalse();
            r.Should().Be(default);
        }
    }

    [Fact]
    public async Task
        Given_AnErrorResponse_When_DoASwitchAsyncWithResultWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = await WhateverAsyncServiceStub.GetErrorAsync();
        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(0);

        Task<int> DoOnSuccess(Success<string> _)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        Task<int> DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task<int> DoOnElse(IResult def)
        {
            await Task.Delay(10);

            executed = true;
            def.IsSuccess().Should().BeFalse();
            def.IsFail().Should().BeTrue();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeFalse();
            r.Should().Be(default);

            return 0;
        }
    }

    [Fact]
    public void Given_AnErrorResponse_When_DoASwitchWithInheritance_Then_OnlyTheFirstShouldBeExecuted()
    {
        var response = WhateverServiceStub.GetSpecificError();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MySpecificError>(DoOnMySpecificError)
            .Case<MyError>(DoOnMyError)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMyError(MyError error)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMySpecificError(MySpecificError error)
        {
            executed = true;
            error.Message.Should().Be("This is a specific error example.");
        }

        void DoOnElse(IResult def)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public void Given_AnErrorResponse_When_DoASwitchWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetError();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnElse(IResult def)
        {
            executed = true;
            def.IsSuccess().Should().BeFalse();
            def.IsFail().Should().BeTrue();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeFalse();
            r.Should().Be(default);
        }
    }

    [Fact]
    public void
        Given_AnErrorResponse_When_DoASwitchWithResultWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetError();
        var executed = false;
        var result = response
            .Switch<int>()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(0);

        int DoOnSuccess(Success<string> _)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        int DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        int DoOnElse(IResult def)
        {
            executed = true;
            def.IsSuccess().Should().BeFalse();
            def.IsFail().Should().BeTrue();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeFalse();
            r.Should().Be(default);

            return 0;
        }
    }

    [Fact]
    public void Given_ANonSucceedResponse_When_DoASwitch_Then_ItShouldWorks()
    {
        var response = WhateverServiceStub.GetValidation();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            executed = true;
            validation.Message.Should().Be("The field must be filled.");
        }

        void DoOnElse(IResult fail)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Fact]
    public async Task
        Given_AnSuccessResponse_When_DoASwitchAsyncWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = await WhateverAsyncServiceStub.GetSuccessAsync();
        var executed = false;
        await response
            .SwitchAsync()
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        Task DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task DoOnElse(IResult def)
        {
            await Task.Delay(10);
            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be("This is my success message.");
        }
    }

    [Theory]
    [CustomAutoData]
    public async Task
        Given_AnSuccessResponse_When_DoASwitchAsyncWithResponseWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled(
            int value)
    {
        var response = await WhateverAsyncServiceStub.GetSuccessAsync(value);
        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        Task<int> DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        async Task<int> DoOnElse(IResult def)
        {
            await Task.Delay(10);

            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<int>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be(value);

            return r * 2;
        }
    }

    [Fact]
    public void Given_AnSuccessResponse_When_DoASwitchWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetSuccess();
        var executed = false;
        response
            .Switch()
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        void DoOnElse(IResult def)
        {
            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<string>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be("This is my success message.");
        }
    }

    [Theory]
    [CustomAutoData]
    public void
        Given_AnSuccessResponse_When_DoASwitchWithResponseWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled(
            int value)
    {
        var response = WhateverServiceStub.GetSuccess(value);
        var executed = false;
        var result = response
            .Switch<int>()
            .Case<MyValidation>(DoOnMyValidation)
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        int DoOnMyValidation(MyValidation validation)
        {
            throw new Exception("This method cannot be called in this context.");
        }

        int DoOnElse(IResult def)
        {
            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<int>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be(value);

            return r * 2;
        }
    }

    [Fact]
    public void Given_ANullValidation_When_TriesToAssignToResultVar_Then_AnExceptionShouldBeThrown()
    {
        MyValidation? validation = null;
        Result<string>? result = null;
        var action = () => result = validation!;
        action.Should().Throw<ArgumentNullException>();
        result.Should().BeNull();
    }

    [Fact]
    public void Given_ASucceedResponse_When_DoASwitch_Then_ItShouldWorks()
    {
        var response = WhateverServiceStub.GetSuccess();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(success => DoOnSuccess(success))
            .Else(DoOnElse);

        executed.Should().BeTrue();

        void DoOnSuccess(string result)
        {
            executed = true;
            result.Should().Be("This is my success message.");
        }

        void DoOnElse(IResult fail)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Theory]
    [CustomAutoData]
    public async Task Given_ASucceedResponse_When_DoASwitchAsync_Then_TheResultMustBeAsExpected(int value)
    {
        var response = await WhateverAsyncServiceStub.GetSuccessAsync(value);

        var executed = false;
        await response
            .SwitchAsync()
            .Case<Success<int>>(success => DoOnSuccess(success))
            .Else(DoOnElse);

        executed.Should().BeTrue();

        async Task DoOnSuccess(int r)
        {
            await Task.Delay(20);

            executed = true;
            r.Should().Be(value);
        }

        async Task DoOnElse(IResult fail)
        {
            await Task.Delay(20);
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Theory]
    [CustomAutoData]
    public async Task Given_ASucceedResponse_When_DoASwitchAsyncWithReturn_Then_TheResultMustBeAsExpected(int value)
    {
        var response = await WhateverAsyncServiceStub.GetSuccessAsync(value);

        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<Success<int>>(success => DoOnSuccess(success))
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        async Task<int> DoOnSuccess(int r)
        {
            await Task.Delay(20);

            executed = true;
            r.Should().Be(value);
            return r * 2;
        }

        async Task<int> DoOnElse(IResult fail)
        {
            await Task.Delay(20);
            throw new Exception("This method cannot be called in this context.");
        }
    }

    [Theory]
    [CustomAutoData]
    public void Given_ASucceedResponse_When_DoASwitchWithReturn_Then_TheResultMustBeAsExpected(int value)
    {
        var response = WhateverServiceStub.GetSuccess(value);

        var executed = false;
        var result = response
            .Switch<int>()
            .Case<Success<int>>(success => DoOnSuccess(success))
            .Else(DoOnElse);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        int DoOnSuccess(int r)
        {
            executed = true;
            r.Should().Be(value);
            return r * 2;
        }

        int DoOnElse(IResult fail)
        {
            throw new Exception("This method cannot be called in this context.");
        }
    }
}
