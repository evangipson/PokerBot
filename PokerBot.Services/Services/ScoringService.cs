using Microsoft.Extensions.Logging;

using PokerBot.Base.DependencyInjection;
using PokerBot.Domain.Models;
using PokerBot.Logic.Services.Interfaces;

namespace PokerBot.Logic.Services
{
	[Service(typeof(IScoringService))]
	public class ScoringService : IScoringService
	{
		private readonly ILogger<ScoringService> _logger;
		internal enum ScoreOrder
		{
			HighCard,
			Pair,
			TwoPair,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraightFlush,
			RoyalFlush
		}

		public ScoringService(ILogger<ScoringService> logger)
		{
			_logger = logger;
		}

		public int ScoreHand(IEnumerable<Card> hand)
		{
			// evaluate hand by finding flushes, straights, and multiples
			var handWithFlush = HandHasFlush(hand);
			var handWithStraight = HandHasStraight(hand);
			var fourOfAKinds = HandHasMultiples(hand, 4);
			IEnumerable<Card> threeOfAKinds = new List<Card>();
			IEnumerable<Card> pairs = new List<Card>();
			if (!fourOfAKinds.Any())
			{
				threeOfAKinds = HandHasMultiples(hand, 3);
			}
			if (!threeOfAKinds.Any())
			{
				pairs = HandHasMultiples(hand, 2);
			}

			// straight flush evaluation
			if (handWithFlush.Any() && handWithStraight.Any())
			{
				if (handWithFlush.All(card => handWithStraight.Contains(card)))
				{
					// royal flush evaluation
					if (handWithStraight.OrderByDescending(card => card.Rank).Last().Rank == 13 && handWithStraight.Any(card => card.Rank == 1))
					{
						_logger.LogInformation($"{nameof(ScoreHand)} info: Found royal flush in hand: {string.Join(", ", handWithStraight)}");
						return 0;
					}

					_logger.LogInformation($"{nameof(ScoreHand)} info: Found straight flush in hand: {string.Join(", ", handWithStraight)}");
					return 0;
				}
			}

			// four of a kind evaluation
			if (fourOfAKinds.Any())
			{
				_logger.LogInformation($"{nameof(ScoreHand)} info: Found four-of-a-kind in hand: {string.Join(", ", fourOfAKinds.OrderByDescending(card => card.Rank).Take(4))}");
				return 0;
			}

			// full house evaluation
			if (threeOfAKinds.Any())
			{
				IEnumerable<Card> fullHousePairs = HandHasMultiples(hand.Where(card => !threeOfAKinds.Contains(card)), 2);
				if (fullHousePairs.Any())
				{
					var fullHouse = threeOfAKinds.OrderByDescending(card => card.Rank).Take(3).ToList();
					fullHouse.AddRange(fullHousePairs.OrderByDescending(card => card.Rank).Take(2));
					_logger.LogInformation($"{nameof(ScoreHand)} info: Found full house in hand: {string.Join(", ", fullHouse)}");
					return 0;
				}

				_logger.LogInformation($"{nameof(ScoreHand)} info: Found four-of-a-kind in hand: {string.Join(", ", threeOfAKinds)}");
				return 0;
			}

			// flush evaluation
			if (handWithFlush.Any())
			{
				_logger.LogInformation($"{nameof(ScoreHand)} info: Found flush in hand: {string.Join(", ", handWithFlush.OrderByDescending(card => card.Rank).Take(5))}");
				return 0;
			}

			// straight evaluation
			if (handWithStraight.Any())
			{
				_logger.LogInformation($"{nameof(ScoreHand)} info: Found straight in hand: {string.Join(", ", handWithStraight.OrderByDescending(card => card.Rank).Take(5))}");
				return 0;
			}

			// two pair evaluation
			if (pairs.Count() > 2)
			{
				_logger.LogInformation($"{nameof(ScoreHand)} info: Found two pair in hand: {string.Join(", ", pairs.OrderByDescending(card => card.Rank).Take(4))}");
				return 0;
			}

			// two pair evaluation
			if (pairs.Count() == 2)
			{
				_logger.LogInformation($"{nameof(ScoreHand)} info: Found pair in hand: {string.Join(", ", pairs.OrderByDescending(card => card.Rank).Take(4))}");
				return 0;
			}

			// high card evaluation
			_logger.LogInformation($"{nameof(ScoreHand)} info: Found high card in hand: {string.Join(", ", hand.OrderByDescending(card => card.Rank).Take(1))}");
			return 0;
		}

		private IEnumerable<Card> HandHasMultiples(IEnumerable<Card> hand, int multipleAmount)
		{
			var handGroupedByRank = hand.GroupBy(card => card.Rank);
			string logMessage = $"{nameof(ScoreHand)} info: Found {multipleAmount}-of-a-kind in hand:";
			if (multipleAmount == 2)
			{
				logMessage = $"{nameof(ScoreHand)} info: Found pair in hand:";
			}

			List<Card> multipleHands = new List<Card>();
			foreach (var rankGroup in handGroupedByRank.Where(group => group.Count() >= multipleAmount))
			{
				//_logger.LogInformation($"{logMessage} {string.Join(", ", rankGroup)}");
				multipleHands.AddRange(rankGroup);
			}

			return multipleHands;
		}

		private IEnumerable<Card> HandHasStraight(IEnumerable<Card> hand)
		{
			var orderedHand = hand.OrderByDescending(card => card.Rank);
			for (var i = 0; i < orderedHand.Count() - 5; i++)
			{
				var skipped = orderedHand.Skip(i);
				var possibleStraight = skipped.Take(5);
				var orderedCardList = possibleStraight.ToList();
				var doubles = orderedCardList.GroupBy(card => card.Rank).Count(group => group.Count() > 1);
				var inARow = orderedCardList[4].Rank - orderedCardList[0].Rank >= 4;

				if (doubles == 0 && inARow)
				{
					//_logger.LogInformation($"{nameof(ScoreHand)} info: Found a straight in hand: {string.Join(", ", orderedCardList)}");
					return orderedCardList;
				}
			}

			return new List<Card>();
		}

		private IEnumerable<Card> HandHasFlush(IEnumerable<Card> hand)
		{
			var groupedHand = hand.OrderByDescending(card => card.Rank).GroupBy(card => card.Suit);
			foreach (var suitCollection in groupedHand)
			{
				if (suitCollection.Count() >= 5)
				{
					//_logger.LogInformation($"{nameof(ScoreHand)} info: Found a flush in hand: {string.Join(", ", suitCollection)}");
					return suitCollection;
				}
			}
			return new List<Card>();
		}
	}
}
