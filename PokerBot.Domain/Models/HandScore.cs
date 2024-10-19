namespace PokerBot.Domain.Models
{
	public class HandScore
	{
		public Hand? Hand { get; set; }

		public Score Score { get; set; }

		public string? ScoreName => Score.GetName<Score>(Score);

		public int ScoreRank { get; set; }
	}
}
