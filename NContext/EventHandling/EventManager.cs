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
    using System.Threading;
    using NContext.Configuration;
    using NContext.Extensions;

    public class EventManager : IApplicationComponent
    {
        private readonly IDictionary<Type, IEnumerable<Type>> _AsyncCache;
        private readonly IActivationProvider _ActivationProvider;
        private CompositionContainer _CompositionContainer;

        public EventManager(IActivationProvider activationProvider)
        {
            _AsyncCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();
            _ActivationProvider = activationProvider;
        }

        public void Raise<TEvent>(TEvent @event)
        {
            IEnumerable<Type> asyncHandlerTypes;
            if (_AsyncCache[typeof(TEvent)] != null)
            {
                asyncHandlerTypes = _AsyncCache[typeof(TEvent)];
            }
            else
            {
                _AsyncCache[typeof(TEvent)] = asyncHandlerTypes = _CompositionContainer.GetExportedTypes<IHandleAsync<TEvent>>();
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            
            asyncHandlerTypes
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .WithCancellation(token)
                .ForAll(handlerType =>
                    {
                        IHandle<TEvent> handler = _ActivationProvider.CreateInstance<TEvent>(handlerType);
                        try
                        {
                            handler.Handle(@event);
                        }
                        catch (Exception ex)
                        {
                            handler.HandleException(@event, ex);
                            cancellationAction.Invoke(@event, handler, ex);
                        }
                    });

            _CompositionContainer
                .GetExportedTypes<IHandle<TEvent>>()
                .ForEach(
                    handlerType =>
                    RaiseEvent(@event, handlerType, (eventSource, handler, exception) => {handler.CancelEvent(eventSource, exception)}));
        }

        private void RaiseEvent<TEvent>(TEvent @event, Type handlerType, Action<TEvent, IHandle<TEvent>, Exception> cancellationAction)
        {
            var handler = _ActivationProvider.CreateInstance<TEvent>(handlerType);

            try
            {
                handler.Handle(@event);
            }
            catch (Exception ex)
            {
                cancellationAction.Invoke(@event, handler, ex);
            }
        }

        public Boolean IsConfigured { get; private set; }

        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            _CompositionContainer = applicationConfiguration.CompositionContainer;
        }
    }

    public class EventManagerBuilder : ApplicationComponentBuilder
    {
        public EventManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) 
            : base(applicationConfigurationBuilder)
        {
        }

        public EventManagerBuilder SetActivationProvider(Func<IActivationProvider> activationProviderFactory)
        {
            
        }
    }

    public interface IActivationProvider
    {
        IHandle<TEvent> CreateInstance<TEvent>(Type handler);
    }

    [InheritedExport]
    public interface IHandle<TEvent>
    {
        void Handle(TEvent @event);

        void HandleException(TEvent @event, Exception exception);
    }

    [InheritedExport]
    public interface IHandleAsync<TEvent>
    {
        void Handle(TEvent @event);

        void HandleException(TEvent @event, Exception exception, CancellationTokenSource cancellationTokenSource);
    }
}