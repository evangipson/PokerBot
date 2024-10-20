namespace PokerBot.Domain.Models
{
	public class Card
	{
		public Suit Suit { get; set; }

		public int Rank { get; set; }

		public string SuitSymbol { get; set; } = string.Empty;

		public string RankSymbol { get; set; } = string.Empty;

		public string RankDisplay => RankSymbol == "10" ? RankSymbol : RankSymbol.First().ToString();

		public override string ToString() => $"{RankDisplay}{SuitSymbol}";
	}
}
