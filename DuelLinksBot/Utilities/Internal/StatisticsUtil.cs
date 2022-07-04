using DuelLinksBot.Utilities.Internal.Statistics;

namespace DuelLinksBot.Utilities.Internal
{
	public static class StatisticsUtil
	{
		private static StatisticsViewModel _statisticsViewModel;

		static StatisticsUtil()
		{
			InitializeStatistics();
		}

		private static void InitializeStatistics()
		{
			_statisticsViewModel = new StatisticsViewModel();
		}

		public static StatisticsViewModel GetStatisticsViewModel()
		{
			return _statisticsViewModel;
		}

		public static void ResetStatistics()
		{
			GetStatisticsViewModel().DuelStats.ResetStats();
			GetStatisticsViewModel().OtherStats.ResetStats();
		}
	}
}