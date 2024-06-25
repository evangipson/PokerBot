using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Logic.Services.Interfaces;
using PokerBot.View.Controllers.Interfaces;

namespace PokerBot.View.Controllers
{
	[Service(typeof(IApplicationController))]
	public class ApplicationController : IApplicationController
	{
		private readonly ILogger<ApplicationController> _logger;
		private readonly IHandService _handService;

		public ApplicationController(ILogger<ApplicationController> logger, IHandService handService)
		{
			_logger = logger;
			_handService = handService;
		}

		public void Run()
		{
			_handService.GetHand();
			_handService.GetFlop();
		}
	}
}
