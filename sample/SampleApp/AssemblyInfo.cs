#if (DEBUG || DEBUGEXTERNAL)
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SampleApp.Tests")]
// moq framework
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif