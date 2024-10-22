namespace PokerBot.Domain.Models
{
	public class HandScore
	{
		public Hand? Hand { get; set; }

		public Score Score { get; set; }

		public string? ScoreName => Enum.GetName(Score);

		public int ScoreRank { get; set; }
	}
}
