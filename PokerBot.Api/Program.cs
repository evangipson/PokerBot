using Photino.Blazor;

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
		[STAThread]
		internal static void Main(string[] args) => PhotinoBlazorAppBuilder.CreateDefault(args)
			.ConfigureBuilder()
			.Build()
			.ConfigureApplication()
			.Run();
	}
}