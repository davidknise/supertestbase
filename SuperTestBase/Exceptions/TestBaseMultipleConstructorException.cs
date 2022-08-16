// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using System;

    public class TestBaseMultipleConstructorException : Exception
    {
        public Type Type { get; set; }

        public TestBaseMultipleConstructorException(Type type)
            : base(string.Format("Type under BaseTest cannot have more than one constructor: {0}", type?.Name.ToString() ?? "Unknown"))
        {
            Type = type;
        }
    }
}