using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Waking Venture Inc.")]
[assembly: AssemblyProduct("NContext")]
[assembly: AssemblyCopyright("Copyright © Waking Venture Inc. 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0 Alpha")]

[assembly: Guid("763725d3-7444-4baf-84ff-d15242d71929")]
[assembly: AssemblyTitle("NContext")]
[assembly: AssemblyDescription("NContext is an application composition framework for .NET 4.0.")]

[assembly: InternalsVisibleTo("NContext.Tests.Specs")]

[assembly: AssemblyFileVersion("2.0.0.0")]

[assembly: CLSCompliant(true)]