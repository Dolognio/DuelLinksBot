using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DuelLinksBot.Configuration;
using DuelLinksBot.Exceptions;
using DuelLinksBot.HelpersLib;
using DuelLinksBot.Logging;
using NHotkey;
using NHotkey.Wpf;
using SettingsProviderNet;

namespace DuelLinksBot
{
	public sealed class AppContext
	{
		private const string STOP_BOT_HOTKEY_IDENTIFIER = "StopBotHotkeyIdentifier";
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(AppContext));
		private BotWorker currentBotWorker;

		private AppContext()
		{
			InitializeBotForRunInstance();
		}

		public Quellatalo.Nin.TheEyes.App DuelLinksApp { get; private set; }
		public ObservableCollection<LogEntry> LogEntries { get; private set; }
		public DuelLinksBotConfig DuelLinksBotConfig { get; private set; }

		public static AppContext Instance { get; } = new AppContext();

		private void InitializeBotForRunInstance()
		{
			CreateOrReadConfiguration();
			InitializeLogging();
		}

		public BotWorker PrepareDuelLinksAppForBotRunning()
		{
			LOGGER.Debug(LogEntryType.RUN_INFO, "Preparing Bot for run...");
			FindDuelLinksApp();
			DuelLinksApp.ToFront();
			NativeWrapper.SetTopMost(DuelLinksApp.Process.MainWindowHandle, true);
			HotkeyManager.Current.AddOrReplace(STOP_BOT_HOTKEY_IDENTIFIER, Key.Space, ModifierKeys.None, OnStopHotkey);

			SaveCurrentSettings();
			currentBotWorker = new BotWorker(DuelLinksBotConfig);
			return currentBotWorker;
		}

		public void ReleaseDuelLinksApp()
		{
			LOGGER.Debug(LogEntryType.RUN_INFO, "Releasing Duel-Links...");
			HotkeyManager.Current.Remove(STOP_BOT_HOTKEY_IDENTIFIER);
			if (DuelLinksApp != null)
			{
				NativeWrapper.SetTopMost(DuelLinksApp.Process.MainWindowHandle, false);
			}

			currentBotWorker = null;
			DuelLinksApp = null;
		}

		private void FindDuelLinksApp()
		{
			DuelLinksApp = Quellatalo.Nin.TheEyes.App.GetAppByWindowTitle(ProgramConstants.DUEL_LINKS_WINDOW_NAME);
			if (DuelLinksApp != null)
			{
				return;
			}

			var duelLinksNotFoundException = new DuelLinksNotFoundException();
			LOGGER.Error("Duel-Links process not found. Is the game already started?", duelLinksNotFoundException);
			throw duelLinksNotFoundException;
		}

		private void CreateOrReadConfiguration()
		{
			//ensure configuration directory exists
			Directory.CreateDirectory(ProgramConstants.MY_DOCS_BOT_FOLDER);
			DuelLinksBotConfig = LoadSettings();
		}

		private void OnStopHotkey(object sender, HotkeyEventArgs e)
		{
			currentBotWorker.StopBot();
		}

		private void InitializeLogging()
		{
			LogEntries = new ObservableCollection<LogEntry>();
		}

		private static DuelLinksBotConfig LoadSettings()
		{
			var settingsProvider = new SettingsProvider(new SettingsStorage(ProgramConstants.MY_DOCS_BOT_FOLDER));
			return settingsProvider.GetSettings<DuelLinksBotConfig>();
		}

		public void SaveCurrentSettings()
		{
			var settingsProvider = new SettingsProvider(new SettingsStorage(ProgramConstants.MY_DOCS_BOT_FOLDER));
			settingsProvider.SaveSettings(DuelLinksBotConfig);
		}
	}
}