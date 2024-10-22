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

		private List<Domain.Models.Card>? _cards;
		protected List<Domain.Models.Card> Cards
		{
			get => _cards ??= [];
			set => _cards = value;
		}

		protected HandScore? HandScore;

		protected string HandClass => HandScore?.Hand?.Cards?.Count > 0
			? "poker-bot__hand poker-bot__hand--scored"
			: "poker-bot__hand";

		protected string? HandScoreDisplay => ScoringService?.GetHandScoreDisplay(HandScore);

		protected string NextCardButtonText => Cards.Count switch
		{
			0 => "Draw new hand",
			< 4 => "Show the flop",
			< 6 => "Show the river",
			< 7 => "Show the turn",
			_ => HandScore == default ? "Get hand score" : "Draw new hand",
		};

		protected Action OnNextCard => Cards.Count switch
		{
			0 => ShowNewHand,
			< 4 => () => Cards.AddRange([.. HandService!.GetFlop()]),
			< 6 => () => Cards.Add(HandService!.GetRiver()),
			< 7 => () => Cards.Add(HandService!.GetTurn()),
			_ => HandScore == default ? () => HandScore = ScoringService!.ScoreHand(Cards) : ShowNewHand,
		};

		protected void ShowNewHand()
		{
			HandScore = null;
			DeckFactory!.ShuffleDeck();
			Cards = [.. HandService!.GetHand().Cards];
		}
	}
}
