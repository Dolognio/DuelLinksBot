namespace DuelLinksBot.UI.Tabs
{
	public partial class MainTab
	{
		public MainTab()
		{
			InitializeComponent();
			DataContext = AppContext.Instance.DuelLinksBotConfig;
		}
	}
}