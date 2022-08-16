// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using Moq;
    using Moq.Language.Flow;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Place generic mock extensions that should apply to all mocks here.
    /// </summary>
    public static class MockExtensions
    {
        /// <summary>
        /// Sets up a string return method mock value for a method.
        /// </summary>
        /// <param name="expression">The string-returning function to mock out.</param>
        /// <param name="includeType">Prefixes with the declaring type name if true. Defaults to true.</param>
        /// <returns>String of format: (Type.)?MethodName().mock</returns>
        public static IReturnsResult<T> SetupStringReturn<T>(
            this Mock<T> mock,
            Expression<Func<T, string>> expression,
            bool includeType = true)
            where T : class
        {
            MethodInfo methodInfo = ((MethodCallExpression)expression.Body).Method;
            string declaringTypeName = includeType ? $"{methodInfo.DeclaringType.Name}." : string.Empty;
            return mock
                .Setup(expression)
                .Returns($"{declaringTypeName}{methodInfo.Name}().mock");
        }
    }
}
