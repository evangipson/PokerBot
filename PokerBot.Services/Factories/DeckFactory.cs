using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Factories.Interfaces;

namespace PokerBot.Logic.Factories
{
	/// <inheritdoc cref="IDeckFactory" />
	[Service(typeof(IDeckFactory))]
	public class DeckFactory : IDeckFactory
	{
		private readonly ILogger<DeckFactory> _logger;
		private readonly ICardFactory _cardFactory;
		private static List<Card>? _deck;
		private static readonly Random _randomShuffler = new Random();

		public DeckFactory(ILogger<DeckFactory> logger, ICardFactory cardFactory)
		{
			_logger = logger;
			_cardFactory = cardFactory;
		}

		/// <summary>
		/// A deck of <see cref="Card"/>.
		/// </summary>
		private List<Card> Deck
		{
			get => _deck ?? (_deck = _cardFactory.GetAllCards().ToList());
			set => _deck = value;
		}

		public Card DrawCard()
		{
			var nextCard = _randomShuffler.Next(Deck.Count());
			var deckCard = Deck.ElementAt(nextCard);

			Deck.RemoveAt(nextCard);
			//_logger.LogInformation($"{nameof(DrawCard)} info: Deck has {Deck.Count()} cards remaining.");
			return deckCard;
		}
	}
}
