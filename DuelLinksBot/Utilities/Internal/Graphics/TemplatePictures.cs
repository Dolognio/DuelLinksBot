using System.Drawing;
using DuelLinksBot.Properties;
using Quellatalo.Nin.TheEyes;

namespace DuelLinksBot.Utilities.Internal.Graphics
{
	public static class TemplatePictures
	{
		public static readonly Template LoginBtn = new ClickableTemplate(Resources.login_button);

		public static readonly Template MaintenanceInProgressIcon
			= new Template(Resources.maintenanceInProgress, 0.98);

		public static readonly Template OkayBtn = new ClickableTemplate(Resources.ok_button);
		public static readonly Template CloseBtn = new ClickableTemplate(Resources.close_button);
		public static readonly Template NextBtn = new ClickableTemplate(Resources.next_button);
		public static readonly Template CancelBtn = new ClickableTemplate(Resources.cancel_button);
		public static readonly Template NoBtn = new ClickableTemplate(Resources.no_button);
		public static readonly Template YesBtn = new ClickableTemplate(Resources.yes_button);
		public static readonly Template EndBtn = new ClickableTemplate(Resources.end_button);
		public static readonly Template RebootBtn = new ClickableTemplate(Resources.reboot_button);
		public static readonly Template RetryBtn = new ClickableTemplate(Resources.retry_button);

		public static readonly Template BackAndHideForTodayBtn =
			new ClickableTemplate(Resources.back_hideForToday_button, 0.98, new Rectangle(498, 19, 25, 25));

		public static readonly Template BackAndHideForTodaySelectedBtn =
			new ClickableTemplate(Resources.back_hideForTodaySelected_button, 0.98, new Rectangle(10, 11, 50, 30));

		public static readonly Template BackBtn = new ClickableTemplate(Resources.back_button);
		public static readonly Template HomeBtn = new ClickableTemplate(Resources.home_button);
		public static readonly Template SettingsBtn = new Template(Resources.settings_button, 0.99999);
		public static readonly Template UpdateBtn = new ClickableTemplate(Resources.update_button);
		public static readonly Template SwitchGateBtn = new ClickableTemplate(Resources.switch_gate_button);

		//series
		public static readonly Template GxSerieBtn = new ClickableTemplate(Resources.gxSerie_button, 0.92);
		public static readonly Template NormalSerieBtn = new ClickableTemplate(Resources.normalSerie_button, 0.92);

		//areas
		public static readonly Template GateAreaBtn = new ClickableTemplate(Resources.gateArea_button);
		public static readonly Template GateAreaSelectedBtn = new ClickableTemplate(Resources.gateArea_button_sel);
		public static readonly Template ArenaAreaBtn = new ClickableTemplate(Resources.arenaArea_button);
		public static readonly Template ArenaAreaSelectedBtn = new ClickableTemplate(Resources.arenaArea_button_sel);
		public static readonly Template ShopAreaBtn = new ClickableTemplate(Resources.shopArea_button);
		public static readonly Template ShopAreaSelectedBtn = new ClickableTemplate(Resources.shopArea_button_sel);
		public static readonly Template CardStudioAreaBtn = new ClickableTemplate(Resources.cardStudioArea_button);

		public static readonly Template CardStudioAreaSelectedBtn =
			new ClickableTemplate(Resources.cardStudioArea_button_sel);

		//duel
		public static readonly Template DuelTextBar = new Template(Resources.duel_text_bar);
		public static readonly Template VagabondDuelBar = new Template(Resources.vagabond_duel_bar);
		public static readonly Template AutoDuelBtn = new ClickableTemplate(Resources.auto_duel_button);
		public static readonly Template ReceiveRewardsPopup = new Template(Resources.receive_rewards_popup);
		public static readonly Template ReceiveAllGiftsBtn = new ClickableTemplate(Resources.receive_all_gifts_button);
		public static readonly Template ReceiveAllGiftsUnavailBtn = new Template(Resources.receive_all_gifts_unavail_button);
		public static readonly Template DuelLostScreen = new Template(Resources.you_loose_message, 0.7);

		//street replay
		public static readonly Template StreetReplayPopup = new Template(Resources.street_replay_popup);

		public static readonly Template StreetReplayDailyAlreadyReceivedMsg =
			new Template(Resources.street_replay_daily_already_received_msg);

		//gate screen
		public static readonly Template GateOpenedScreen = new Template(Resources.gate_screen);

		public static readonly Template InDuelAutoDuelActivatedBtn =
			new ClickableTemplate(Resources.in_duel_auto_duel_activated_button);

		public static readonly Template InDuelAutoDuelDeactivatedBtn =
			new ClickableTemplate(Resources.in_duel_auto_duel_deactivated_button);

		public static readonly Template AfterDuelDuelResultScreen =
			new Template(Resources.after_duel_duel_results_screen);

		//errors
		public static readonly Template NoNetworkDetectedPopup = new Template(Resources.no_network_detected_error);

		//events
		public static readonly Template RiseOfYubelEventGateScreen = new Template(Resources.rise_of_yubel_event_gate_image);

		public static readonly Template[] PopupRelatedTemplates =
		{
			LoginBtn, HomeBtn, OkayBtn, CloseBtn, NextBtn, CancelBtn, NoBtn, BackAndHideForTodaySelectedBtn,
			BackAndHideForTodayBtn,
			BackBtn, UpdateBtn, RetryBtn, RebootBtn,
			AutoDuelBtn
		};

		public class Template : Pattern
		{
			internal Template(Bitmap templateImage, double similarityThreshold = 0.94) : base(
				templateImage,
				similarityThreshold)
			{
			}
		}

		public class ClickableTemplate : Template
		{
			internal ClickableTemplate(Bitmap templateImage, double similarityThreshold = 0.94,
				Rectangle? clickableAreaOverwrite = null) : base(templateImage,
				similarityThreshold)
			{
				ClickableArea = clickableAreaOverwrite ?? GetClickableAreaForCompleteImage(templateImage);
			}

			public Rectangle ClickableArea { get; }

			private static Rectangle GetClickableAreaForCompleteImage(Bitmap image)
			{
				return new Rectangle(0, 0, image.Width, image.Height);
			}
		}
	}
}