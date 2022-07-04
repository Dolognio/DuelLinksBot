using System;
using DuelLinksBot.Utilities.Internal.Game;

namespace DuelLinksBot.Utilities.Internal.Statistics
{
	public class DuelStatisticsType : IClearableStatistics
	{
		public DuelStatisticsType()
		{
			Npc = new DuelStatisticEntry("NPC");
			Vagabond = new DuelStatisticEntry("Vagabond");
			Gate = new DuelStatisticEntry("Gate");
		}

		public DuelStatisticEntry Npc { get; }

		public DuelStatisticEntry Vagabond { get; }

		public DuelStatisticEntry Gate { get; }

		public void ResetStats()
		{
			Npc.ResetStats();
			Vagabond.ResetStats();
			Gate.ResetStats();
		}

		public void AddDuelResult(DuelResult result)
		{
			switch (result.DuelType)
			{
				case DuelType.Npc:
					Npc.AddDuelStats(result.DuelDuration, result.DuelWon);
					break;
				case DuelType.Vagabond:
					Vagabond.AddDuelStats(result.DuelDuration, result.DuelWon);
					break;
				case DuelType.Gate:
					Gate.AddDuelStats(result.DuelDuration, result.DuelWon);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}