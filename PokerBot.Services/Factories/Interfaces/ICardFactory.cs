using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories.Interfaces
{
	public interface ICardFactory
	{
		Card MakeCard();

		IEnumerable<Card> GetAllCards();
	}
}
