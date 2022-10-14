using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TheNoobs.Results.Internals;
using TheNoobs.Results.UnitTests.Stubs;
using Xunit;

namespace TheNoobs.Results.UnitTests;

[Trait("Category", "UnitTests")]
[Trait("Class", nameof(Switch))]
[ExcludeFromCodeCoverage]
public class SwitchTests
{
    [Fact]
    public void Given_ANullError_When_DoASwitch_Then_AnExceptionMustBeThrown()
    {
        MyError? error = null;
        var actionSwitch = () => new Switch(error!);
        actionSwitch.Should().Throw<ArgumentNullException>();

        var actionSwitchT = () => new Switch<string>(error!);
        actionSwitchT.Should().Throw<ArgumentNullException>();

        var actionSwitchAsync = () => new SwitchAsync(error!);
        actionSwitchAsync.Should().Throw<ArgumentNullException>();

        var actionSwitchAsyncT = () => new SwitchAsync<string>(error!);
        actionSwitchAsyncT.Should().Throw<ArgumentNullException>();
    }
}
