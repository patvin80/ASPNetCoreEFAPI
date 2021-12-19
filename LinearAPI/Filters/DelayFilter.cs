using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinearAPI.Filters
{
    //https://robertwray.co.uk/blog/adding-a-delay-to-asp-net-core-web-api-methods-to-simulate-slow-or-erratic-networks
    public class DelayFilter : IAsyncActionFilter
    {
        private int _delayInMs;
        public DelayFilter(IConfiguration configuration)
        {
            _delayInMs = configuration.GetValue<int>("ApiDelayDuration", 0);
        }

        async Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Task.Delay(_delayInMs);
            await next();
        }
    }
}
