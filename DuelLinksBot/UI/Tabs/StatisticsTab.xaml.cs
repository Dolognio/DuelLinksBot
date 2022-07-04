using DuelLinksBot.Utilities.Internal;

namespace DuelLinksBot.UI.Tabs
{
	public partial class StatisticsTab
	{
		public StatisticsTab()
		{
			InitializeComponent();
			DataContext = StatisticsUtil.GetStatisticsViewModel();
		}
	}
}