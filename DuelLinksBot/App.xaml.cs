using System;
using System.Windows;
using DuelLinksBot.Logging;

namespace DuelLinksBot
{
	/// <summary>
	///  Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(App));

		public App()
		{
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += DuelLinksUnhandledExceptionHandler;
		}

		private static void DuelLinksUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			LOGGER.ErrorFormat("Unhandled Exception: {0}", e.ExceptionObject);
		}

		private void App_OnExit(object sender, ExitEventArgs e)
		{
			AppContext.Instance.SaveCurrentSettings();
		}
	}
}