using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace Jobnium
{
    public static class Extension
    {
        public static IServiceCollection AddJobs(this IServiceCollection services, string assemblyName, string dbFileFullPath)
        {
            Abstract.StaticValues.DbFilePath = dbFileFullPath;
           
            var serviceInit = AppDomain.CurrentDomain.Load(assemblyName).DefinedTypes
                .Where(t => typeof(IJob).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass)
                .Select(p => p.AsType());

            foreach (var job in serviceInit)
            {
                var instance = job.GetTypeInfo().GetConstructor(Type.EmptyTypes).Invoke(null) as IHostedService;
                services.Add(new ServiceDescriptor(typeof(IHostedService), instance.GetType(),
                    ServiceLifetime.Singleton));
            }

            return services;
        }
    }
}