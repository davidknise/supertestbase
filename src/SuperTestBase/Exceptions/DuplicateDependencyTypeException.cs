namespace SuperTestBase;

using System;

public class DuplicateDependencyTypeException : Exception
{
    public Type Type { get; set; }

    public DuplicateDependencyTypeException(Type type)
        : base(string.Format("TestBase<T> does not support constructors with identically typed dependencies. Identical type: {0}", type?.Name ?? "Unknown"))
    {
        Type = type;
    }
}
