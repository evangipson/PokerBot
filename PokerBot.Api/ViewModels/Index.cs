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

		public List<Card>? Cards;

		public int HandScore;

		private static readonly Dictionary<int, string> HandScoreToHand = new()
		{
			[0] = "No cards?",
			[1] = "High Card",
			[2] = "Pair",
			[3] = "Two Pair",
			[4] = "Straight",
			[5] = "Flush",
			[6] = "Three Of A Kind",
			[7] = "Full House",
			[8] = "Four Of A Kind",
			[9] = "Straight Flush",
			[10] = "Royal Flush"
		};

		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender)
			{
				ShowNewHand();
				StateHasChanged();
			}
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
			HandScore = 0;
			DeckFactory!.ShuffleDeck();
			Cards = HandService?.GetHand().Cards.ToList();
		}
	}
}
