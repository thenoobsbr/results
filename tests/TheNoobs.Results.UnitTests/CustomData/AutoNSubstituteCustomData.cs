using System.Diagnostics.CodeAnalysis;
using AutoFixture.Xunit2;
using Xunit;

namespace TheNoobs.Results.UnitTests.CustomData;

[ExcludeFromCodeCoverage]
public class CustomAutoDataAttribute : AutoDataAttribute
{
}

[ExcludeFromCodeCoverage]
public class InlineAutoNSubstituteDataAttribute : CompositeDataAttribute
{
    public InlineAutoNSubstituteDataAttribute(params object[] values)
        : base(new InlineDataAttribute(values), new CustomAutoDataAttribute())
    {
    }
}
