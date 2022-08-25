// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace SuperTestBase
{
    using System;
    using System.Reflection;
    using Xunit;

    public partial class TestBase<T>
        where T : class
    {
        /// <summary>
        /// A shared method that uses reflection to test constructor injected dependencies for
        /// <see cref="DependencyNullException"/> error handling.
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        public virtual void Constructor()
        {
            if (!TestConstructor)
            {
                // Skip test
                return;
            }

            // InitializeMocks if they haven't already been
            InitializeTestBase(false);

            // Test each missing parameter
            for (int parameterIndex = 0; parameterIndex < TestParameters.Length; parameterIndex++)
            {
                object[] constructorObjects = new object[TestParameters.Length];

                for (int dependencyIndex = 0; dependencyIndex < TestParameters.Length; dependencyIndex++)
                {
                    object mockObject = null;

                    if (parameterIndex != dependencyIndex)
                    {
                        mockObject = Objects[dependencyIndex];
                    }

                    constructorObjects[dependencyIndex] = mockObject;
                }

                Type parameterType = TestParameters[parameterIndex].ParameterType;

                Type exceptionType = DefaultConstructorExceptionType;

                if (ConstructorExceptionTypeMap.TryGetValue(parameterType, out Type overrideExceptionType))
                {
                    exceptionType = overrideExceptionType;
                }

                if (exceptionType == null)
                {
                    // Is an optional parameter
                    continue;
                }

                if (!parameterType.IsInterface)
                {
                    if (parameterType == typeof(string)
                         || parameterType.IsEnum)
                    {
                        TestConstructorInfo.Invoke(constructorObjects);
                        continue;
                    }

                    // could be a class or func factory that gets validated
                }

                TargetInvocationException reflectedActual = Assert.Throws<TargetInvocationException>(
                    () => TestConstructorInfo.Invoke(constructorObjects));

                Assert.Equal(exceptionType, reflectedActual.InnerException?.GetType());
            }
        }
    }
}
