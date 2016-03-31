NContext
========
NContext is an application composition framework for >= .NET 4.5.2.

This project is an on-going architectural research effort. It borrows many
ideas, patterns, and concepts from a variety of software architects in the
field. NContext is unique and innovative in how it allows you to design your
application without tying it down to any specific technology or architectural
pattern.

At it's core, NContext introduces two fundamental concepts: 
  1. Dynamic application composition (bootstrapping) based upon MEF  
  2. IServiceResponse - a DTO which allows you to create a functionally composable application codebase; one without the use of non-local exits as a use of control flow.

It also introduces new concepts in designing your bounded-contexts. With few
core dependencies, NContext's highly-extensible design allows for custom add-on
modules. Already built extensions include: ASP.NET Web API, AutoMapper, and Redis. Some application components have 
been baked into NContext including: Caching, Security, Cryptography, Event Handling, 
and Logging.

NContext is dependency-injection container agnostic. Ninject support has been added.

Project Management & Status
---------------------------
Version 3 of NContext provided a stable release that's currently being used in 
high-volume production websites.  Version 4 will introduce breaking changes. 
Transfer of project ownership to allow for easier contribution and management 
from parties highly-invested in this codebase.

2016 Roadmap
------------
  1. [x] Migrate project to PowerDMS github
  2. [x] Migrate CI server configurations
  3. [x] Remove .NET 4.0 support and extensions not in active development
  4. [x] Refactor ASP.NET / WebAPI and remove legacy MS dependencies
  5. [x] Remove NContext.Data
  6. [ ] Add support for DryIoC extension
  7. [ ] Add more IEither abstaction extensions

License
-------
The MIT License (MIT)

Copyright (c) Daniel Gioulakis, PowerDMS Inc. 2016

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

Continuous Delivery
-------------------
Currently, NContext releases Nuget alpha releases on each commit to master.  This will change to feature branching in the future.

The core libraries (NContext.dll & NContext.Common.dll) will be released as 4.x.x. 
Ideally all extensions will be independently versioned, however, the current build process does not reflect this. 
This will be addressed in future builds as updating NuGet references is currently a pain since each build packages 
a new NuGet release for every .nuspec.