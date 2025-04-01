namespace SuperTestBase;

using Xunit;

public class AssertExt
{
    public static void ValueEquals<T>(T expected, T actual) where T : class
    {
        Assert.True(EqualityHelper.ValueEquals(expected, actual));
    }
}
