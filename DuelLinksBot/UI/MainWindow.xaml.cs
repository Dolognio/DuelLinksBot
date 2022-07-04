using System;
using System.Threading.Tasks;
using System.Windows;
using DuelLinksBot.Logging;
using DuelLinksBot.Utilities.Internal;

namespace DuelLinksBot.UI
{
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(MainWindow));

		public MainWindow()
		{
			InitializeComponent();
			DataContext = AppContext.Instance.LogEntries;
		}

		private bool IsBotRunning { get; set; }

		private async void ButtonStartBot_OnClick(object sender, RoutedEventArgs e)
		{
			if (IsBotRunning)
			{
				LOGGER.Debug(LogEntryType.RUN_INFO, "Bot is already running...");
				return;
			}

			StartBotBtn.IsEnabled = false;
			SpaceBotHintLbl.Visibility = Visibility.Visible;
			try
			{
				var botWorker = AppContext.Instance.PrepareDuelLinksAppForBotRunning();
				StatisticsUtil.ResetStatistics();
				MainWindowTabControl.SelectedIndex = 0;
				IsBotRunning = true;
				await Task.Run(() =>
						botWorker.StartBot(),
					botWorker.GetCancellationToken());
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				AppContext.Instance.ReleaseDuelLinksApp();
				LOGGER.Info(LogEntryType.RUN_INFO, "Bot stopped!");
				IsBotRunning = false;
				StartBotBtn.IsEnabled = true;
				SpaceBotHintLbl.Visibility = Visibility.Hidden;
			}
		}
	}
}