using Microsoft.Extensions.Logging;

using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services
{
	/// <inheritdoc cref="IScoringService"/>
	public class ScoringService(ILogger<ScoringService> logger) : IScoringService
	{
		private static readonly Dictionary<int, string> _cardRankDisplay = new()
		{
			[1] = "Ace",
			[2] = "Two",
			[3] = "Three",
			[4] = "Four",
			[5] = "Five",
			[6] = "Six",
			[7] = "Seven",
			[8] = "Eight",
			[9] = "Nine",
			[10] = "Ten",
			[11] = "Jack",
			[12] = "Queen",
			[13] = "King",
		};
		private static readonly Dictionary<Score, string> _handScoreDisplay = new()
		{
			[Score.HighCard] = "High card",
			[Score.Pair] = "Pair",
			[Score.TwoPair] = "Two pair",
			[Score.ThreeOfAKind] = "Three of a kind",
			[Score.Straight] = "Straight",
			[Score.Flush] = "Flush",
			[Score.FullHouse] = "Full house",
			[Score.FourOfAKind] = "Four of a kind",
			[Score.StraightFlush] = "Straight flush",
			[Score.RoyalFlush] = "Royal flush",
		};

		public HandScore ScoreHand(IEnumerable<Card> hand)
		{
			var handWithFlush = HandHasFlush(hand);
			var handWithStraight = HandHasStraight(hand);
			var fourOfAKinds = HandHasMultiples(hand, 4);
			IEnumerable<Card> threeOfAKinds = [];
			IEnumerable<Card> pairs = [];
			if (fourOfAKinds.Count == 0)
			{
				threeOfAKinds = HandHasMultiples(hand, 3);
			}
			if (!threeOfAKinds.Any())
			{
				pairs = HandHasMultiples(hand, 2);
			}

			// straight flush evaluation
			var handHasStraightAndFlush = handWithFlush.Count != 0 && handWithStraight.Count != 0;
			if (handHasStraightAndFlush && handWithStraight.All(card => handWithFlush.Any(flushCard => card.Rank == flushCard.Rank)))
			{
				// royal flush evaluation
				if (handWithStraight.First().Rank == 1 && handWithStraight.Last().Rank == 10)
				{
					logger.LogInformation($"{nameof(ScoreHand)} info: Found royal flush in hand: {string.Join(", ", handWithStraight)}");
					return new()
					{
						Hand = new()
						{
							Cards = handWithStraight
						},
						Score = Score.RoyalFlush,
						ScoreRank = 10
					};
				}

				logger.LogInformation($"{nameof(ScoreHand)} info: Found straight flush in hand: {string.Join(", ", handWithStraight)}");
				return new()
				{
					Hand = new()
					{
						Cards = handWithFlush
					},
					Score = Score.StraightFlush,
					ScoreRank = 9
				};
			}

			// four of a kind evaluation
			if (fourOfAKinds.Count != 0)
			{
				logger.LogInformation($"{nameof(ScoreHand)} info: Found four-of-a-kind in hand: {string.Join(", ", fourOfAKinds.OrderByDescending(card => card.Rank).Take(4))}");
				return new()
				{
					Hand = new()
					{
						Cards = [..fourOfAKinds.OrderByDescending(card => card.Rank).Take(4)]
					},
					Score = Score.FourOfAKind,
					ScoreRank = 8
				};
			}

			// full house evaluation
			if (threeOfAKinds.Any())
			{
				IEnumerable<Card> fullHousePairs = HandHasMultiples(hand.Where(card => !threeOfAKinds.Contains(card)), 2);
				if (fullHousePairs.Any())
				{
					var fullHouse = threeOfAKinds.OrderByDescending(card => card.Rank).Take(3).ToList();
					fullHouse.AddRange(fullHousePairs.OrderByDescending(card => card.Rank).Take(2));
					logger.LogInformation($"{nameof(ScoreHand)} info: Found full house in hand: {string.Join(", ", fullHouse)}");
					return new()
					{
						Hand = new()
						{
							Cards = [..fullHouse]
						},
						Score = Score.FullHouse,
						ScoreRank = 7
					};
				}

				logger.LogInformation($"{nameof(ScoreHand)} info: Found three-of-a-kind in hand: {string.Join(", ", threeOfAKinds)}");
				return new()
				{
					Hand = new()
					{
						Cards = threeOfAKinds.ToList()
					},
					Score = Score.ThreeOfAKind,
					ScoreRank = 6
				};
			}

			// flush evaluation
			if (handWithFlush.Count != 0)
			{
				logger.LogInformation($"{nameof(ScoreHand)} info: Found flush in hand: {string.Join(", ", handWithFlush.OrderByDescending(card => card.Rank).Take(5))}");
				return new()
				{
					Hand = new()
					{
						Cards = [.. handWithFlush.OrderByDescending(card => card.Rank).Take(5)]
					},
					Score = Score.Flush,
					ScoreRank = 5
				};
			}

			// straight evaluation
			if (handWithStraight.Count != 0)
			{
				logger.LogInformation($"{nameof(ScoreHand)} info: Found straight in hand: {string.Join(", ", handWithStraight.Take(5))}");
				return new()
				{
					Hand = new()
					{
						Cards = [.. handWithStraight.Take(5)]
					},
					Score = Score.Straight,
					ScoreRank = 4
				};
			}

			// two pair evaluation
			if (pairs.Count() > 2)
			{
				logger.LogInformation($"{nameof(ScoreHand)} info: Found two pair in hand: {string.Join(", ", pairs.OrderByDescending(card => card.Rank).Take(4))}");
				return new()
				{
					Hand = new()
					{
						Cards = [.. pairs.OrderByDescending(card => card.Rank).Take(4)]
					},
					Score = Score.TwoPair,
					ScoreRank = 3
				};
			}

			// pair evaluation
			if (pairs.Count() == 2)
			{
				logger.LogInformation($"{nameof(ScoreHand)} info: Found pair in hand: {string.Join(", ", pairs.OrderByDescending(card => card.Rank).Take(4))}");
				return new()
				{
					Hand = new()
					{
						Cards = [.. pairs.OrderByDescending(card => card.Rank).Take(4)]
					},
					Score = Score.Pair,
					ScoreRank = 2
				};
			}

			// high card evaluation
			List<Card> highCard = hand.Where(card => card.Rank == 1).Any()
				? [.. hand.OrderBy(card => card.Rank).Take(1)]
				: [.. hand.OrderByDescending(card => card.Rank).Take(1)];
			logger.LogInformation($"{nameof(ScoreHand)} info: Found high card in hand: {highCard}");
			return new()
			{
				Hand = new()
				{
					Cards = highCard
				},
				Score = Score.HighCard,
				ScoreRank = 1
			};
		}

		public string GetHandScoreDisplay(HandScore? scoredHand = null) => scoredHand?.Score switch
		{
			Score.Pair => $"{_handScoreDisplay[scoredHand.Score]} of {_cardRankDisplay[scoredHand.Hand!.Cards.First().Rank]}s",
			Score.TwoPair => $"{_handScoreDisplay[scoredHand.Score]}, {_cardRankDisplay[scoredHand.Hand!.Cards.First().Rank]}s and {_cardRankDisplay[scoredHand.Hand.Cards.Last().Rank]}s",
			Score.ThreeOfAKind => $"{_handScoreDisplay[scoredHand.Score]}, {_cardRankDisplay[scoredHand.Hand!.Cards.First().Rank]}s",
			Score.FullHouse => $"{_handScoreDisplay[scoredHand.Score]}, {_cardRankDisplay[scoredHand.Hand!.Cards.Last().Rank]}s full of {_cardRankDisplay[scoredHand.Hand.Cards.First().Rank]}s",
			Score.Straight => $"{_handScoreDisplay[scoredHand.Score]}, {_cardRankDisplay[scoredHand.Hand!.Cards.Last().Rank]} to {_cardRankDisplay[scoredHand.Hand.Cards.First().Rank]}",
			Score.Flush => $"{_handScoreDisplay[scoredHand.Score]} of {Suit.GetName(scoredHand.Hand!.Cards.First().Suit)}",
			Score.StraightFlush => $"{_handScoreDisplay[scoredHand.Score]}, {Suit.GetName(scoredHand.Hand!.Cards.First().Suit)} with {_cardRankDisplay[scoredHand.Hand.Cards.First().Rank]} to {_cardRankDisplay[scoredHand.Hand.Cards.Last().Rank]}",
			Score.RoyalFlush => $"{_handScoreDisplay[scoredHand.Score]} of {Suit.GetName(scoredHand.Hand!.Cards.First().Suit)}",
			Score.HighCard => $"{_handScoreDisplay[scoredHand.Score]}, {_cardRankDisplay[scoredHand.Hand!.Cards.First().Rank]} of {scoredHand.Hand.Cards.First().Suit}",
			_ or null => string.Empty
		};

		private List<Card> HandHasMultiples(IEnumerable<Card> hand, int multipleAmount)
		{
			var handGroupedByRank = hand.GroupBy(card => card.Rank);
			string logMessage = $"{nameof(ScoreHand)} info: Found {multipleAmount}-of-a-kind in hand:";
			if (multipleAmount == 2)
			{
				logMessage = $"{nameof(ScoreHand)} info: Found pair in hand:";
			}

			List<Card> multipleHands = [];
			foreach (var rankGroup in handGroupedByRank.Where(group => group.Count() >= multipleAmount))
			{
				//_logger.LogInformation($"{logMessage} {string.Join(", ", rankGroup)}");
				multipleHands.AddRange(rankGroup);
			}

			return multipleHands;
		}

		private static List<Card> HandHasStraight(IEnumerable<Card> hand)
		{
			var orderedSingleRankedCards = hand.OrderByDescending(card => card.Rank)
				.GroupBy(card => card.Rank)
				.SelectMany(grouping => grouping.Take(1))
				.ToList();

			// handle wrapped straights (i.e.: 10 - A)
			var handContainsAce = orderedSingleRankedCards.Any(card => card.Rank == 1);
			var handContainsAllHighestRanks = orderedSingleRankedCards.Where(card => card.Rank >= 10).Count() == 4;
			if(handContainsAce && handContainsAllHighestRanks)
			{
				var newHandOrder = hand.OrderByDescending(card => card.Rank)
					.GroupBy(card => card.Rank)
					.SelectMany(grouping => grouping.Take(1));
				var highestRanks = newHandOrder.Take(4);
				if(highestRanks.First().Rank - highestRanks.Last().Rank == 3)
				{
					var ace = newHandOrder.TakeLast(1).Single();
					return [ ace, .. highestRanks ];
				}
			}

			// handle sequential straights
			for (var i = 0; i < orderedSingleRankedCards.Count - 4; i++)
			{
				IEnumerable<Card> sliceOfOrderedRanks = [.. orderedSingleRankedCards.Take(new Range(i, i + 5))];
				if (sliceOfOrderedRanks.First().Rank - sliceOfOrderedRanks.Last().Rank == 4)
				{
					return sliceOfOrderedRanks.ToList();
				}
			}

			return [];
		}

		private static List<Card> HandHasFlush(IEnumerable<Card> hand)
		{
			var groupedHand = hand.OrderByDescending(card => card.Rank).GroupBy(card => card.Suit);
			foreach (var suitCollection in groupedHand)
			{
				if (suitCollection.Count() >= 5)
				{
					//_logger.LogInformation($"{nameof(ScoreHand)} info: Found a flush in hand: {string.Join(", ", suitCollection)}");
					return [.. suitCollection];
				}
			}
			return [];
		}
	}
}
