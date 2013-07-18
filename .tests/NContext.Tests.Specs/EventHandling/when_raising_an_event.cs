// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_raising_an_event.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Tests.Specs.EventHandling
{
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;

    using Machine.Specifications;

    using NContext.Configuration;
    using NContext.EventHandling;

    using Telerik.JustMock;

    public class when_raising_an_event
    {
        private static IManageEvents _EventManager;
        private static IActivationProvider _ActivationProvider;

        Establish context = () =>
            {
                var appConfig = Mock.Create<ApplicationConfigurationBase>();
                _ActivationProvider = Mock.Create<IActivationProvider>();

                Mock.Arrange(() => appConfig.CompositionContainer)
                    .Returns(new CompositionContainer(new AggregateCatalog(new AssemblyCatalog(Assembly.Load("NContext")), new AssemblyCatalog(Assembly.GetExecutingAssembly()))));
                
                _EventManager = new EventManager(ActivationProvider);
                _EventManager.Configure(appConfig);
            };

        protected static IManageEvents EventManager
        {
            get { return _EventManager; }
        }

        protected static IActivationProvider ActivationProvider
        {
            get { return _ActivationProvider; }
        }
    }
}