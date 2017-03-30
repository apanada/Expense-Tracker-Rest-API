﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ExpenseTracker.API
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Register()
        {
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultRouting",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // clear the supported mediatypes of the xml formatter
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json-patch+json"));

            // results should come out
            // - with indentation for readability
            // - in camelCase
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return config;
             
        }
    }
}
