using Microsoft.Extensions.Logging;

using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories
{
    /// <inheritdoc cref="IDeckFactory" />
	public class DeckFactory(ILogger<DeckFactory> logger, ICardFactory cardFactory) : IDeckFactory
	{
		private List<Card>? _deck;
		private static readonly Random _randomShuffler = new();

		/// <summary>
		/// A deck of <see cref="Card"/>.
		/// </summary>
		private List<Card> Deck
		{
			get => _deck ??= cardFactory.GetAllCards().ToList();
			set => _deck = value;
		}

		public Card DrawCard()
		{
			var nextCard = _randomShuffler.Next(Deck.Count);
			var deckCard = Deck.ElementAt(nextCard);

			Deck.RemoveAt(nextCard);
			//_logger.LogInformation($"{nameof(DrawCard)} info: Deck has {Deck.Count()} cards remaining.");
			return deckCard;
		}

		public void ShuffleDeck()
		{
			Deck = cardFactory.GetAllCards().ToList();
		}
	}
}
