using Moq;

namespace SampleApp.Tests
{
    /// <summary>
    /// AppTests is a clean implementation of those tests.
    /// But this is how you would normally expect to see tests written.
    /// </summary>
    /// <remarks>
    /// The tests in this class copy and paste setup and verification code for each test case.
    /// </remarks>
    public class AppNormalTests
    {
        // Because we can't access private functions
        // all of our tests have to go through the top level Run() function
        
        // Following guidance, the test class implements one test per test case
        // They copy the setup and verification calls for each test case

        [Fact]
        public void Run_DoSomething_Success()
        {
            // Each test function likely instantiates and makes it's own unit under test
            // If the constructor signature changes, every test has to be updated
            Mock<IDependency> MockDependency = new Mock<IDependency>();
            Mock<ILogger> MockLogger = new Mock<ILogger>();
            var mocked = new AppNormal(MockDependency.Object, MockLogger.Object);

            // It needs to return real values to change code flow in another function
            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns("DoSomething");

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

            MockDependency.Verify(mock => mock.DoSomething("DoSomething"), Times.Once);
            MockDependency.Verify(mock => mock.DoSomethingElse(It.IsAny<string>()), Times.Never);

            // It does not write an exception message
            // Because it calls Write in other functions, we have to specify a more specific verification of the string
            MockLogger.Verify(mock => mock.Write(It.Is<string>(s => s.Contains("Exception message:"))), Times.Never);
        }

        [Fact]
        public void Run_DoSomething_Throws()
        {
            // Each test function likely instantiates and makes it's own unit under test
            // If the constructor signature changes, every test has to be updated
            Mock<IDependency> MockDependency = new Mock<IDependency>();
            Mock<ILogger> MockLogger = new Mock<ILogger>();
            var mocked = new AppNormal(MockDependency.Object, MockLogger.Object);

            // It has to retest the whole call stack to test the error handling in DoStuff

            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns("DoSomething");
            
            var expectedException = new Exception("Dependency.DoSomething().mock");
            MockDependency
                .Setup(mock => mock.DoSomething(It.IsAny<string>()))
                .Throws(expectedException);

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

            MockDependency.Verify(mock => mock.DoSomething("DoSomething"),Times.Once);
            MockDependency.Verify(mock => mock.DoSomethingElse(It.IsAny<string>()), Times.Never);

            // It writes the exception message and fails
            MockLogger.Verify(mock => mock.Write($"Exception message: Dependency.DoSomething().mock"), Times.Once);
        }

        [Fact]
        public void Run_DoSomethingElse_Success()
        {
            // Each test function likely instantiates and makes it's own unit under test
            // If the constructor signature changes, every test has to be updated
            Mock<IDependency> MockDependency = new Mock<IDependency>();
            Mock<ILogger> MockLogger = new Mock<ILogger>();
            var mocked = new AppNormal(MockDependency.Object, MockLogger.Object);

            // It needs to return real values to change code flow in another function
            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns("DoSomethingElse");

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

            MockDependency.Verify(mock => mock.DoSomething(It.IsAny<string>()), Times.Never);
            MockDependency.Verify(mock => mock.DoSomethingElse("DoSomethingElse"), Times.Once);

            // It does not write an exception message
            // Because it calls Write in other functions, we have to specify a more specific verification of the string
            MockLogger.Verify(mock => mock.Write(It.Is<string>(s => s.Contains("Exception message:"))), Times.Never);
        }

        [Fact]
        public void Run_DoSomethingElse_Throws()
        {
            // Each test function likely instantiates and makes it's own unit under test
            // If the constructor signature changes, every test has to be updated
            Mock<IDependency> MockDependency = new Mock<IDependency>();
            Mock<ILogger> MockLogger = new Mock<ILogger>();
            var mocked = new AppNormal(MockDependency.Object, MockLogger.Object);

            // It has to retest the whole call stack to test the error handling in DoStuff

            MockDependency
                .Setup(mock => mock.GetSomethingToDo())
                .Returns("DoSomethingElse");

            var expectedException = new Exception("Dependency.DoSomethingElse().mock");
            MockDependency
                .Setup(mock => mock.DoSomethingElse(It.IsAny<string>()))
                .Throws(expectedException);

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

            MockDependency.Verify(mock => mock.DoSomething(It.IsAny<string>()), Times.Never);
            MockDependency.Verify(mock => mock.DoSomethingElse("DoSomethingElse"), Times.Once);

            // It writes the exception message and fails
            MockLogger.Verify(mock => mock.Write($"Exception message: Dependency.DoSomethingElse().mock"), Times.Once);
        }
    }
}