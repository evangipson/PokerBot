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

		public string? GetHandScoreDisplay() => ScoringService!.GetHandScoreDisplay(HandScore!);
	}
}
