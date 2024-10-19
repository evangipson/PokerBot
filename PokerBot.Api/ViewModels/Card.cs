using Microsoft.AspNetCore.Components;

using PokerBot.Domain.Models;

namespace PokerBot.Api.ViewModels
{
	public partial class Card
	{
		[Parameter]
		public HandScore? HandScore { get; set; }

		[Parameter]
		public Domain.Models.Card? CurrentCard { get; set; }

		private readonly string? _baseCardClass = "poker-bot__card";

		public int SuitDisplayTimes => CurrentCard!.Rank > 10 ? 1 : CurrentCard!.Rank;

		public string? GetCardClass()
		{
			var scoreHighlight = HandScore?.Hand?.Cards.Contains(CurrentCard!) == true
				? $"{_baseCardClass} {_baseCardClass}--highlight"
				: _baseCardClass;

			var suitColor = CurrentCard?.Suit == Suit.Hearts || CurrentCard?.Suit == Suit.Diamonds
				? $"{scoreHighlight} {_baseCardClass}--red"
				: $"{scoreHighlight} {_baseCardClass}--black";

			var rankSymbolLayout = CurrentCard?.Rank <= 3
				? $"{suitColor} {_baseCardClass}--columns"
				: $"{suitColor} {_baseCardClass}--rows";

			var rankSymbolSize = CurrentCard?.Rank >= 7 && CurrentCard?.Rank <= 10
				? $"{rankSymbolLayout} {_baseCardClass}--small"
				: $"{rankSymbolLayout}";

			var singleSymbol = CurrentCard?.Rank == 1 || CurrentCard?.Rank > 10
				? $"{rankSymbolSize} {_baseCardClass}--large"
				: $"{rankSymbolSize}";

			return singleSymbol;
		}

		public string? GetRoyalty()
		{
			if(CurrentCard?.Rank == 13)
			{
				return $"♚";
			}

			if(CurrentCard?.Rank == 12)
			{
				return "♛";
			}

			if(CurrentCard?.Rank == 11)
			{
				return "♝";
			}

			return null;
		}
	}
}
