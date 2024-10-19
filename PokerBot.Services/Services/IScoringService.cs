using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services
{
	public interface IScoringService
	{
		int ScoreHand(IEnumerable<Card> hand);
	}
}
