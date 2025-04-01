namespace SuperTestBase;

using System;

public class MockTestException : Exception
{
    public MockTestException() : base()
    {
    }

    public MockTestException(string message) : base(message)
    {
    }
}