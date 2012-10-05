NContext
========

NContext is an application composition framework for .NET 4.0.

This project is an on-going architectural research effort. It borrows many
ideas, patterns, and concepts from a variety of software architects in the
field. NContext is unique and innovative in how it allows you to design your
application without tying it down to any specific technology or architectural
pattern.

It also introduces new concepts in designing your bounded-contexts. With few
core dependencies, NContext's highly-extensible design allows for custom add-on
modules for things like: Entity Framework, Enterprise Library, and ASP.NET Web
API.

NContext is dependency-injection container agnostic. Ninject and Unity support
has been added with the intentions of supporting all popular containers in the
near future.

Project Management
------------------
**Trello:** *https://trello.com/b/xqQg5bfC*  

Continuous Delivery
-------------------

Current efforts are underway to bring NContext to a production-ready state. This 
includes specification tests and an efficient build and deployment strategy.  To 
accomplish this, I have set up the following environments for the community to use:

**CI TeamCity Server:** *https://teamcity.wakingventure.com*  
**CI NuGet Package Source:** *https://nuget.wakingventure.com*  
**CI Symbol Source Server:** *http://symbolsource.wakingventure.com*  

NContext versioning is still being determined. The core libraries (NContext.dll & 
NContext.Common.dll) will be released as 2.0.0. Ideally all extensions will be 
independently versioned, however, the current build process does not reflect this. 
This will be addressed in future builds as updating NuGet references is currently a 
pain since each build packages a new NuGet release for every .nuspec.
