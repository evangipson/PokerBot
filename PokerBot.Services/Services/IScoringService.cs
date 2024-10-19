using PokerBot.Domain.Models;

namespace PokerBot.Logic.Services
{
	public interface IScoringService
	{
		HandScore ScoreHand(IEnumerable<Card> hand);
	}
}
