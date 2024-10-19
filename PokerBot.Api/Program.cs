using PokerBot.Api.Extensions;

namespace PokerBot.Api
{
	/// <summary>
	/// A class which is referenced when the application starts.
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// Configures and runs the application.
		/// </summary>
		internal static void Main() => WebApplication.CreateBuilder()
			.ConfigureBuilder()
			.Build()
			.ConfigureApplication()
			.Run();
	}
}