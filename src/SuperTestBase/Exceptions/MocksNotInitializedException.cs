namespace SuperTestBase;

using System;

public class MocksNotInitializedException : Exception
{
    public Type Type { get; set; }

    public MocksNotInitializedException(Type type)
        : base(string.Format("Mocks are not set to be initialized for the test class: {0}", type?.Name.ToString() ?? "Unknown"))
    {
        Type = type;
    }
}