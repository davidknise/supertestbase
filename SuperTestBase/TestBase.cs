// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace TestBase
{
    using Moq.Language;
    using Moq.Language.Flow;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;

    /// <summary>
    /// A shared test base that can be used to provide auto-magic tests for classes.
    /// </summary>
    /// <typeparam name="T">The class under test of the derived type.</typeparam>
    public abstract class TestBase<T>
        where T : class
    {
        protected readonly Type Type;

        protected bool TestBaseIsInitialized = false;

        protected ConstructorInfo TestConstructorInfo = null;
        protected ParameterInfo[] TestParameters = null;

        protected Dictionary<Type, Type> ExplicitMocks = null;
        protected Dictionary<Type, int> MockObjectIndexMap = null;
        protected Dictionary<Type, object> Mocks = null;
        protected object[] Objects = null;

        protected Type DefaultConstructorExceptionType = typeof(ArgumentNullException);
        protected Dictionary<Type, Type> ConstructorExceptionTypeMap = new Dictionary<Type, Type>();

        public bool PartialMock { get; set; } = true;
        public bool CallBase { get; set; } = false;
        public MockBehavior MockBehavior { get; set; } = MockBehavior.Default;
        public bool TestConstructor { get; set; } = true;

        private Mock<T> mockObject = null;
        private T mocked = null;

        protected Mock<T> MockObject
        {
            get
            {
                if (!PartialMock)
                {
                    throw new MockTestException("MockObject for non-PartialMock TestBase is not supported.");
                }

                if (mockObject == null)
                {
                    GetMockedSetup();
                    mockObject = GetMockObject();
                    GetMockedPostSetup();
                }

                return mockObject;
            }
        }

        protected T Mocked
        {
            get
            {
                if (mocked == null)
                {
                    GetMockedSetup();
                    mocked = GetMocked();
                    GetMockedPostSetup();
                }

                return mocked;
            }
        }

        public TestBase() : this(true)
        {
            // Default constructor initializes the test base
        }

        public TestBase(bool initialize)
        {
            Type = typeof(T);

            if (initialize)
            {
                InitializeTestBase();
            }
        }

        protected virtual void GetMockedSetup()
        {
            // Stub will be overwritten
        }

        protected virtual void GetMockedPostSetup()
        {
            // Stub will be overwritten
        }

        protected virtual Mock<T> GetMockObject()
        {
            if (Objects == null)
            {
                throw new MocksNotInitializedException(Type);
            }

            var mockObject = new Mock<T>(MockBehavior, Objects.ToArray())
            {
                CallBase = CallBase
            };

            return mockObject;
        }

        protected virtual T GetMocked()
        {
            if (Objects == null)
            {
                throw new MocksNotInitializedException(Type);
            }

            T mocked = null;

            if (PartialMock)
            {
                mocked = MockObject.Object;
            }
            else
            {
                mocked = (T)TestConstructorInfo.Invoke(Objects.ToArray());
            }

            return mocked;
        }

        private object GetMockedObject(object mock, Type type)
        {
            return mock.GetType().GetProperties().Single(p => string.Equals(p.Name, "Object") && p.PropertyType == type).GetValue(mock);
        }

        protected Mock<TMock> GetMock<TMock>()
            where TMock : class
        {
            object mockObject = null;

            if (Mocks == null)
            {
                throw new MocksNotInitializedException(Type);
            }

            if (!Mocks.TryGetValue(typeof(TMock), out mockObject))
            {
                throw new MockDoesNotExistException(typeof(TMock));
            }

            if (mockObject == null)
            {
                throw new MockNotInitializedException(typeof(TMock));
            }

            return (Mock<TMock>)mockObject;
        }

        protected void SetMock<TMock>(Mock<TMock> mock)
            where TMock : class
        {
            if (Mocks == null)
            {
                throw new MocksNotInitializedException(Type);
            }

            // Set the mock
            Mocks[typeof(TMock)] = mock;

            // Replace the mock object
            SetObject(mock.Object);
        }

        protected void SetObject<TObject>(TObject objectParameter)
        {
            int objectIndex = MockObjectIndexMap[typeof(TObject)];
            Objects[objectIndex] = objectParameter;
        }

        protected void SetObject(int index, object objectParameter)
        {
            Objects[index] = objectParameter;
        }

        private Func<TMock> GetMockFactory<TMock>(TMock mockedObject)
        {
            return () => mockedObject;
        }

        private Func<string, TMock> GetMockStringArgumentFactory<TMock>(TMock mockedObject)
        {
            return (string input) => mockedObject;
        }

        private Func<TArgument, TMock> GetMockObjectArgumentFactory<TArgument, TMock>(TArgument mockedArgument, TMock mockedObject)
        {
            return (TArgument input) => mockedObject;
        }

        protected ICallBaseResult SetupCallBase(Expression<Action<T>> expression)
        {
            return MockObject
                .Setup(expression)
                .CallBase();
        }

        protected ICallBaseResult SetupCallBase<T1>(Mock<T1> mock, Expression<Action<T1>> expression)
            where T1 : class
        {
            return mock
                .Setup(expression)
                .CallBase();
        }

        protected IReturnsResult<T> SetupCallBase<TResult>(Expression<Func<T, TResult>> expression)
        {
            return MockObject
                .Setup(expression)
                .CallBase();
        }

        protected IReturnsResult<T1> SetupCallBase<T1, TResult>(Mock<T1> mock, Expression<Func<T1, TResult>> expression)
            where T1 : class
        {
            return mock
                .Setup(expression)
                .CallBase();
        }

        protected ISetup<T1> Setup<T1>(Expression<Action<T1>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.Setup(expression);
        }

        protected ISetup<T1, TResult> Setup<T1, TResult>(Expression<Func<T1, TResult>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.Setup(expression);
        }

        protected ISetupGetter<T1, TProperty> SetupGet<T1, TProperty>(Expression<Func<T1, TProperty>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.SetupGet(expression);
        }

        protected ISetup<T1> SetupSet<T1>(Action<T1> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.SetupSet(expression);
        }

        protected ISetupSetter<T1, TProperty> SetupSet<T1, TProperty>(Action<T1> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.SetupSet<TProperty>(expression);
        }

        protected ISetupSequentialAction SetupSequence<T1>(Expression<Action<T1>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.SetupSequence(expression);
        }

        protected ISetupSequentialResult<TResult> SetupSequence<T1, TResult>(Expression<Func<T1, TResult>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            return mock.SetupSequence(expression);
        }

        /// <summary>
        /// Magically setups up a sequence of returns on the TestBase MockObject with the given list.
        /// </summary>
        protected ISetupSequentialResult<TResult> SetupSequenceList<TResult>(Expression<Func<T, TResult>> expression, IList<TResult> list)
        {
            return SetupSequenceList(MockObject, expression, list);
        }

        /// <summary>
        /// Magically setups up a sequence of returns on the given mock with the given list.
        /// </summary>
        protected ISetupSequentialResult<TResult> SetupSequenceList<T1, TResult>(Mock<T1> mock, Expression<Func<T1, TResult>> expression, IList<TResult> list)
            where T1 : class
        {
            ISetupSequentialResult<TResult> setup = mock.SetupSequence(expression);

            for (int i = 0; i < (list?.Count ?? 0); i++)
            {
                setup = setup.Returns(list[i]);
            }

            return setup;
        }

        /// <summary>
        /// Sets up a string return method mock value for a method on the unit under test (MockObject).
        /// </summary>
        /// <param name="expression">The string-returning function to mock out on the MockObject.</param>
        /// <returns>String of format: MethodName().mock</returns>
        protected IReturnsResult<T> SetupStringReturn(Expression<Func<T, string>> expression)
        {
            return MockObject.SetupStringReturn(expression, false);
        }

        protected void Verify<T1>(Expression<Action<T1>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression);
        }

        protected void Verify<T1>(Expression<Action<T1>> expression, Times times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression, times);
        }

        protected void Verify<T1>(Expression<Action<T1>> expression, Func<Times> times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression, times);
        }

        protected void Verify<T1, TResult>(Expression<Func<T1, TResult>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression);
        }

        protected void Verify<T1, TResult>(Expression<Func<T1, TResult>> expression, Times times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression, times);
        }

        protected void Verify<T1, TResult>(Expression<Func<T1, TResult>> expression, Func<Times> times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.Verify(expression, times);
        }

        protected void VerifyGet<T1, TProperty>(Expression<Func<T1, TProperty>> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.VerifyGet(expression);
        }

        protected void VerifyGet<T1, TProperty>(Expression<Func<T1, TProperty>> expression, Func<Times> times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.VerifyGet(expression, times);
        }

        protected void VerifySet<T1>(Action<T1> expression)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.VerifySet(expression);
        }

        protected void VerifySet<T1>(Action<T1> expression, Func<Times> times)
            where T1 : class
        {
            Mock<T1> mock = GetMock<T1>();
            mock.VerifySet(expression, times);
        }

        protected void InitializeTestBase()
        {
            InitializeTestBase(true);
        }

        protected void InitializeTestBase(bool force)
        {
            if (!force && TestBaseIsInitialized)
            {
                return;
            }

            // Set the Constructor and Parameters
            ConstructorInfo[] constructors = Type.GetConstructors();

            if (constructors == null || constructors.Length <= 0 || constructors.Length > 1)
            {
                throw new TestBaseMultipleConstructorException(Type);
            }

            TestConstructorInfo = constructors[0];
            TestParameters = TestConstructorInfo.GetParameters();

            // Define the Mocks and Objects
            Type mockGenericType = typeof(Mock<>);

            // Find all explicitly mocks in the assembly
            InitializeExplicitMocks();

            // Initialize the mocks
            MockObjectIndexMap = new Dictionary<Type, int>(TestParameters.Length);
            Mocks = new Dictionary<Type, object>(TestParameters.Length);
            Objects = new object[TestParameters.Length];

            // Initialize the constructor parameter mock objects
            for (int i = 0; i < TestParameters.Length; i++)
            {
                Type parameterType = TestParameters[i].ParameterType;

                if (Mocks.ContainsKey(parameterType))
                {
                    throw new DuplicateDependencyTypeException(parameterType);
                }

                Type mockParameterType = parameterType;
                Type mockArgumentType = null;
                object mockParameter = null;
                object mockArgument = null;
                object objectParameter = null;
                bool objectParameterFactory = false;
                bool objectParameterStringArgumentFactory = false;
                bool objectParameterObjectArgumentFactory = false;

                if (ExplicitMocks.ContainsKey(parameterType))
                {
                    // The type has an explicitly defined Mock in the class. Use that mock.
                    Type explicitMockType = ExplicitMocks[parameterType];
                    mockParameter = Activator.CreateInstance(explicitMockType);
                }
                else if (string.Equals(parameterType.Name, "Func`1", StringComparison.Ordinal))
                {
                    // It is a function factory
                    mockParameterType = parameterType.GenericTypeArguments[0];
                    objectParameterFactory = true;
                }
                else if (string.Equals(parameterType.Name, "Func`2", StringComparison.Ordinal))
                {
                    // It is a function factory with one argument
                    mockParameterType = parameterType.GenericTypeArguments[1];
                    mockArgumentType = parameterType.GenericTypeArguments[0];

                    if (string.Equals(mockArgumentType.Name, "String", StringComparison.Ordinal))
                    {
                        objectParameterStringArgumentFactory = true;
                    }
                    else if (ExplicitMocks.ContainsKey(mockArgumentType))
                    {
                        // The type has an explicitly defined Mock in the class. Use that mock.
                        Type explicitMockType = ExplicitMocks[mockArgumentType];
                        mockArgument = Activator.CreateInstance(explicitMockType);
                        objectParameterObjectArgumentFactory = true;
                    }
                    else if (mockArgumentType.GetConstructor(Type.EmptyTypes) != null)
                    {
                        // Type has parameterless constructor - we can create object stright away
                        // As it is used only to specify type of the argument for Function, we can do it such a way
                        mockArgument = Activator.CreateInstance(mockArgumentType);
                        objectParameterObjectArgumentFactory = true;
                    }
                    else
                    {
                        // Note: We used to throw here, but often times, Moq can handle it anyway.
                        // throw new MockTypeNotSupportedException(parameterType);
                    }
                }
                else if (!parameterType.IsInterface)
                {
                    if (parameterType == typeof(string))
                    {
                        mockParameter = $"{TestParameters[i].Name}.fake";
                    }
                    else if (parameterType.IsEnum)
                    {
                        mockParameter = 0; // Default enum value
                    }

                    if (mockParameter != null)
                    {
                        // Object does not get added to Mocks
                        // Multiples of this may exist
                        Objects[i] = mockParameter;
                        continue;
                    }
                }

                if (mockParameter == null)
                {
                    mockParameter = Activator.CreateInstance(mockGenericType.MakeGenericType(mockParameterType));
                }

                if (objectParameterFactory)
                {
                    dynamic mockedObject = GetMockedObject(mockParameter, mockParameterType);
                    objectParameter = GetMockFactory(mockedObject);
                }
                else if (objectParameterStringArgumentFactory)
                {
                    dynamic mockedObject = GetMockedObject(mockParameter, mockParameterType);
                    objectParameter = GetMockStringArgumentFactory(mockedObject);
                }
                else if (objectParameterObjectArgumentFactory)
                {
                    dynamic mockedObject = GetMockedObject(mockParameter, mockParameterType);
                    // dynamic mockedArgument = GetMockedObject(mockArgument, mockArgumentType);
                    objectParameter = GetMockObjectArgumentFactory(mockArgument, mockedObject);
                }
                else if (objectParameter == null)
                {
                    objectParameter = GetMockedObject(mockParameter, mockParameterType);
                }

                // Add the Mock and underlying mocked Object
                Mocks.Add(mockParameterType, mockParameter);
                Objects[i] = objectParameter;
                MockObjectIndexMap.Add(parameterType, i);
            }

            TestBaseIsInitialized = true;
        }

        /// <summary>
        /// Loads in explicitly defined Mock objects with defaulted mock setups.
        /// </summary>
        /// <remarks>
        /// Loads explicit mocks from the test assembly of the target type and
        /// loads explicit mocks form this assembly, the Tests.Common assembly.
        /// </remarks>
        private void InitializeExplicitMocks()
        {
            ExplicitMocks = new Dictionary<Type, Type>();

            Assembly testAssembly = this.GetType().Assembly;
            InitializeExplicitMocks(testAssembly);

            Assembly testsCommonAssembly = typeof(TestBase<>).Assembly;
            InitializeExplicitMocks(testsCommonAssembly);
        }

        /// <summary>
        /// Registers explicitly defined Mock objects within the assembly by registering
        /// any class that starts with the letters Mock.
        /// </summary>
        /// <param name="assembly">The assembly to search for explicit mocks to register.</param>
        private void InitializeExplicitMocks(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name.StartsWith("Mock")
                    && type.BaseType.GenericTypeArguments.Length == 1)
                {
                    // Parent class is Mock<InterfaceType>
                    Type interfaceType = type.BaseType.GenericTypeArguments[0];
                    ExplicitMocks.Add(interfaceType, type);
                }
            }
        }

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
                    if (parameterType == typeof(string) ||
                        parameterType.IsEnum)
                    {
                        TestConstructorInfo.Invoke(constructorObjects);
                        continue;
                    }

                    // could be a class or func factory that gets validated
                }

                TargetInvocationException reflectedActual = Assert.Throws<TargetInvocationException>(() => TestConstructorInfo.Invoke(constructorObjects));

                Assert.Equal(exceptionType, reflectedActual.InnerException?.GetType());
            }
        }
    }
}
