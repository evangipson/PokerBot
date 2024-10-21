using Microsoft.Extensions.Logging.Abstractions;
using PokerBot.Domain.Models;
using PokerBot.Logic.Factories;
using PokerBot.Logic.Services;

namespace PokerBot.Tests.Services
{
	public class ScoringServiceTests
	{
		private readonly IScoringService _scoringService = new ScoringService(NullLogger<ScoringService>.Instance);
		private readonly ICardFactory _cardFactory = new CardFactory(NullLogger<CardFactory>.Instance);

		private IEnumerable<Card> _allCards => _cardFactory.GetAllCards();

		[Fact]
		public void ScoreHand_ShouldScoreRoyalFlush_WhenProvidedRoyalFlush()
		{
			Card aceOfClubs = _allCards.First(card => card.Rank == 1 && card.Suit == Suit.Clubs);
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfClubs = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Clubs);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			IEnumerable<Card> royalFlushHand = [ aceOfClubs, kingOfClubs, queenOfClubs, jackOfClubs, tenOfClubs ];

			var result = _scoringService.ScoreHand(royalFlushHand);

			Assert.Equal(Score.RoyalFlush, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreRoyalFlush_WhenProvidedMultipleStraightsWithRoyalFlush()
		{
			Card aceOfClubs = _allCards.First(card => card.Rank == 1 && card.Suit == Suit.Clubs);
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card tenOfSpades = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Spades);
			Card nineOfHearts = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Hearts);
			Card queenOfClubs = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Clubs);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			Card queenOfSpades = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Spades);
			Card jackOfDiamonds = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Diamonds);
			Card eightOfSpades = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Spades);
			IEnumerable<Card> multipleRoyalFlushHand = [aceOfClubs, kingOfClubs, queenOfClubs, jackOfClubs, jackOfDiamonds, tenOfClubs, nineOfHearts, queenOfSpades, eightOfSpades, tenOfSpades];

			var result = _scoringService.ScoreHand(multipleRoyalFlushHand);

			Assert.Equal(Score.RoyalFlush, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreRoyalFlush_WhenProvidedMultipleRoyalFlushes()
		{
			Card aceOfClubs = _allCards.First(card => card.Rank == 1 && card.Suit == Suit.Clubs);
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfClubs = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Clubs);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			Card aceOfSpades = _allCards.First(card => card.Rank == 1 && card.Suit == Suit.Spades);
			Card kingOfSpades = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Spades);
			Card queenOfSpades = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Spades);
			Card jackOfSpades = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Spades);
			Card tenOfSpades = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Spades);
			IEnumerable<Card> multipleRoyalFlushHand = [aceOfClubs, kingOfClubs, queenOfClubs, jackOfClubs, tenOfClubs, aceOfSpades, kingOfSpades, queenOfSpades, jackOfSpades, tenOfSpades];

			var result = _scoringService.ScoreHand(multipleRoyalFlushHand);

			Assert.Equal(Score.RoyalFlush, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldNotScoreRoyalFlush_WhenProvidedDifferentSuits()
		{
			Card aceOfClubs = _allCards.First(card => card.Rank == 1 && card.Suit == Suit.Clubs);
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfSpades = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Spades);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			IEnumerable<Card> almostRoyalFlushHand = [aceOfClubs, kingOfClubs, queenOfSpades, jackOfClubs, tenOfClubs];

			var result = _scoringService.ScoreHand(almostRoyalFlushHand);

			Assert.Equal(Score.Straight, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreStraightFlush_WhenProvidedStraightFlush()
		{
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfClubs = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Clubs);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			Card nineOfClubs = _allCards.First(card => card.Rank == 9 && card.Suit == Suit.Clubs);
			IEnumerable<Card> straightFlushHand = [ kingOfClubs, queenOfClubs, jackOfClubs, tenOfClubs, nineOfClubs ];

			var result = _scoringService.ScoreHand(straightFlushHand);

			Assert.Equal(Score.StraightFlush, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreFourOfAKind_WhenProvidedFourOfAKind()
		{
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card kingOfHearts = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Hearts);
			Card kingOfSpades = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Spades);
			Card kingOfDiamonds = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Diamonds);
			Card sevenOfClubs = _allCards.First(card => card.Rank == 7 && card.Suit == Suit.Clubs);
			IEnumerable<Card> fourOfAKindHand = [kingOfClubs, kingOfHearts, kingOfSpades, kingOfDiamonds, sevenOfClubs];

			var result = _scoringService.ScoreHand(fourOfAKindHand);

			Assert.Equal(Score.FourOfAKind, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreStraight_WhenProvidedStraight()
		{
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfSpades = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Spades);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			Card nineOfClubs = _allCards.First(card => card.Rank == 9 && card.Suit == Suit.Clubs);
			IEnumerable<Card> straightHand = [kingOfClubs, queenOfSpades, jackOfClubs, tenOfClubs, nineOfClubs];

			var result = _scoringService.ScoreHand(straightHand);

			Assert.Equal(Score.Straight, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreStraight_WhenProvidedMultipleStraights()
		{
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);
			Card queenOfSpades = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Spades);
			Card jackOfClubs = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Clubs);
			Card tenOfClubs = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Clubs);
			Card nineOfClubs = _allCards.First(card => card.Rank == 9 && card.Suit == Suit.Clubs);
			Card kingOfSpades = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Spades);
			Card queenOfDiamonds = _allCards.First(card => card.Rank == 12 && card.Suit == Suit.Diamonds);
			Card jackOfHearts = _allCards.First(card => card.Rank == 11 && card.Suit == Suit.Hearts);
			Card tenOfDiamonds = _allCards.First(card => card.Rank == 10 && card.Suit == Suit.Diamonds);
			Card nineOfSpades = _allCards.First(card => card.Rank == 9 && card.Suit == Suit.Spades);
			IEnumerable<Card> multipleStraightsHand = [kingOfClubs, queenOfSpades, jackOfClubs, tenOfClubs, nineOfClubs, kingOfSpades, queenOfDiamonds, jackOfHearts, tenOfDiamonds, nineOfSpades];

			var result = _scoringService.ScoreHand(multipleStraightsHand);

			Assert.Equal(Score.Straight, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreStraightFlush_WhenProvidedFourOfAKindAndLowStraightFlush()
		{
			Card sixOfClubs = _allCards.First(card => card.Rank == 6 && card.Suit == Suit.Clubs);
			Card twoOfClubs = _allCards.First(card => card.Rank == 2 && card.Suit == Suit.Clubs);
			Card threeOfClubs = _allCards.First(card => card.Rank == 3 && card.Suit == Suit.Clubs);
			Card sixOfDiamonds = _allCards.First(card => card.Rank == 6 && card.Suit == Suit.Diamonds);
			Card fourOfClubs = _allCards.First(card => card.Rank == 4 && card.Suit == Suit.Clubs);
			Card fiveOfClubs = _allCards.First(card => card.Rank == 5 && card.Suit == Suit.Clubs);
			Card sixOfSpades = _allCards.First(card => card.Rank == 6 && card.Suit == Suit.Spades);
			Card sixOfHearts = _allCards.First(card => card.Rank == 6 && card.Suit == Suit.Hearts);
			IEnumerable<Card> straightFlushHand = [sixOfClubs, twoOfClubs, threeOfClubs, sixOfDiamonds, fourOfClubs, fiveOfClubs, sixOfSpades, sixOfHearts];

			var result = _scoringService.ScoreHand(straightFlushHand);

			Assert.Equal(Score.StraightFlush, result.Score);
		}

		[Fact]
		public void ScoreHand_ShouldScoreFourOfAKind_WhenProvidedStraightAndFlushAndFourOfAKind()
		{
			Card sixOfClubs = _allCards.First(card => card.Rank == 6 && card.Suit == Suit.Clubs);
			Card twoOfClubs = _allCards.First(card => card.Rank == 2 && card.Suit == Suit.Clubs);
			Card threeOfClubs = _allCards.First(card => card.Rank == 3 && card.Suit == Suit.Clubs);
			Card eightOfClubs = _allCards.First(card => card.Rank == 8 && card.Suit == Suit.Clubs);
			Card kingOfClubs = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Clubs);

			Card fourOfDiamonds = _allCards.First(card => card.Rank == 4 && card.Suit == Suit.Diamonds);
			Card fiveOfHearts = _allCards.First(card => card.Rank == 5 && card.Suit == Suit.Hearts);

			Card kingOfDiamonds = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Diamonds);
			Card kingOfHearts = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Hearts);
			Card kingOfSpades = _allCards.First(card => card.Rank == 13 && card.Suit == Suit.Spades);

			IEnumerable<Card> fourOfAKindHand = [sixOfClubs, twoOfClubs, threeOfClubs, eightOfClubs, kingOfClubs, fourOfDiamonds, fiveOfHearts, kingOfDiamonds, kingOfHearts, kingOfSpades];

			var result = _scoringService.ScoreHand(fourOfAKindHand);

			Assert.Equal(Score.FourOfAKind, result.Score);
		}
	}
}
