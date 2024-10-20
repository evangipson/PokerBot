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

		public string? HandScoreDisplay => ScoringService?.GetHandScoreDisplay(HandScore);

		public string NextCardButtonText => Cards?.Count switch
		{
			<= 2 => "Show the flop",
			<= 5 => "Show the river",
			<= 6 => "Show the turn",
			<= 7 => HandScore == default ? "Get hand score" : "Draw new hand",
			_ => "Draw new hand"
		};

		public Action OnNextCard => Cards?.Count switch
		{
			<= 2 => ShowFlop,
			<= 5 => ShowRiver,
			<= 6 => ShowTurn,
			<= 7 => HandScore == default ? ScoreHand : ShowNewHand,
			_ => ShowNewHand
		};

		public void ShowFlop() => Cards?.AddRange([.. HandService!.GetFlop()]);

		public void ShowRiver() => Cards?.Add(HandService!.GetRiver());

		public void ShowTurn() => Cards?.Add(HandService!.GetTurn());

		public void ScoreHand()
		{
			HandScore = ScoringService?.ScoreHand(Cards!);
		}

		public void ShowNewHand()
		{
			HandScore = null;
			DeckFactory!.ShuffleDeck();
			Cards = HandService?.GetHand().Cards.ToList();
		}
	}
}
