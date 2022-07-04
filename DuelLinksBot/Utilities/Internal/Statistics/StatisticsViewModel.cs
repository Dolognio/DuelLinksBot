using System.Collections.ObjectModel;

namespace DuelLinksBot.Utilities.Internal.Statistics
{
	public class StatisticsViewModel
	{
		public readonly DuelStatisticsType DuelStats = new DuelStatisticsType();

		public StatisticsViewModel()
		{
			DuelStatistics =
				new ObservableCollection<DuelStatisticEntry>
				{
					DuelStats.Npc,
					DuelStats.Vagabond,
					DuelStats.Gate
				};
		}

		public ObservableCollection<DuelStatisticEntry> DuelStatistics { get; }
		public OtherStatisticsType OtherStats { get; } = new OtherStatisticsType();
	}
}