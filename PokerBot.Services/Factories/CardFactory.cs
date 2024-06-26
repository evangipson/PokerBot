﻿using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Factories.Interfaces;

namespace PokerBot.Logic.Factories
{
	/// <inheritdoc cref="ICardFactory" />
	[Service(typeof(ICardFactory))]
	public class CardFactory : ICardFactory
	{
		private readonly ILogger<CardFactory> _logger;
		private readonly IEnumerable<string> _rankSymbols;
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
			_rankSymbols = Enumerable.Range(1, 13).Select(x =>
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
			var rank = _randomShuffler.Next(_rankSymbols.Count());
			return new Card
			{
				Suit = suit,
				SuitSymbol = GetSuitSymbol(suit),
				Rank = rank,
				RankSymbol = _rankSymbols.ElementAt(rank - 1)
			};
		}

		public IEnumerable<Card> GetAllCards()
		{
			var allCards = new List<Card>();
			foreach (var suit in Enum.GetValues<Suit>())
			{
				foreach (var rank in Enumerable.Range(1, 13))
				{
					allCards.Add(new Card
					{
						Suit = suit,
						SuitSymbol = GetSuitSymbol(suit),
						Rank = rank,
						RankSymbol = _rankSymbols.ElementAt(rank - 1),
					});
				}
			}
			return allCards;
		}

		/// <summary>
		/// Gets a random <see cref="Suit"/>, to attach to
		/// a <see cref="Card"/>.
		/// </summary>
		/// <returns>
		/// A random <see cref="Suit"/>.
		/// </returns>
		private Suit GetSuit() => Enum.GetValues<Suit>().ElementAt(_randomShuffler.Next(Enum.GetValues<Suit>().Count()));

		/// <summary>
		/// Gets the symbol for a <see cref="Suit"/>.
		/// </summary>
		/// <param name="suit">
		/// The <see cref="Suit"/> to get the symbol for.
		/// </param>
		/// <returns>
		/// A <see cref="Suit"/> symbol.
		/// </returns>
		private string GetSuitSymbol(Suit suit) => _suitSymbols[suit];
	}
}
