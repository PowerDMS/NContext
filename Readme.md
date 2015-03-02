NContext
========
NContext is an application composition framework for .NET 4 & 4.5.

This project is an on-going architectural research effort. It borrows many
ideas, patterns, and concepts from a variety of software architects in the
field. NContext is unique and innovative in how it allows you to design your
application without tying it down to any specific technology or architectural
pattern.

At it's core, NContext introduces two fundamental concepts: dynamic application
composition (bootstrapping) based upon MEF and IResponseTransferObject - a DTO
which allows you to create a functionally composable application codebase. With 
it, the idea of Functional Domain Composition.

It also introduces new concepts in designing your bounded-contexts. With few
core dependencies, NContext's highly-extensible design allows for custom add-on
modules. Already built extensions include: Entity Framework, ASP.NET Web API & 
OWIN, AutoMapper, Value Injecter, and Logging. Some application components have 
been baked into NContext including: Caching, Security, Cryptography.

NContext is dependency-injection container agnostic. Ninject and Unity support
has been added with the intentions of supporting all popular containers in the
near future.

License
-------
The MIT License (MIT)

Copyright (c) 2014 Daniel Gioulakis

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.


Project Management
------------------
Feel free to comment and vote on ideas or features you'd like to see incorporated
into NContext.

**Trello:** *https://trello.com/b/xqQg5bfC*  

Continuous Delivery
-------------------
Currently, NContext is being deployed on-demand to reduce development costs. As new feature and bug fixes are brought into the framework and tested, the CI servers are spun-up and deployments are done from the following sites. Pre-release versions are currently deployed to official Nuget/Symbol Source servers. I don't recommend relying on these CI servers for any builds as they aren't live 24/7.

**CI TeamCity Server:** *https://teamcity.dgdev.net*  
**CI NuGet Package Source:** *https://nuget.dgdev.net*  
**CI Symbol Source Server:** *http://symbolsource.dgdev.net*  

NContext versioning is still being determined. The core libraries (NContext.dll & 
NContext.Common.dll) will be released as 2.x.0. Ideally all extensions will be 
independently versioned, however, the current build process does not reflect this. 
This will be addressed in future builds as updating NuGet references is currently a 
pain since each build packages a new NuGet release for every .nuspec.
