using Microsoft.AspNetCore.Components.Web;
using PokerBot.Domain.Models;

namespace PokerBot.Api.Services
{
	/// <summary>
	/// A service that abstracts logic related to dragging elements.
	/// </summary>
	public interface IDraggableService
	{
		/// <summary>
		/// Remembers the <paramref name="card"/> that is being dragged.
		/// </summary>
		/// <param name="card">
		/// The <see cref="Card"/> that is being dragged.
		/// </param>
		void OnCardDrag(Card card);

		/// <summary>
		/// Places the player <see cref="Card"/> that's being dragged into it's new position.
		/// </summary>
		/// <param name="dragEventArgs">
		/// The drag event.
		/// </param>
		/// <param name="cards">
		/// The list of all cards on the screen.
		/// </param>
		void OnPlayerCardDragEnd(DragEventArgs dragEventArgs, List<Card> cards);

		/// <summary>
		/// Places the table <see cref="Card"/> that's being dragged into it's new position.
		/// </summary>
		/// <param name="dragEventArgs">
		/// The drag event.
		/// </param>
		/// <param name="cards">
		/// The list of all cards on the screen.
		/// </param>
		void OnTableCardDragEnd(DragEventArgs dragEventArgs, List<Card> cards);
	}
}
