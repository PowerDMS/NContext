// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManager.cs" company="Waking Venture, Inc.">
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

namespace NContext.EventHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Threading.Tasks;

    using NContext.Configuration;
    using NContext.Extensions;

    public class EventManager : IManageEvents
    {
        readonly IDictionary<Type, IEnumerable<Type>> _EventHandlerCache;

        readonly IActivationProvider _ActivationProvider;

        CompositionContainer _CompositionContainer;

        Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        /// <param name="activationProvider">The activation provider.</param>
        public EventManager(IActivationProvider activationProvider)
        {
            _EventHandlerCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();
            _ActivationProvider = activationProvider;
        }

        public Task Raise<TEvent>(TEvent @event)
        {
#if NET45
            return Task.Run(() => RaiseEvent(@event));
#else
            return Task.Factory.StartNew(() => RaiseEvent(@event));
#endif
        }

        private void RaiseEvent<TEvent>(TEvent @event)
        {
            IEnumerable<Type> handlerTypes;
            if (_EventHandlerCache.ContainsKey(typeof(TEvent)))
            {
                handlerTypes = _EventHandlerCache[typeof(TEvent)];
            }
            else
            {
                _EventHandlerCache[typeof(TEvent)] = handlerTypes = _CompositionContainer.GetExportTypesThatImplement<IHandleEvent<TEvent>>().ToList();
            }

            handlerTypes
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .ForAll(handlerType =>
                    {
                        try
                        {
                            var handler = _ActivationProvider.CreateInstance<TEvent>(handlerType);

                            try
                            {
                                handler.Handle(@event);
                            }
                            catch (Exception ex)
                            {
                                handler.HandleException(@event, ex);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }

        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageEvents>(this);
            _CompositionContainer = applicationConfiguration.CompositionContainer;

            _IsConfigured = true;
        }
    }
}