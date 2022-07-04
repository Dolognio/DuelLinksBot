using System;
using System.Windows;
using DuelLinksBot.Configuration;
using DuelLinksBot.DebugHelpers;
using DuelLinksBot.Exceptions;
using DuelLinksBot.Utilities;
using DuelLinksBot.Utilities.Internal.Game;
using Quellatalo.Nin.TheEyes;

namespace DuelLinksBot.UI.Tabs
{
	public partial class DebugTab
	{
		public DebugTab()
		{
			InitializeComponent();
			DataContext = AppContext.Instance.DuelLinksBotConfig;
		}

		private void ButtonTestTemplate_OnClick(object sender, RoutedEventArgs e)
		{
			TemplatesUtil.IsHomeScreenVisible();
		}

		private void ButtonHighlightArea_OnClick(object sender, RoutedEventArgs e)
		{
			var x = int.Parse(X.Text);
			var y = int.Parse(Y.Text);
			var w = int.Parse(W.Text);
			var h = int.Parse(H.Text);
			DebugUtils.HighlightArea(x, y, w, h);
		}

		private void ButtonClearHighlight_OnClick(object sender, RoutedEventArgs e)
		{
			Area.ClearHighlight();
		}

		private void ButtonTestFindDuelists_OnClick(object sender, RoutedEventArgs e)
		{
			DuelArea area;
			if (GateRadio.IsChecked != null && GateRadio.IsChecked.Value)
			{
				area = DuelArea.GATE_AREA;
			}
			else if (ArenaRadio.IsChecked != null && ArenaRadio.IsChecked.Value)
			{
				area = DuelArea.ARENA_AREA;
			}
			else if (ShopRadio.IsChecked != null && ShopRadio.IsChecked.Value)
			{
				area = DuelArea.SHOP_AREA;
			}
			else
			{
				area = DuelArea.CARD_STUDIO_AREA;
			}

			DebugUtils.TestFindDuelists(area);
		}

		private void ButtonMakeAppScreenshot_OnClick(object sender, RoutedEventArgs e)
		{
			DebugUtils.MakeDuelLinksAppScreenshot();
		}

		private void ButtonSendPushbulletNotification_OnClick(object sender, RoutedEventArgs e)
		{
			DebugUtils.SendErrorPushbulletNotification(((DuelLinksBotConfig) DataContext).PushbulletApiToken,
				new Exception("TestMsg", new DuelLinksTemplateTimeoutException()));
		}
	}
}