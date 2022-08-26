// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NullEmptySingleMultiCountTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>()
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { 1 },
            new object[] { 3 }
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
