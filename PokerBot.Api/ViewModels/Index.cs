using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
		private Domain.Models.Card? _draggedCard;

		protected HandScore? HandScore;

		protected bool CardsScored => HandScore?.Hand?.Cards?.Count > 0;

		protected string HandClass => CardsScored
			? "poker-bot__hand poker-bot__hand--scored"
			: "poker-bot__hand";

		protected string? HandScoreDisplay => ScoringService?.GetHandScoreDisplay(HandScore);

		protected List<Domain.Models.Card> Cards
		{
			get => _cards ??= [];
			set => _cards = value;
		}

		protected List<Domain.Models.Card> PlayerCards => [..Cards.Take(2)];

		protected List<Domain.Models.Card> TableCards => [..Cards.Skip(2)];

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
			< 4 => ShowFlop,
			< 6 => ShowRiver,
			< 7 => ShowTurn,
			_ => HandScore == default ? ScoreHand : ShowNewHand,
		};

		protected void ShowFlop() => Cards.AddRange([.. HandService!.GetFlop()]);

		protected void ShowRiver() => Cards.Add(HandService!.GetRiver());

		protected void ShowTurn() => Cards.Add(HandService!.GetTurn());

		protected void ScoreHand() => HandScore = ScoringService!.ScoreHand(Cards);

		protected void ShowNewHand()
		{
			HandScore = null;
			DeckFactory!.ShuffleDeck();
			Cards = [.. HandService!.GetHand().Cards];
		}

		protected void OnCardDrag(Domain.Models.Card card) => _draggedCard = card;

		protected void OnPlayerCardDragEnd(DragEventArgs dragEventArgs) => OnCardDragEnd(dragEventArgs, 0, 1);

		protected void OnTableCardDragEnd(DragEventArgs dragEventArgs) => OnCardDragEnd(dragEventArgs, 2, Cards.Count);

		private void OnCardDragEnd(DragEventArgs dragEventArgs, int minIndex, int maxIndex)
		{
			if (_draggedCard == null)
			{
				return;
			}

			var oldCardIndex = Cards.FindIndex(card => card.Rank == _draggedCard.Rank && card.Suit == _draggedCard.Suit);
			var newCardIndex = dragEventArgs.OffsetX > 0
				? oldCardIndex + (int)(dragEventArgs.OffsetX / 150)
				: oldCardIndex + (int)(Math.Abs(dragEventArgs.OffsetX - 100) / 150) * -1;
			Cards.Remove(_draggedCard);
			Cards.Insert(Math.Clamp(newCardIndex, minIndex, maxIndex), _draggedCard);
			_draggedCard = null;
		}
	}
}
