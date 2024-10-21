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

		public bool CardsScored => HandScore?.Hand?.Cards?.Count > 0;

		public string HandClass => CardsScored
			? "poker-bot__hand poker-bot__hand--scored"
			: "poker-bot__hand";

		public string? HandScoreDisplay => ScoringService?.GetHandScoreDisplay(HandScore);

		public string NextCardButtonText => Cards?.Count switch
		{
			0 or null => "Draw new hand",
			<= 3 => "Show the flop",
			<= 5 => "Show the river",
			<= 6 => "Show the turn",
			_ => HandScore == default ? "Get hand score" : "Draw new hand",
		};

		public Action OnNextCard => Cards?.Count switch
		{
			0 or null => ShowNewHand,
			<= 3 => ShowFlop,
			<= 5 => ShowRiver,
			<= 6 => ShowTurn,
			_ => HandScore == default ? ScoreHand : ShowNewHand,
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
