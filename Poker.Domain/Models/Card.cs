namespace PokerBot.Domain.Models
{
	public class Card
	{
		public Suit Suit { get; set; }

		public string SuitSymbol { get; set; } = string.Empty;

		public string Rank { get; set; } = string.Empty;

		public override string ToString() => $"{Rank.First()}{SuitSymbol}";
	}
}
