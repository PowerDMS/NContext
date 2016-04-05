using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("PowerDMS Inc.")]
[assembly: AssemblyProduct("NContext")]
[assembly: AssemblyCopyright("Copyright © Daniel Gioulakis, PowerDMS Inc. 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: Guid("763725d3-7444-4baf-84ff-d15242d71929")]
[assembly: AssemblyTitle("NContext")]
[assembly: AssemblyDescription("NContext is an application composition framework for .NET.")]

[assembly: InternalsVisibleTo("NContext.Tests.Specs")]

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.0.0")]
[assembly: AssemblyInformationalVersion("0.0.0.0")]