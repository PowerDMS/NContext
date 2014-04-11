NContext
========
NContext is an application composition framework for .NET 4 & 4.5.

This project is an on-going architectural research effort. It borrows many
ideas, patterns, and concepts from a variety of software architects in the
field. NContext is unique and innovative in how it allows you to design your
application without tying it down to any specific technology or architectural
pattern.

It also introduces new concepts in designing your bounded-contexts. With few
core dependencies, NContext's highly-extensible design allows for custom add-on
modules for things like: Entity Framework, ASP.NET Web API, AutoMapper, 

NContext is dependency-injection container agnostic. Ninject and Unity support
has been added with the intentions of supporting all popular containers in the
near future.

Project Management
------------------
Feel free to comment and vote on ideas or features you'd like to see incorporated
into NContext.

**Trello:** *https://trello.com/b/xqQg5bfC*  

Continuous Delivery
-------------------
Currently, NContext is being deployed on-demand to reduce development costs. As new feature and bug fixes are brought into the framework and tested, the CI servers are spun-up and deployments are done from the following sites. Pre-release versions are currently deployed to official Nuget/Symbol Source servers. I don't recommend relying on these CI servers for any builds as they aren't live 24/7.

**CI TeamCity Server:** *https://teamcity.wakingventure.com*  
**CI NuGet Package Source:** *https://nuget.wakingventure.com*  
**CI Symbol Source Server:** *http://symbolsource.wakingventure.com*  

NContext versioning is still being determined. The core libraries (NContext.dll & 
NContext.Common.dll) will be released as 2.x.0. Ideally all extensions will be 
independently versioned, however, the current build process does not reflect this. 
This will be addressed in future builds as updating NuGet references is currently a 
pain since each build packages a new NuGet release for every .nuspec.
