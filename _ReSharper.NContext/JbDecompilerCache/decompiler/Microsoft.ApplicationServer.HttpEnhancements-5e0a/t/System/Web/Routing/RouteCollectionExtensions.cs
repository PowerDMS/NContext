// Type: System.Web.Routing.RouteCollectionExtensions
// Assembly: Microsoft.ApplicationServer.HttpEnhancements, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Projects\NContext\packages\WebApi.Enhancements.0.5.0\lib\40-Full\Microsoft.ApplicationServer.HttpEnhancements.dll

using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Activation;

namespace System.Web.Routing
{
  public static class RouteCollectionExtensions
  {
    private static object lockObject = new object();
    private static HttpConfiguration defaultConfiguration;

    static RouteCollectionExtensions()
    {
      lock (RouteCollectionExtensions.lockObject)
        RouteCollectionExtensions.defaultConfiguration = (HttpConfiguration) new WebApiConfiguration(true);
    }

    public static void SetDefaultHttpConfiguration(this RouteCollection routes, HttpConfiguration configuration)
    {
      RouteCollectionExtensions.defaultConfiguration = configuration;
    }

    public static HttpConfiguration GetDefaultHttpConfiguration(this RouteCollection routes)
    {
      return RouteCollectionExtensions.defaultConfiguration;
    }

    public static void MapServiceRoute<TService>(this RouteCollection routes, string routePrefix, HttpConfiguration configuration = null, object constraints = null, bool useMethodPrefixForHttpMethod = true)
    {
      if (configuration == null)
        configuration = RouteCollectionExtensions.defaultConfiguration;
      if (routes == null)
        throw new ArgumentNullException("routes");
      string routePrefix1 = routePrefix;
      HttpServiceHostFactory serviceHostFactory1 = new HttpServiceHostFactory();
      serviceHostFactory1.set_Configuration(configuration);
      HttpServiceHostFactory serviceHostFactory2 = serviceHostFactory1;
      Type serviceType = typeof (TService);
      WebApiRoute webApiRoute = new WebApiRoute(routePrefix1, (ServiceHostFactoryBase) serviceHostFactory2, serviceType);
      webApiRoute.Constraints = new RouteValueDictionary(constraints);
      ((Collection<RouteBase>) routes).Add((RouteBase) webApiRoute);
    }
  }
}
