using PokerBot.Api.Components;
using PokerBot.Logic.Factories;
using PokerBot.Logic.Services;

namespace PokerBot.Api.Extensions
{
	/// <summary>
	/// A collection of extension methods for <see cref="WebApplication"/>
	/// and <see cref="WebApplicationBuilder"/>.
	/// </summary>
	internal static class BootstrapExtensions
	{
		/// <summary>
		/// Adds configuration options, dependency-injected services, controllers,
		/// and http client configuration to the application builder.
		/// </summary>
		/// <param name="builder">
		/// The builder for the application.
		/// </param>
		/// <returns>
		/// The provided <paramref name="builder"/> with all the configurations implemented.
		/// </returns>
		internal static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
		{
			builder.AddConfigurationOptions();
			builder.AddServices();
			builder.Services.AddSassCompiler();
			builder.Services.AddRazorComponents().AddInteractiveServerComponents();

			return builder;
		}

		/// <summary>
		/// Sets up exception handling, maps routes, and adds HTTPS to the application.
		/// </summary>
		/// <param name="webApplication">
		/// The built web application to configure.
		/// </param>
		/// <returns>
		/// The provided <paramref name="webApplication"/> with all the configurations implemented.
		/// </returns>
		internal static WebApplication ConfigureApplication(this WebApplication webApplication)
		{
			webApplication.UseExceptionHandler("/error");
			webApplication.AddHttps();
			webApplication.UseStaticFiles();
			webApplication.MapRazorComponents<App>().AddInteractiveServerRenderMode();

			return webApplication;
		}

		/// <summary>
		/// Adds options to the application configuration, which provides access to sections
		/// of the application settings.
		/// </summary>
		/// <param name="builder">
		/// The builder for the application.
		/// </param>
		private static void AddConfigurationOptions(this WebApplicationBuilder builder)
		{
			//var basicApplicationSettings = builder.Configuration.GetSection(nameof(BasicApplicationSettings));
			//var complexApplicationSettings = builder.Configuration.GetSection(nameof(ComplexApplicationSettings));
			//var dataProvidersSettings = builder.Configuration.GetSection(nameof(DataProviders)).Get<List<DataProvider>>();

			//_ = builder.Services.AddOptions<BasicApplicationSettings>().Bind(basicApplicationSettings);
			//_ = builder.Services.AddOptions<ComplexApplicationSettings>().Bind(complexApplicationSettings);
			//builder.Services.Configure<DataProviders>(options => options.Providers = dataProvidersSettings);
		}

		/// <summary>
		/// Adds dependency-injected services with their scope to the application builder
		/// services container.
		/// </summary>
		/// <param name="builder">
		/// The builder for the application.
		/// </param>
		private static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services
				.AddScoped<ICardFactory, CardFactory>()
				.AddScoped<IDeckFactory, DeckFactory>()
				.AddScoped<IHandService, HandService>()
				.AddTransient<IScoringService, ScoringService>()
				.AddRazorPages();
		}

		/// <summary>
		/// Adds https to the provided <paramref name="webApplication"/>.
		/// </summary>
		/// <param name="webApplication">
		/// The <see cref="WebApplication"/> to add https to.
		/// </param>
		private static void AddHttps(this WebApplication webApplication)
		{
			webApplication.UseHttpsRedirection();
			webApplication.UseAuthorization();
			webApplication.UseAntiforgery();
		}
	}
}
