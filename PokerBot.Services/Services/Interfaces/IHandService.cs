using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services.Interfaces
{
	public interface IHandService
	{
		Hand GetHand();

		Hand GetFlop();
	}
}
