namespace SampleApp
{
    public class App
    {
        private readonly IDependency Dependency;
        private readonly ILogger Logger;

        public App(
            IDependency dependency,
            ILogger logger)
        {
            Dependency = dependency ?? throw new ArgumentNullException("dependency");
            Logger = logger ?? throw new ArgumentNullException("logger");
        }

        public bool Run()
        {
            Logger.Write("App.Run()");

            Setup();

            string value = Dependency.GetSomethingToDo();

            bool success = DoStuff(value);

            return success;
        }

        private void Setup()
        {
            Logger.Write("App.Setup()");
            Logger.Write("message.fake");
        }

        private bool DoStuff(string value)
        {
            bool success = true;

            try
            {
                if (string.Equals(value, "DoSomething", StringComparison.Ordinal))
                {
                    Dependency.DoSomething(value);
                }
                else
                {
                    Dependency.DoSomethingElse(value);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"Exception message: {ex.Message}");
                success = false;
            }

            return success;
        }
    }
}