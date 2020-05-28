using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ProxyKit;
using System;
using System.Threading.Tasks;

namespace Ynab.Extensions
{
    public static class EndpointRoutingExtensions
    {
        public static IEndpointConventionBuilder MapReverseProxy(this IEndpointRouteBuilder endpoints, string pattern, string forwardTo, Func<ForwardContext, Task> configureOptions)
        {
            var pipeline = endpoints.CreateApplicationBuilder();
            pipeline.RunProxy(async context =>
            {
                var forwardContext = context.ForwardTo(forwardTo);
                await configureOptions.Invoke(forwardContext);
                return await forwardContext.Send();
            });

            return endpoints.Map(pattern, pipeline.Build()).WithDisplayName(pattern);
        }
    }
}
