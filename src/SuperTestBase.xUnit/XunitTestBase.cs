namespace SuperTestBase;

using System;
using System.Reflection;
using Xunit;

public partial class TestBase<T>
    where T : class
{
    /// <summary>
    /// A shared method that uses reflection to test constructor injected dependencies for
    /// <see cref="DependencyNullException"/> error handling.
    /// </summary>
    [Fact]
    public virtual void Constructor()
    {
        VerifyConstructorTest = VerifyConstructorTestxUnit;
        ConstructorTest();
    }

    private void VerifyConstructorTestxUnit(
        Action testAction,
        Type exceptionType)
    {
        TargetInvocationException reflectedActual = Assert.Throws<TargetInvocationException>(
            () => testAction());

        Assert.Equal(exceptionType, reflectedActual.InnerException?.GetType());
    }
}
