# SuperTestBase

A TestBase class for auto mock and constructor testing and other test helpers using [Moq]().

## Code Guidelines

* Reach every function from test code:
  * Use `internal` instead of `private`
  * Add an [`AssemblyInfo.cs`](/sample/ampleApp/AssemblyInfo.cs) file to each project that exposes internals to the test assembly
  * **What about encapsulation?** You should not be calling your class directly but exposing an **interface** that matches your public functions. Whether your functions are `private` or `internal` shouldn't be noticeable anywhere.
* Mark functions with `virtual` to allow Moq to override them
  * In addition to a mocked unit under test, you will also get a `MockObject` which allows you to mock and verify other functions on your unit under test. This only works if Moq can override your function.
  * Additional requirement - can't use `sealed` on the class, as Moq needs to inherit from it.
* Write small functions!
  * **Single Responsibility Principle (SRP)** - have your functions do one thing to help isolate code to test.
  * **Beware of the word "and"** - if your function does this *and* this *and* that, it likely does too much. 
* Do not use `static` classes or functions
  * You can't mock it out and thus have to also test what the static function does from each and every caller.
  * If you really need a static class, make it a singleton with an `Instance` property as a get-out-of-jail free card.

## SampleApp

See the **SampleApp** in the solution or `/sample` folder.

`App.cs` is how code would be expected to be written.

`TestableApp.cs` is how the code looks like adhering to these code guidelines for testability.

The associated [AppTests](/sample/SampleApp.Tests/AppTests.cs) and (TestableAppTests)(/sample/SampleApp.Tests/TestableAppTests.cs) demonstrate the difference in testability.