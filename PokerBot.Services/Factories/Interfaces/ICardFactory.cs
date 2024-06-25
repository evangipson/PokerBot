using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories.Interfaces
{
	/// <summary>
	/// A factory for making a <see cref="Card"/>.
	/// </summary>
	public interface ICardFactory
	{
		/// <summary>
		/// Makes one card of a random <see cref="Suit"/>
		/// and rank.
		/// </summary>
		/// <returns>
		/// One card of a random <see cref="Suit"/> and rank.
		/// </returns>
		Card MakeCard();

		/// <summary>
		/// Gets all possible combination of <see cref="Card"/>.
		/// </summary>
		/// <returns>
		/// All combination of <see cref="Suit"/> and rank as
		/// a collection of <see cref="Card"/>.
		/// </returns>
		IEnumerable<Card> GetAllCards();
	}
}
