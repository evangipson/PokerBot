using Microsoft.Extensions.Logging;

using PokerBot.Domain.Models;
using PokerBot.Logic.Factories;

namespace PokerBot.Logic.Services
{
    /// <inheritdoc cref="IHandService" />
	public class HandService(ILogger<HandService> logger, IDeckFactory deckFactory) : IHandService
	{
		private readonly List<int> _handSize = Enumerable.Range(0, 2).ToList();
		private readonly List<int> _flopSize = Enumerable.Range(0, 3).ToList();

		public Hand GetHand()
		{
			var hand = new Hand();
			_handSize.ForEach(_ => hand.Cards.Add(deckFactory.DrawCard()));
			logger.LogInformation($"{nameof(GetHand)} info: Drew hand: {string.Join(", ", hand.Cards)}");
			return hand;
		}

		public IEnumerable<Card> GetFlop()
		{
			var flop = new List<Card>();
			_flopSize.ForEach(_ => flop.Add(deckFactory.DrawCard()));
			logger.LogInformation($"{nameof(GetHand)} info: Drew flop: {string.Join(", ", flop)}");
			return flop;
		}

		public Card GetRiver()
		{
			var riverCard = deckFactory.DrawCard();
			logger.LogInformation($"{nameof(GetHand)} info: Drew river: {riverCard}");
			return riverCard;
		}

		public Card GetTurn()
		{
			var turnCard = deckFactory.DrawCard();
			logger.LogInformation($"{nameof(GetHand)} info: Drew turn: {turnCard}");
			return turnCard;
		}
	}
}
