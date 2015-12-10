NContext
========
NContext is an application composition framework for .NET 4 & 4.5.

This project is an on-going architectural research effort. It borrows many
ideas, patterns, and concepts from a variety of software architects in the
field. NContext is unique and innovative in how it allows you to design your
application without tying it down to any specific technology or architectural
pattern.

At it's core, NContext introduces two fundamental concepts: 
  1. Dynamic application composition (bootstrapping) based upon MEF  
  2. IServiceResponse - a DTO which allows you to create a functionally composable application codebase; one without the use of non-local exits as a use of control flow. With it, the idea of Functional Domain Composition.

It also introduces new concepts in designing your bounded-contexts. With few
core dependencies, NContext's highly-extensible design allows for custom add-on
modules. Already built extensions include: Entity Framework, ASP.NET Web API & 
OWIN, AutoMapper, and Value Injecter. Some application components have 
been baked into NContext including: Caching, Security, Cryptography, Event Handling, 
and Logging.

NContext is dependency-injection container agnostic. Ninject and Unity support
has been added.

Project Management & Status
---------------------------
Version 3 of NContext provided a stable release that's currently being used in 
high-volume production websites.  Version 4 will introduce breaking changes as 
well as transfer project ownership to allow for easier contribution and management 
from parties highly-invested in this codebase.

2016 Roadmap
------------
  1. [x] Migrate project to PowerDMS github
  2. [x] Migrate CI server configurations
  3. [ ] Remove .NET 4.0 support
  4. [ ] Refactor ASP.NET / WebAPI to use OWIN. Remove legacy MS dependencies.
  5. [ ] Remove NContext.Data (UoW / Specification patterns).
  6. [ ] Add support for DryIoC extension.
  7. [ ] Add more IEither abstaction extensions.

License
-------
The MIT License (MIT)

Copyright (c) 2015 Daniel Gioulakis

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
Currently, NContext releases Nuget alpha releases on each commit to master.  This will change to a /dev branch in the future.

NContext versioning is still being determined. The core libraries (NContext.dll & 
NContext.Common.dll) will be released as 2.x.0. Ideally all extensions will be 
independently versioned, however, the current build process does not reflect this. 
This will be addressed in future builds as updating NuGet references is currently a 
pain since each build packages a new NuGet release for every .nuspec.
