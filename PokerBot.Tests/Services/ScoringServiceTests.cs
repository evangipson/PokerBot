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
	}
}
