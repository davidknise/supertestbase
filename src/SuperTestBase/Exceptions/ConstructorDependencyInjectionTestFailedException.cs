// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using System;

    public class ConstructorDependencyInjectionTestFailedException : Exception
    {
        public Type Expected { get; set; }

        public Type Actual { get; set; }

        public ConstructorDependencyInjectionTestFailedException(Type expected, Type actual)
        {
            Expected = expected;
            Actual = actual;
        }   
    }
}
