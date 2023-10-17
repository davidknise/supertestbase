using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public class TestableApp
    {
        private readonly IDependency Dependency;
        private readonly ILogger Logger;

        public TestableApp(
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

        internal virtual void Setup()
        {
            Logger.Write("App.Setup()");
            Logger.Write("message.fake");
        }

        internal virtual bool DoStuff(string value)
        {
            bool success = true;
            
            try
            {
                if (ShouldDoSomething(value))
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

        internal virtual bool ShouldDoSomething(string value)
        {
            return string.Equals(value, "DoSomething", StringComparison.OrdinalIgnoreCase);
        }
    }
}
