using Microsoft.AspNetCore.Components;

using PokerBot.Domain.Models;
using PokerBot.Logic.Factories;
using PokerBot.Logic.Services;

namespace PokerBot.Api.ViewModels
{
	public partial class Index
	{
		[Inject]
		private IScoringService? ScoringService { get; set; }

		[Inject]
		private IDeckFactory? DeckFactory { get; set; }

		[Inject]
		private IHandService? HandService { get; set; }

		public List<Domain.Models.Card>? Cards;

		public HandScore? HandScore;

		public Dictionary<Score, string> HandScoreDisplay = new()
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

		public Dictionary<int, string> CardRankDisplay = new()
		{
			[1] = "Ace",
			[2] = "Two",
			[3] = "Three",
			[4] = "Four",
			[5] = "Five",
			[6] = "Sixe",
			[7] = "Seven",
			[8] = "Eight",
			[9] = "Nine",
			[10] = "Ten",
			[11] = "Jack",
			[12] = "Queen",
			[13] = "King",
		};

		protected override void OnAfterRender(bool firstRender)
		{
			//if (firstRender)
			//{
			//	ShowNewHand();
			//	StateHasChanged();
			//}
		}

		public void ShowFlop() => Cards?.AddRange([.. HandService!.GetFlop()]);

		public void ShowRiver() => Cards?.Add(HandService!.GetRiver());

		public void ShowTurn() => Cards?.Add(HandService!.GetTurn());

		public void ScoreHand()
		{
			HandScore = ScoringService!.ScoreHand(Cards!);
		}

		public void ShowNewHand()
		{
			HandScore = new();
			DeckFactory!.ShuffleDeck();
			Cards = HandService?.GetHand().Cards.ToList();
		}

		public string? GetHandScoreDisplay()
		{
			if(HandScore!.Score == Score.Pair)
			{
				return $"{HandScoreDisplay[HandScore!.Score]} of {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]}s";
			}
			if(HandScore!.Score == Score.TwoPair)
			{
				return $"{HandScoreDisplay[HandScore!.Score]}, {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]}s and {CardRankDisplay[HandScore.Hand!.Cards.Last().Rank]}s";
			}
			if (HandScore!.Score == Score.ThreeOfAKind)
			{
				return $"{HandScoreDisplay[HandScore!.Score]}, {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]}s";
			}
			if (HandScore!.Score == Score.FullHouse)
			{
				return $"{HandScoreDisplay[HandScore!.Score]}, {CardRankDisplay[HandScore.Hand!.Cards.Last().Rank]}s full of {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]}s";
			}
			if (HandScore!.Score == Score.Straight)
			{
				return $"{HandScoreDisplay[HandScore!.Score]}, {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]} to {CardRankDisplay[HandScore.Hand!.Cards.Last().Rank]}";
			}
			if (HandScore!.Score == Score.Flush)
			{
				return $"{HandScoreDisplay[HandScore!.Score]} of {Suit.GetName(HandScore.Hand!.Cards.First().Suit)}";
			}
			if (HandScore!.Score == Score.StraightFlush)
			{
				return $"{HandScoreDisplay[HandScore!.Score]}, {Suit.GetName(HandScore.Hand!.Cards.First().Suit)} with {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]} to {CardRankDisplay[HandScore.Hand!.Cards.Last().Rank]}";
			}
			if (HandScore!.Score == Score.RoyalFlush)
			{
				return $"{HandScoreDisplay[HandScore!.Score]} of {Suit.GetName(HandScore.Hand!.Cards.First().Suit)}";
			}

			return $"{HandScoreDisplay[HandScore!.Score]}, {CardRankDisplay[HandScore.Hand!.Cards.First().Rank]} of {HandScore.Hand!.Cards.First().Suit}";
		}
	}
}
