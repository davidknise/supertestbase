namespace SampleApp
{
    public class AppNormal
    {
        private readonly IDependency Dependency;
        private readonly ILogger Logger;

        public AppNormal(
            IDependency dependency,
            ILogger logger)
        {
            Dependency = dependency ?? throw new ArgumentNullException("dependency");
            Logger = logger ?? throw new ArgumentNullException("logger");
        }

        public bool Run()
        {
            Logger.Write("App.Run()");

            Logger.Write("App.Setup()");
            Logger.Write("message.fake");

            string value = Dependency.GetSomethingToDo();
            
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