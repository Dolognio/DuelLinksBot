using System;

namespace DuelLinksBot.Utilities.Internal.Game
{
	public class DuelResult
	{
		public DuelResult(DuelType duelType, TimeSpan duelDuration, bool duelWon)
		{
			DuelType = duelType;
			DuelDuration = duelDuration;
			DuelWon = duelWon;
		}

		public DuelType DuelType { get; }
		public TimeSpan DuelDuration { get; }
		public bool DuelWon { get; }
	}
}