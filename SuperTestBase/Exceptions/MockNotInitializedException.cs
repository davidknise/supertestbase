// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using System;

    public class MockNotInitializedException : Exception
    {
        public Type Type { get; set; }

        public MockNotInitializedException(Type type)
            : base(string.Format("Mock was not initialized: {0}", type?.Name.ToString() ?? "Unknown"))
        {
            Type = type;
        }
    }
}