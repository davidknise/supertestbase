namespace SuperTestBase;

using System;
using System.Collections;
using System.Collections.Generic;

public class IsNullOrEmptyTestData : IEnumerable<object[]>
{
    private readonly List<object[]> data = new List<object[]>()
    {
        new object[] { null },
        new object[] { string.Empty }
    };

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
