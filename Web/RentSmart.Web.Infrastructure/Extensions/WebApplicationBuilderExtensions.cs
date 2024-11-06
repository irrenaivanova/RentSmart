namespace RentSmart.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class WebApplicationBuilderExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection, Type serviceType)
        {
            Assembly serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service provided!");
            }

            Type[] serviceTypes = serviceAssembly.GetTypes().Where(x => x.Name.EndsWith("Service") && !x.IsInterface).ToArray();

            foreach (Type service in serviceTypes)
            {
                Type interfaceType = service.GetInterfaces().FirstOrDefault(x => x.Name == $"I{service.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException("No matching interface provided!");
                }

                serviceCollection.AddTransient(service, interfaceType);
            }
        }
    }
}
