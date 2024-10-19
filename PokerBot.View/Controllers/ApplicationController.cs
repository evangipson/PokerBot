using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Services;
using PokerBot.Logic.Services.Interfaces;
using PokerBot.View.Controllers.Interfaces;

namespace PokerBot.View.Controllers
{
    /// <inheritdoc cref="IApplicationController" />
    [Service(typeof(IApplicationController))]
	public class ApplicationController : IApplicationController
	{
		private readonly ILogger<ApplicationController> _logger;
		private readonly IHandService _handService;
		private readonly IScoringService _scoringService;

		public ApplicationController(ILogger<ApplicationController> logger, IHandService handService, IScoringService scoringService)
		{
			_logger = logger;
			_handService = handService;
			_scoringService = scoringService;
		}

		public void Run()
		{
			List<Card> totalHand = [ .. _handService.GetHand().Cards];
			totalHand.AddRange(_handService.GetFlop());
			totalHand.Add(_handService.GetRiver());
			totalHand.Add(_handService.GetTurn());

			_scoringService.ScoreHand(totalHand);
		}
	}
}
