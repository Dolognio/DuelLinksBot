using System.Windows.Controls;

namespace DuelLinksBot.UI.Tabs
{
	public partial class AdvancedTab
	{
		public AdvancedTab()
		{
			InitializeComponent();
			DataContext = AppContext.Instance.DuelLinksBotConfig;
		}
	}
}
