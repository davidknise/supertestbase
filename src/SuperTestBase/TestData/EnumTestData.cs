namespace SuperTestBase;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enumerates over all values in an enumeration.
/// </summary>
/// <remarks>
/// Often times you just want to prove that the code uses your input Enumeration value.
/// To prove this, you just need to input two values, not every Enumeration value.
/// To save test time for this scenario, use <see cref="QuickEnumTestData{T}"/>.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class EnumTestData<T> : IEnumerable<object[]>
{
    private List<object[]> data = null;

    public IEnumerator<object[]> GetEnumerator()
    {
        if (data == null)
        {
            data = new List<object[]>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                data.Add(new object[] { value });
            }
        }

        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
