using Photino.Blazor;

using PokerBot.Api.Components;
using PokerBot.Api.Services;
using PokerBot.Logic.Factories;
using PokerBot.Logic.Services;

namespace PokerBot.Api.Extensions
{
	/// <summary>
	/// A collection of extension methods for <see cref="PhotinoBlazorApp"/>
	/// and <see cref="PhotinoBlazorAppBuilder"/>.
	/// </summary>
	internal static class BootstrapExtensions
	{
		/// <summary>
		/// Adds configuration options, dependency-injected services, controllers,
		/// and root components to the <see cref="PhotinoBlazorAppBuilder"/>.
		/// </summary>
		/// <param name="builder">
		/// The builder for the application.
		/// </param>
		/// <returns>
		/// The provided <paramref name="builder"/> with all the configurations implemented.
		/// </returns>
		internal static PhotinoBlazorAppBuilder ConfigureBuilder(this PhotinoBlazorAppBuilder builder)
		{
			builder.AddServices();
			builder.Services.AddSassCompiler();
			builder.Services.AddLogging();
			builder.RootComponents.Add<App>("app");

			return builder;
		}

		/// <summary>
		/// Sets up necessary configuration for the <see cref="PhotinoBlazorApp"/>.
		/// </summary>
		/// <param name="photinoApp">
		/// The built web application to configure.
		/// </param>
		/// <returns>
		/// The provided <paramref name="photinoApp"/> with all the configurations implemented.
		/// </returns>
		internal static PhotinoBlazorApp ConfigureApplication(this PhotinoBlazorApp photinoApp)
		{
			photinoApp.MainWindow.SetTitle("PokerBot").SetIconFile("wwwroot/favicon.ico");
			AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
			{
				photinoApp.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
			};

			return photinoApp;
		}

		/// <summary>
		/// Adds dependency-injected services with their scope to the application builder
		/// services container.
		/// </summary>
		/// <param name="builder">
		/// The builder for the application.
		/// </param>
		private static void AddServices(this PhotinoBlazorAppBuilder builder)
		{
			builder.Services
				.AddScoped<ICardFactory, CardFactory>()
				.AddScoped<IDeckFactory, DeckFactory>()
				.AddScoped<IHandService, HandService>()
				.AddTransient<IScoringService, ScoringService>()
				.AddScoped<IDraggableService, DraggableService>();
		}
	}
}
