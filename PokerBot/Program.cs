using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Logic.Factories.Interfaces;
using PokerBot.Logic.Services.Interfaces;
using PokerBot.View.Controllers.Interfaces;

namespace PokerBot.View
{
    internal class Program
	{
		private static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.Title = "Poker Bot";

			// setup our DI
			var serviceCollection = new ServiceCollection().AddLogging(cfg => cfg.AddConsole());

			// add PokerBot services from PokerBot.Services and PokerBot.View using reflection
			serviceCollection.AddServicesFromAssembly(Assembly.GetAssembly(typeof(ICardFactory)));
			serviceCollection.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IHandService)));
			serviceCollection.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IApplicationController)));

			// instantiate depenedency injection concrete object
			var serviceProvider = serviceCollection.BuildServiceProvider();

			// start the application by getting the Application
			// class from the required services, and run it.
			serviceProvider.GetRequiredService<IApplicationController>().Run();

			return;
		}
	}
}