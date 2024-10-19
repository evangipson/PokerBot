using Microsoft.Extensions.Logging;

using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services
{
	/// <inheritdoc cref="IScoringService"/>
	public class ScoringService(ILogger<ScoringService> logger) : IScoringService
	{
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
			if (handWithFlush.Count != 0 && handWithStraight.Count != 0)
			{
				if (handWithFlush.All(card => handWithStraight.Contains(card)))
				{
					// royal flush evaluation
					if (handWithStraight.OrderByDescending(card => card.Rank).Last().Rank == 13 && handWithStraight.Any(card => card.Rank == 1))
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
				logger.LogInformation($"{nameof(ScoreHand)} info: Found straight in hand: {string.Join(", ", handWithStraight.OrderByDescending(card => card.Rank).Take(5))}");
				return new()
				{
					Hand = new()
					{
						Cards = [.. handWithStraight.OrderByDescending(card => card.Rank).Take(5)]
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
