using Microsoft.AspNetCore.Components.Web;
using PokerBot.Domain.Models;

namespace PokerBot.Api.Services
{
	/// <inheritdoc cref="IDraggableService"/>
	public class DraggableService : IDraggableService
	{
		private Card? _draggedCard;

		public void OnCardDrag(Card card) => _draggedCard = card;

		public void OnPlayerCardDragEnd(DragEventArgs dragEventArgs, List<Card> cards) => OnCardDragEnd(dragEventArgs, 0, 1, cards);

		public void OnTableCardDragEnd(DragEventArgs dragEventArgs, List<Card> cards) => OnCardDragEnd(dragEventArgs, 2, cards.Count - 1, cards);

		private void OnCardDragEnd(DragEventArgs dragEventArgs, int minIndex, int maxIndex, List<Card> cards)
		{
			if (_draggedCard == null)
			{
				return;
			}

			var oldCardIndex = cards.FindIndex(card => card.Rank == _draggedCard.Rank && card.Suit == _draggedCard.Suit);
			var newCardIndex = dragEventArgs.OffsetX > 0
				? oldCardIndex + (int)(dragEventArgs.OffsetX / 150)
				: oldCardIndex + (int)(Math.Abs(dragEventArgs.OffsetX - 100) / 150) * -1;
			cards.Remove(_draggedCard);
			cards.Insert(Math.Clamp(newCardIndex, minIndex, maxIndex), _draggedCard);
			_draggedCard = null;
		}
	}
}
