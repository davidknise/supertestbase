namespace SuperTestBase;

using System;
using System.Reflection;
using NUnit;
using NUnit.Framework;

public partial class TestBase<T>
    where T : class
{
    /// <summary>
    /// A shared method that uses reflection to test constructor injected dependencies for
    /// <see cref="DependencyNullException"/> error handling.
    /// </summary>
    [TestCase]
    public virtual void Constructor()
    {
        VerifyConstructorTest = VerifyConstructorTestNUnit;
        ConstructorTest();
    }

    private void VerifyConstructorTestNUnit(
        Action testAction,
        Type exceptionType)
    {
        TargetInvocationException reflectedActual = Assert.Throws<TargetInvocationException>(
            () => testAction());

        Assert.AreEqual(exceptionType, reflectedActual.InnerException?.GetType());
    }
}
