using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using InPr.Domain.Services;

namespace InPr.Domain.Providers
{
    public static class ProviderExtentions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddTransient<UserService>();
            services.AddTransient<ArticleService>();
        }

    }
}