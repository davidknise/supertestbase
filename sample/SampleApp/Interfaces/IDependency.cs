namespace SampleApp
{
    public interface IDependency
    {
        string GetSomethingToDo();
        void DoSomething(string value);
        void DoSomethingElse(string value);
    }
}