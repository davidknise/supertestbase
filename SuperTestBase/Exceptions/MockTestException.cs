// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace TestBase
{
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
}
