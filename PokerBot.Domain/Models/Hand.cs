namespace PokerBot.Domain.Models
{
	public class Hand
	{
		private IList<Card>? _cards;

		public IList<Card> Cards
		{
			get => _cards ??= [];
		}
	}
}
