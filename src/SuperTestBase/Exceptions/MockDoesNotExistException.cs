namespace SuperTestBase;

using System;

public class MockDoesNotExistException : Exception
{
    public Type Type { get; set; }

    public MockDoesNotExistException(Type type)
        : base(string.Format("Mock does not exist: {0}", type?.Name.ToString() ?? "Unknown"))
    {
        Type = type;
    }
}
