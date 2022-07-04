namespace DuelLinksBot.Utilities.Internal.Statistics
{
	public class OtherStatisticsType : IClearableStatistics
	{
		public OtherStatisticsType()
		{
			Others = new OtherStatisticEntry();
		}

		public OtherStatisticEntry Others { get; }

		public void ResetStats()
		{
			Others.ResetStats();
		}
	}
}