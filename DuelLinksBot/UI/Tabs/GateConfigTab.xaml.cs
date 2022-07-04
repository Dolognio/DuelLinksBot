namespace DuelLinksBot.UI.Tabs
{
	public partial class GateConfigTab
	{
		public GateConfigTab()
		{
			InitializeComponent();
			DataContext = AppContext.Instance.DuelLinksBotConfig;
		}
	}
}