using Moq;

namespace SampleApp.Tests
{
    /// <summary>
    /// This is a clean implementation of tests for the not very testable App class.
    /// </summary>
    /// <remarks>
    /// While this may look manageable, this only went ONE call stack layer deep.
    /// Most real world applications will have many more layers of function to function
    /// calls within a class. For each nested layer of handling, all of the control logic
    /// and testing must still go through and test the entire application. The more the file
    /// does, the longer each and every test gets, and the more combinations occur.
    /// Furthemrore, the data isn't isolated. A mock value from a depenedency call in App.Run()
    /// changes the control flow in App.DoStuff().
    /// </remarks>
    public class AppTests
    {
        private readonly Mock<IDependency> MockDependency = new Mock<IDependency>();
        private readonly Mock<ILogger> MockLogger = new Mock<ILogger>();

        private App GetApp()
        {
            // I need to make my own mocked unit under test
            // If the constructor signature changes due to adding or removing a dependency
            // this code will stop compiling
            return new App(
                MockDependency.Object,
                MockLogger.Object);
        }

        // Because we can't access private functions
        // all of our tests have to go through the top level Run() function
        
        [Theory]
        [InlineData("DoSomething")]
        [InlineData("DoSomethingElse")]
        // It case insensitive
        [InlineData("DOSOMETHING")]
        // It handles null values
        [InlineData(null)]
        public void Run_Success(string whatToDo)
        {
            // It needs to return real values to change code flow in another function
            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns(whatToDo);

            App mocked = GetApp();

            bool actual = mocked.Run();

            Assert.True(actual);

            // It writes the app header
            MockLogger.Verify(mock => mock.Write("App.Run()"), Times.Once);

            // It can't verify or mock Setup()
            // It can't call Setup() so it has to run and test Setup() here
            MockLogger.Verify(mock => mock.Write("App.Setup()"), Times.Once);
            MockLogger.Verify(mock => mock.Write("message.fake"), Times.Once);

            // It can't verify or mock DetermineWhatToDo
            // It can't call DetermineWhatToDo so it has to run and test DetermineWhatToDo() here
            // It has to copy-paste the control logic to run on test values
            bool doSomething = string.Equals(whatToDo, "DoSomething", StringComparison.Ordinal);

            MockDependency.Verify(mock => mock.DoSomething(whatToDo), doSomething ? Times.Once : Times.Never);
            MockDependency.Verify(mock => mock.DoSomethingElse(whatToDo), !doSomething ? Times.Once : Times.Never);

            // It does not write an exception message
            // Because it calls Write in other functions, we have to specify a more specific verification of the string
            MockLogger.Verify(mock => mock.Write(It.Is<string>(s => s.Contains("Exception message:"))), Times.Never);
        }

        [Theory]
        [InlineData("DoSomething")]
        [InlineData("DoSomethingElse")]
        // It case insensitive
        [InlineData("DOSOMETHING")]
        // It handles null values
        [InlineData(null)]
        public void Run_Throws(string whatToDo)
        {
            // It has to retest the whole call stack to test the error handling in DoStuff
            
            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns(whatToDo);

            bool doSomething = string.Equals(whatToDo, "DoSomething", StringComparison.Ordinal);
            
            Exception expectedException;

            if (doSomething)
            {
                expectedException = new Exception("Dependency.DoSomething().mock");
                MockDependency
                    .Setup(mock => mock.DoSomething(It.IsAny<string>()))
                    .Throws(expectedException);
            }
            else
            {
                expectedException = new Exception("Dependency.DoSomethingElse().mock");
                MockDependency
                    .Setup(mock => mock.DoSomethingElse(It.IsAny<string>()))
                    .Throws(expectedException);
            }

            App mocked = GetApp();

            bool actual = mocked.Run();

            Assert.False(actual);

            // It writes the app header
            MockLogger.Verify(mock => mock.Write("App.Run()"), Times.Once);

            // It can't verify or mock Setup()
            // It can't call Setup() so it has to run and test Setup() here
            MockLogger.Verify(mock => mock.Write("App.Setup()"), Times.Once);
            MockLogger.Verify(mock => mock.Write("message.fake"), Times.Once);

            // It can't verify or mock DetermineWhatToDo
            // It can't call DetermineWhatToDo so it has to run and test DetermineWhatToDo() here
            // It has to copy-paste the control logic to run on test values

            MockDependency.Verify(mock => mock.DoSomething(whatToDo), doSomething ? Times.Once : Times.Never);
            MockDependency.Verify(mock => mock.DoSomethingElse(whatToDo), !doSomething ? Times.Once : Times.Never);

            // It writes the exception message and fails
            MockLogger.Verify(mock => mock.Write($"Exception message: {expectedException.Message}"), Times.Once);
        }
    }
}