namespace RentSmart.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class WebApplicationBuilderExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection, Type service)
        {
            Assembly serviceAssembly = Assembly.GetAssembly(service);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service provided!");
            }

            Type[] serviceTypes = serviceAssembly.GetTypes().Where(x => x.Name.EndsWith("Service") && !x.IsInterface).ToArray();

            foreach (Type serviceType in serviceTypes)
            {
                Type interfaceType = serviceType.GetInterfaces().FirstOrDefault(x => x.Name == $"I{serviceType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException("No matching interface provided!");
                }

                serviceCollection.AddTransient(interfaceType, serviceType);
            }
        }
    }
}
