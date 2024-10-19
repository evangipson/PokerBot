using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services
{
    /// <summary>
    /// The service for getting a hand of cards.
    /// </summary>
    public interface IHandService
    {
        /// <summary>
        /// Gets a hand of <see cref="Card"/> from
        /// the deck.
        /// </summary>
        /// <returns>
        /// A hand of <see cref="Card"/>.
        /// </returns>
        Hand GetHand();

        /// <summary>
        /// Gets a flop of <see cref="Card"/> from
        /// the deck.
        /// </summary>
        /// <returns>
        /// A flop of <see cref="Card"/>.
        /// </returns>
        IEnumerable<Card> GetFlop();

        Card GetRiver();

        Card GetTurn();
    }
}
