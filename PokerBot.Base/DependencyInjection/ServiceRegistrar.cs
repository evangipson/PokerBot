using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PokerBot.Base.DependencyInjection
{
	/// <summary>
	/// A collection of <c>static</c> methods intended to
	/// be used to add services which utilize the
	/// <c>[<see cref="ServiceAttribute">Service</see>]</c>
	/// attribute in their concrete implementations.
	/// </summary>
	public static class ServiceRegistrar
	{
		/// <summary>
		/// Adds all classes that use the
		/// <c>[<see cref="ServiceAttribute">Service</see>]</c>
		/// attribute within the provided <paramref name="assembly"/>
		/// to the <paramref name="serviceCollection"/> that invokes it.
		/// </summary>
		/// <param name="serviceCollection">
		/// The dependency injection service collection.
		/// </param>
		/// <param name="assembly">
		/// The assembly to add services from.
		/// </param>
		public static void AddServicesFromAssembly(this IServiceCollection serviceCollection, Assembly? assembly)
		{
			if(assembly?.ExportedTypes == null)
			{
				throw new ApplicationException(
					$"{nameof(AddServicesFromAssembly)} error: the {nameof(assembly)} assembly that was provided has no \"ExportedTypes\" to register to the dependency injection service collection."
				);
			}

			foreach (var type in assembly.ExportedTypes)
			{
				var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
				if (serviceAttribute?.ServiceType != null)
				{
					serviceCollection.AddService(serviceAttribute.Lifetime, serviceAttribute.ServiceType, type);
				}
			}
		}

		/// <summary>
		/// Adds a service to the <paramref name="serviceCollection"/>
		/// based on the <paramref name="serviceLifetime"/>.
		/// </summary>
		/// <param name="serviceCollection">
		/// A dependency injection collection of services.
		/// </param>
		/// <param name="serviceLifetime">
		/// The <see cref="ServiceLifetime"/> of the service to be
		/// added to the <paramref name="serviceCollection"/>.
		/// </param>
		/// <param name="serviceInterface">
		/// The interface <see cref="Type"/> of the service to be
		/// added to the <paramref name="serviceCollection"/>.
		/// </param>
		/// <param name="serviceImplementation">
		/// The concrete <see cref="Type"/> of the service to be
		/// added to the <paramref name="serviceCollection"/>.
		/// </param>
		private static void AddService(this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime, Type serviceInterface, Type serviceImplementation)
		{
			switch (serviceLifetime)
			{
				case ServiceLifetime.Singleton:
					serviceCollection.AddSingleton(serviceInterface, serviceImplementation);
					break;
				case ServiceLifetime.Scoped:
					serviceCollection.AddScoped(serviceInterface, serviceImplementation);
					break;
				case ServiceLifetime.Transient:
					serviceCollection.AddTransient(serviceInterface, serviceImplementation);
					break;
				default:
					throw new ApplicationException(
						$"{nameof(AddServicesFromAssembly)} error: unknown {nameof(ServiceLifetime)} encountered while adding the {serviceInterface} service type to the dependency injection service collection."
					);
			}
		}
	}
}
