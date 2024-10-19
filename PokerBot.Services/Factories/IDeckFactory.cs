using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories
{
    /// <summary>
    /// A factory to create a deck of <see cref="Card"/>.
    /// </summary>
    public interface IDeckFactory
    {
        /// <summary>
        /// Draws a <see cref="Card"/> from the deck.
        /// </summary>
        /// <returns>
        /// A <see cref="Card"/> from the deck.
        /// </returns>
        Card DrawCard();

        public void ShuffleDeck();
	}
}
