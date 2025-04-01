namespace SuperTestBase;

using System;
using System.Reflection;

public static class EqualityHelper
{
    public static bool ValueEquals<T>(T obj1, T obj2)
    {
        Type reflectedType = typeof(T);
        PropertyInfo[] reflectedProperties = reflectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        bool isValid = obj2 != null && obj2.GetType() == reflectedType;

        if (isValid)
        {
            foreach (PropertyInfo property in reflectedProperties)
            {
                // Ignore static properties
                if (property.GetAccessors(true)[0].IsStatic)
                {
                    continue;
                }

                object thisProperty = property.GetValue(obj1);
                object obj2Property = property.GetValue(obj2);

                if (property.DeclaringType == typeof(string))
                {
                    isValid = string.Equals((string)thisProperty, (string)obj2Property, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    if (thisProperty == null)
                    {
                        isValid = obj2Property == null;
                    }
                    else
                    {
                        isValid = thisProperty.Equals(obj2Property);
                    }
                }

                if (!isValid)
                {
                    break;
                }
            }
        }

        return isValid;
    }
}