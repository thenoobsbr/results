using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TheNoobs.Results.Abstractions;
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
    public void Given_ANullValidation_When_TriesToAssignToResultVar_Then_AnExceptionShouldBeThrown()
    {
        MyValidation? validation = null;
        Result<string>? result = null;
        var action = () => result = validation!;
        action.Should().Throw<ArgumentNullException>();
        result.Should().BeNull();
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnMyError(MyError error)
        {
            executed = true;
            error.Message.Should().Be("This is an error example.");
            error.Ex.Should().BeOfType<TimeoutException>();
        }

        void DoOnDefault(IResult fail)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }
    }

    [Fact]
    public async Task Given_AnErrorResponse_When_DoASwitchAsyncWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetError();
        var executed = false;
        await response
            .SwitchAsync()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        Task DoOnSuccess(Success<string> result)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        Task DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        async Task DoOnDefault(IResult def)
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
        var response = WhateverServiceStub.GetError();
        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(0);

        Task<int> DoOnSuccess(Success<string> _)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        Task<int> DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        async Task<int> DoOnDefault(IResult def)
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
    public void Given_AnErrorResponse_When_DoASwitchWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetError();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(DoOnSuccess)
            .Case<MyValidation>(DoOnMyValidation)
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnDefault(IResult def)
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(0);

        int DoOnSuccess(Success<string> _)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        int DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        int DoOnDefault(IResult def)
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        void DoOnSuccess(Success<string> result)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnMyValidation(MyValidation validation)
        {
            executed = true;
            validation.Message.Should().Be("The field must be filled.");
        }

        void DoOnDefault(IResult fail)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }
    }

    [Fact]
    public async Task Given_AnSuccessResponse_When_DoASwitchAsyncWithoutTheExpectedErrorInCase_Then_TheDefaultShouldBeCalled()
    {
        var response = WhateverServiceStub.GetSuccess();
        var executed = false;
        await response
            .SwitchAsync()
            .Case<MyValidation>(DoOnMyValidation)
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        Task DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        async Task DoOnDefault(IResult def)
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
        var response = WhateverServiceStub.GetSuccess(value);
        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<MyValidation>(DoOnMyValidation)
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        Task<int> DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        async Task<int> DoOnDefault(IResult def)
        {
            await Task.Delay(10);

            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<int>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be(value);

            return r! * 2;
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        void DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        void DoOnDefault(IResult def)
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        int DoOnMyValidation(MyValidation validation)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }

        int DoOnDefault(IResult def)
        {
            executed = true;
            def.IsSuccess().Should().BeTrue();
            def.IsFail().Should().BeFalse();
            var isValueObtained = def.TryGetResult<int>(out var r);
            isValueObtained.Should().BeTrue();
            r.Should().Be(value);

            return r! * 2;
        }
    }

    [Fact]
    public void Given_ASucceedResponse_When_DoASwitch_Then_ItShouldWorks()
    {
        var response = WhateverServiceStub.GetSuccess();
        var executed = false;
        response
            .Switch()
            .Case<Success<string>>(success => DoOnSuccess(success))
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        void DoOnSuccess(string result)
        {
            executed = true;
            result.Should().Be("This is my success message.");
        }

        void DoOnDefault(IResult fail)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }
    }

    [Theory]
    [CustomAutoData]
    public async Task Given_ASucceedResponse_When_DoASwitchAsync_Then_TheResultMustBeAsExpected(int value)
    {
        var response = WhateverServiceStub.GetSuccess(value);

        var executed = false;
        await response
            .SwitchAsync()
            .Case<Success<int>>(success => DoOnSuccess(success))
            .Default(DoOnDefault);

        executed.Should().BeTrue();

        async Task DoOnSuccess(int r)
        {
            await Task.Delay(20);

            executed = true;
            r.Should().Be(value);
        }

        async Task DoOnDefault(IResult fail)
        {
            await Task.Delay(20);
            throw new NotImplementedException("This method cannot be called in this context.");
        }
    }

    [Theory]
    [CustomAutoData]
    public async Task Given_ASucceedResponse_When_DoASwitchAsyncWithReturn_Then_TheResultMustBeAsExpected(int value)
    {
        var response = WhateverServiceStub.GetSuccess(value);

        var executed = false;
        var result = await response
            .SwitchAsync<int>()
            .Case<Success<int>>(success => DoOnSuccess(success))
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        async Task<int> DoOnSuccess(int r)
        {
            await Task.Delay(20);

            executed = true;
            r.Should().Be(value);
            return r * 2;
        }

        async Task<int> DoOnDefault(IResult fail)
        {
            await Task.Delay(20);
            throw new NotImplementedException("This method cannot be called in this context.");
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
            .Default(DoOnDefault);

        executed.Should().BeTrue();
        result.Should().Be(value * 2);

        int DoOnSuccess(int r)
        {
            executed = true;
            r.Should().Be(value);
            return r * 2;
        }

        int DoOnDefault(IResult fail)
        {
            throw new NotImplementedException("This method cannot be called in this context.");
        }
    }
}
