// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace TestBase
{
    using System;

    public class MockTypeNotSupportedException : Exception
    {
        public Type Type { get; set; }

        public MockTypeNotSupportedException(Type type)
            : base(string.Format("Mock type is not supported. Add support for it to TestBase: {0}", type?.Name ?? "Unknown"))
        {
            Type = type;
        }
    }
}
