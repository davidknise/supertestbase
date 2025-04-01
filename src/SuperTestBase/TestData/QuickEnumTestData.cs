namespace SuperTestBase;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Like <see cref="EnumTestData{T}"/> but only uses the first two values of the enumeration.
/// </summary>
/// <remarks>
/// Often times you just want to prove that the code uses your input Enumeration value.
/// To prove this, you just need to input two values, not every Enumeration value.
/// </remarks>
/// <typeparam name="T">The enumeration type.</typeparam>
public class QuickEnumTestData<T> : IEnumerable<object[]>
{
    private List<object[]> data = null;

    public IEnumerator<object[]> GetEnumerator()
    {
        if (data == null)
        {
            data = new List<object[]>();

            Array values = Enum.GetValues(typeof(T));
            for (int i = 0; i < 2; i++)
            {
                data.Add(new object[] { values.GetValue(i) });
            }
        }

        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
