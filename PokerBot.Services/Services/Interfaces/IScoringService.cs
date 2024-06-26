using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services.Interfaces
{
	public interface IScoringService
	{
		int ScoreHand(IEnumerable<Card> hand);
	}
}
