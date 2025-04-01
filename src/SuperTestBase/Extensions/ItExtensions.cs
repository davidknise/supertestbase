namespace SuperTestBase;

using Moq;
using System;

public static class ItExt
{
    public static T IsValue<T>(T expected) where T : class
    {
        return It.Is<T>(x => EqualityHelper.ValueEquals(x, expected));
    }
}
