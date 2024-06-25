using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Factories.Interfaces;

namespace PokerBot.Logic.Factories
{
	[Service(typeof(ICardFactory))]
	public class CardFactory : ICardFactory
	{
		private readonly ILogger<CardFactory> _logger;
		private readonly IEnumerable<string> _ranks;
		private static readonly Random _randomShuffler = new Random();
		private static Dictionary<Suit, string> _suitSymbols = new Dictionary<Suit, string>
		{
			[Suit.Clubs] = "♧",
			[Suit.Spades] = "♤",
			[Suit.Hearts] = "♡",
			[Suit.Diamonds] = "♢"
		};

        public CardFactory(ILogger<CardFactory> logger)
        {
            _logger = logger;
            _ranks = Enumerable.Range(1, 13).Select(x =>
			{
				switch (x)
				{
					case 1:
						return "Ace";
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
						return x.ToString();
					case 11:
						return "Jack";
					case 12:
						return "Queen";
					case 13:
						return "King";
					default:
						return "Joker";
				}
			});
		}

		public Card MakeCard()
		{
			var suit = GetSuit();
			return new Card
			{
				Suit = suit,
				SuitSymbol = GetSuitSymbol(suit),
				Rank = _ranks.ElementAtOrDefault(_randomShuffler.Next(_ranks.Count())) ?? string.Empty,
			};
		}

		public IEnumerable<Card> GetAllCards()
		{
			var allCards = new List<Card>();
			foreach(var suit in Enum.GetValues<Suit>())
			{
				foreach(var rank in _ranks)
				{
					allCards.Add(new Card
					{
						Suit = suit,
						SuitSymbol = GetSuitSymbol(suit),
						Rank = rank,
					});
				}
			}
			return allCards;
		}

		private Suit GetSuit() => Enum.GetValues<Suit>().ElementAt(_randomShuffler.Next(Enum.GetValues<Suit>().Count()));

		private string GetSuitSymbol(Suit suit) => _suitSymbols[suit];
	}
}
