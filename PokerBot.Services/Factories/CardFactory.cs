using Microsoft.Extensions.Logging;

using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories
{
    /// <inheritdoc cref="ICardFactory" />
	public class CardFactory(ILogger<CardFactory> logger) : ICardFactory
	{
		private static readonly Random _randomShuffler = new();
		private static readonly Dictionary<Suit, string> _suitSymbols = new()
		{
			[Suit.Clubs] = "♧",
			[Suit.Spades] = "♤",
			[Suit.Hearts] = "♡",
			[Suit.Diamonds] = "♢"
		};
		private readonly IEnumerable<string> _rankSymbols = Enumerable.Range(1, 13).Select(x =>
		{
			return x switch
			{
				1 => "Ace",
				2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 => x.ToString(),
				11 => "Jack",
				12 => "Queen",
				13 => "King",
				_ => "Joker",
			};
		});

		public Card MakeCard()
		{
			var suit = GetSuit();
			var rank = _randomShuffler.Next(_rankSymbols.Count());
			return new Card
			{
				Suit = suit,
				SuitSymbol = GetSuitSymbol(suit),
				Rank = rank,
				RankSymbol = _rankSymbols.ElementAt(rank - 1)
			};
		}

		public IEnumerable<Card> GetAllCards() => Enum.GetValues<Suit>()
			.SelectMany(suit => Enumerable.Range(1, 13).Select(rank => new Card()
			{
				Suit = suit,
				SuitSymbol = GetSuitSymbol(suit),
				Rank = rank,
				RankSymbol = _rankSymbols.ElementAt(rank - 1),
			}));

		private static Suit GetSuit() => Enum.GetValues<Suit>().ElementAt(_randomShuffler.Next(Enum.GetValues<Suit>().Count()));

		private static string GetSuitSymbol(Suit suit) => _suitSymbols[suit];
	}
}
