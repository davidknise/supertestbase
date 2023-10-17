namespace SampleApp.Tests
{
    using Moq;
    using SuperTestBase;

    /// <summary>
    /// This is a clean implementation of tests using the TestBase base class of the TestableApp class.
    /// </summary>
    /// <remarks>
    /// Each function is completely isolated and tested on it's own,
    /// allowing for deep test case penetration with ease.
    /// </remarks>
    public class TestableAppTests : TestBase<TestableApp>
    {
        private Mock<IDependency> MockDependency => GetMock<IDependency>();
        private Mock<ILogger> MockLogger => GetMock<ILogger>();
        
        [Theory]
        [ClassData(typeof(TrueOrFalse))]
        public void Run(bool success)
        {
            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns("Dependency.GetSomethingToDo().mock");

            MockObject
                .Setup(mock => mock.DoStuff(It.IsAny<string>()))
                .Returns(success);

            bool actual = Mocked.Run();

            Assert.Equal(success, actual);

            // It writes the app header
            MockLogger.Verify(mock => mock.Write("App.Run()"), Times.Once);

            // It runs setup
            MockObject.Verify(mock => mock.Setup(), Times.Once);

            // It gets something to do
            MockDependency.Verify(mock => mock.GetSomethingToDo(), Times.Once);

            // It does stuff
            MockObject.Verify(mock => mock.DoStuff("Dependency.GetSomethingToDo().mock"), Times.Once;
        }

        [Fact]
        public void Setup_Test()
        {
            SetupCallBase(mock => mock.Setup());

            Mocked.Setup();

            MockLogger.Verify(mock => mock.Write("App.Setup()"), Times.Once);
            MockLogger.Verify(mock => mock.Write("message.fake"), Times.Once);
        }

        [Theory]
        [ClassData(typeof(TrueOrFalse))]
        public void DoStuff_Success(bool shouldDoSomething)
        {
            MockObject
                .Setup(mock => mock.ShouldDoSomething(It.IsAny<string>()))
                .Returns(shouldDoSomething);

            bool actual = Mocked.DoStuff("value.fake");

            // It determines what to do
            MockObject.Verify(mock => mock.ShouldDoSomething("value.fake"), Times.Once);

            // It either does something
            MockDependency.Verify(mock => mock.DoSomething("value.fake"), TimesExt.OnceOrNever(shouldDoSomething));

            // Or it does something else
            MockDependency.Verify(mock => mock.DoSomethingElse("value.fake"), TimesExt.OnceOrNever(!shouldDoSomething));

            // It does not write an exception message
            MockLogger.Verify(mock => mock.Write(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [ClassData(typeof(TrueOrFalse))]
        public void DoStuff_Throws(bool shouldDoSomething)
        {
            MockObject
                .Setup(mock => mock.ShouldDoSomething(It.IsAny<string>()))
                .Returns(shouldDoSomething);

            Exception expectedException;

            if (shouldDoSomething)
            {
                expectedException = new MockTestException("Dependency.DoSomething().mock");
                MockDependency
                    .Setup(mock => mock.DoSomething(It.IsAny<string>()))
                    .Throws(expectedException);
            }
            else
            {
                expectedException = new MockTestException("Dependency.DoSomethingElse().mock");
                MockDependency
                    .Setup(mock => mock.DoSomethingElse(It.IsAny<string>()))
                    .Throws(expectedException);
            }

            bool actual = Mocked.DoStuff("value.fake");

            // It does not succeed
            // It does not fail or throw due to try-catchy
            Assert.False(actual);

            // It determines what to do
            MockObject.Verify(mock => mock.ShouldDoSomething("value.fake"), Times.Once);

            // It either tries to do something
            MockDependency.Verify(mock => mock.DoSomething("value.fake"), TimesExt.OnceOrNever(shouldDoSomething));

            // Or it tries to do something else
            MockDependency.Verify(mock => mock.DoSomethingElse("value.fake"), TimesExt.OnceOrNever(!shouldDoSomething));

            // It writes the exception message and fails
            MockLogger.Verify(mock => mock.Write($"Exception message: {expectedException.Message}"), Times.Once);
        }

        [Theory]
        [ClassData(typeof(IsNullOrWhiteSpaceTestData))]
        public void ShouldDoSomething_IsNullOrWhiteSpace(string value)
        {
            SetupCallBase(mock => mock.Setup());

            bool actual = Mocked.ShouldDoSomething(value);

            // It doesn't null ref (value.Equals(...)), it just returns false
            Assert.False(false);
        }

        [Theory]
        [InlineData("value.fake", false)]
        [InlineData("DoSomething", true)]
        // It is case insensitive
        [InlineData("DOSOMETHING", true)]
        public void ShouldDoSomething(string value, bool expected)
        {
            SetupCallBase(mock => mock.Setup());

            bool actual = Mocked.ShouldDoSomething(value);

            Assert.Equal(expected, actual);
        }
    }
}
