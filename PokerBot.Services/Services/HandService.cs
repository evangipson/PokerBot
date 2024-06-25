using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Factories.Interfaces;
using PokerBot.Logic.Services.Interfaces;

namespace PokerBot.Logic.Services
{
    [Service(typeof(IHandService))]
	public class HandService : IHandService
	{
		private readonly ILogger<HandService> _logger;
		private readonly IDeckFactory _deckFactory;
		private readonly List<int> _handSize = Enumerable.Range(0, 2).ToList();
		private readonly List<int> _flopSize = Enumerable.Range(0, 3).ToList();

		public HandService(ILogger<HandService> logger, IDeckFactory deckFactory)
		{
			_logger = logger;
			_deckFactory = deckFactory;
		}

		public Hand GetHand()
		{
			var hand = new Hand();
			_handSize.ForEach(_ => hand.Cards.Add(_deckFactory.DrawCard()));
			_logger.LogInformation($"{nameof(GetHand)} info: Drew hand: {string.Join(", ", hand.Cards)}");
			return hand;
		}

		public Hand GetFlop()
		{
			var flop = new Hand();
			_flopSize.ForEach(_ => flop.Cards.Add(_deckFactory.DrawCard()));
			_logger.LogInformation($"{nameof(GetHand)} info: Drew flop: {string.Join(", ", flop.Cards)}");
			return flop;
		}
	}
}
