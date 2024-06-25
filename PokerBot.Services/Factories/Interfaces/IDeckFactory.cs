using PokerBot.Domain.Models;

namespace PokerBot.Logic.Factories.Interfaces
{
	public interface IDeckFactory
	{
		Card DrawCard();
	}
}
