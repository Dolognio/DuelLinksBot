using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Accord.Extensions.Imaging.Algorithms.LINE2D;
using Accord.Imaging;
using DotImaging;
using DuelLinksBot.Configuration;
using DuelLinksBot.Exceptions;
using DuelLinksBot.Logging;
using DuelLinksBot.Utilities.Internal;
using DuelLinksBot.Utilities.Internal.Game;
using DuelLinksBot.Utilities.Internal.Graphics;
using Quellatalo.Nin.TheEyes;
using static DuelLinksBot.Utilities.Internal.Graphics.TemplatePictures;
using Match = Quellatalo.Nin.TheEyes.Match;
using TemplateMatch = DuelLinksBot.Utilities.Internal.Graphics.TemplateMatch;
using TemplatePyramid = Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplatePyramid<
	Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplate>;

namespace DuelLinksBot.Utilities
{
	public static class TemplatesUtil
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(TemplatesUtil));
		private static readonly Action<DuelLinksAppArea> NOOP_ACTION = appArea => { };

		private static readonly Action<DuelLinksAppArea> CLICKANYWHERE_ACTION =
			ClickInSpecificAreaAction(ClickAreas.ClickAnywhereRectangle);

		private static void GoToHomeScreen()
		{
			while (true)
			{
				if (IsHomeScreenVisible())
				{
					return;
				}

				GameUtils.SleepAfterGeneralAction();
				var appArea = GetDuelLinksAppArea();

				foreach (var template in PopupRelatedTemplates)
				{
					var clickableRect = SearchForAnyFirstMatch(appArea, template);
					if (clickableRect == null)
					{
						continue;
					}

					if (BackAndHideForTodayBtn == template)
					{
						LOGGER.Info(LogEntryType.BOT_ACTION, "'News' found.");
					}
					else if (new[] {CloseBtn, NextBtn, OkayBtn, NoBtn}.Contains(template))
					{
						LOGGER.Info(LogEntryType.BOT_ACTION, "'Popup' found.");
					}
					else if (LoginBtn == template)
					{
						LOGGER.Info(LogEntryType.BOT_ACTION, "'Login' found.");
					}
					else if (RebootBtn == template)
					{
						if (SearchForAnyFirstMatch(appArea, NoNetworkDetectedPopup) != null)
						{
							LOGGER.ErrorFormat("Network Error");
							StatisticsUtil.GetStatisticsViewModel().OtherStats.Others.IncrementNetworkErrorCounter();
						}
						else
						{
							LOGGER.ErrorFormat("Reboot required");
							StatisticsUtil.GetStatisticsViewModel().OtherStats.Others.IncrementOtherErrorCounter();
						}
					}
					else if (RetryBtn == template)
					{
						LOGGER.ErrorFormat("Network Error");
						StatisticsUtil.GetStatisticsViewModel().OtherStats.Others.IncrementNetworkErrorCounter();
					}
					else if (AutoDuelBtn == template)
					{
						LOGGER.Info(LogEntryType.BOT_ACTION, "Duel-Button detected.");
						var duelResult =
							SearchForAutoDuelBtnAndPlayDuel(DuelType.Npc, appArea);
						StatisticsUtil.GetStatisticsViewModel().DuelStats.AddDuelResult(duelResult);
					}

					MouseUtil.ClickInRectangle(appArea.TopLeft(), clickableRect);
					break;
				}

				//in case no other pattern matched the current displayImage,
				//just click anywhere (might be a new Bonus Campaign or something).
				LOGGER.Debug(LogEntryType.BOT_ACTION, "No pattern found. Clicking anywhere.");
				MouseUtil.ClickInRectangle(appArea.TopLeft(), ClickAreas.ClickAnywhereRectangle);
			}
		}

		public static IEnumerable<Rectangle> SearchForDuelists(DuelLinkWorld currentDuelLinkWorld, DuelArea currentDuelArea)
		{
			GoToHomeScreen();
			var appArea = GetDuelLinksAppArea();
			var subAreaForGivenDuelLinksWorld = currentDuelArea.SubAreaForGivenDuelLinksWorld(currentDuelLinkWorld);
			appArea = appArea.SubArea(subAreaForGivenDuelLinksWorld);
			LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Search for duelists/items...");
			using (var displayingImage = appArea.GetDisplayingImage())
			{
				var imageToMatchAgainst = displayingImage.ToBgr();
				try
				{
					var linPyr = LinearizedMapPyramid.CreatePyramid(imageToMatchAgainst); //prepare linear-pyramid maps
					var matches = linPyr.MatchTemplates(DuelistMatcher.DuelistTemplates, 96);

					var bestRepresentatives = new List<Rectangle>();
					var matchGroups = new MatchClustering(2).Group(matches.ToArray());
					foreach (var group in matchGroups)
					{
						var rectInSubArea = group.Representative.BoundingRect.ToRect();
						//add offset of whole appArea
						rectInSubArea.X += subAreaForGivenDuelLinksWorld.X;
						rectInSubArea.Y += subAreaForGivenDuelLinksWorld.Y;
						bestRepresentatives.Add(rectInSubArea);
					}

					LOGGER.DebugFormat(LogEntryType.BOT_ACTION, "{0} matches found", bestRepresentatives.Count);
					return bestRepresentatives;
				}
				catch (Exception e)
				{
					LOGGER.Error("Error when searching for duelists and items.", e);
					return Enumerable.Empty<Rectangle>();
				}
			}
		}

		public static void CheckForDuelOrItem(Rectangle duelistRect)
		{
			var duelLinksAppArea = GetDuelLinksAppArea();
			var clickableRect = new Rectangle(duelistRect.Center(), new Size(5, 5));
			//click on matched area (possibly a duelist or item)
			MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), clickableRect);
			GameUtils.SleepWhileWaitingForBonusOrDuelist();
			//check if it was a bonus item
			var receiveRewardsPopupMatch = SearchForAnyFirstMatch(duelLinksAppArea, ReceiveRewardsPopup);
			if (receiveRewardsPopupMatch != null)
			{
				LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Bonus found.");
				MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), receiveRewardsPopupMatch);
				return;
			}

			var vagabondDetected = SearchForAnyFirstMatch(duelLinksAppArea, VagabondDuelBar) != null;
			if (vagabondDetected || SearchForAnyFirstMatch(duelLinksAppArea, DuelTextBar, AutoDuelBtn) != null)
			{
				LOGGER.InfoFormat(LogEntryType.BOT_ACTION, vagabondDetected ? "Vagabond detected." : "SD/LD detected.");
				var duelResult =
					SearchForAutoDuelBtnAndPlayDuel(vagabondDetected ? DuelType.Vagabond : DuelType.Npc, duelLinksAppArea);
				StatisticsUtil.GetStatisticsViewModel().DuelStats.AddDuelResult(duelResult);
			}

			//then go to homescreen
			GoToHomeScreen();
		}

		private static DuelResult SearchForAutoDuelBtnAndPlayDuel(DuelType type, DuelLinksAppArea duelLinksAppArea)
		{
			//search for Auto-Duel button
			var autoDuelTemplateMatch =
				WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[] {AutoDuelBtn}, CLICKANYWHERE_ACTION);
			//click the auto-duel button
			WaitUntilTemplateNotFoundAnymoreWhileDoingWork(duelLinksAppArea, AutoDuelBtn,
				ClickInSpecificAreaAction(autoDuelTemplateMatch));
			LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Starting Auto-Duel...");
			//duel starts...
			var duelDurationSW = new Stopwatch();
			duelDurationSW.Start();
			var duelWon = WaitForDuelEndAndReturnResult(duelLinksAppArea);
			duelDurationSW.Stop();
			LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Duel-End detected.");

			//leave result screen
			void ClickOkNextOrAnywhereAction(DuelLinksAppArea appArea)
			{
				//click OK, Next, etc... until the duelist talks with us again
				var okOrNextBtnMatch = SearchForAnyFirstMatch(appArea, OkayBtn, NextBtn);
				if (okOrNextBtnMatch != null)
				{
					MouseUtil.ClickInRectangle(appArea.TopLeft(),
						okOrNextBtnMatch);
				}
				else
				{
					MouseUtil.ClickInRectangle(appArea.TopLeft(),
						ClickAreas.ClickAnywhereRectangle);
				}
			}

			//skip result screens, the boring character speech and just go back to homescreen
			GoToHomeScreen();

			return new DuelResult(type, duelDurationSW.Elapsed, duelWon);
		}

		private static bool WaitForDuelEndAndReturnResult(DuelLinksAppArea duelLinksAppArea)
		{
			void ActivateAutoDuelAgainIfDeactivated(DuelLinksAppArea appArea)
			{
				var inDuelAutoDuelDeactivatedBtnMatch =
					SearchForAnyFirstMatch(appArea, InDuelAutoDuelDeactivatedBtn);
				if (inDuelAutoDuelDeactivatedBtnMatch != null)
				{
					//Auto-Duel deactivated --> activate it
					MouseUtil.ClickInRectangle(appArea.TopLeft(), inDuelAutoDuelDeactivatedBtnMatch);
				}
			}

			var okayBtnAfterDuelMatch =
				WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[] {OkayBtn}, ActivateAutoDuelAgainIfDeactivated,
					false);
			//OK button displayed -> duel-end
			var duelLost = SearchForAnyFirstMatch(duelLinksAppArea, DuelLostScreen) != null;
			MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), okayBtnAfterDuelMatch);

			return !duelLost;
		}

		public static void ClickStageEventInDuelArea(DuelLinkWorld currentDuelLinkWorld, DuelArea currentDuelArea)
		{
			GoToHomeScreen();
			var appArea = GetDuelLinksAppArea();
			LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Stage-Event for '{0}'", currentDuelArea.NameOfArea);
			MouseUtil.ClickInRectangle(appArea.TopLeft(),
				currentDuelArea.StageEventAreaForGivenDuelLinksWorld(currentDuelLinkWorld));
			GameUtils.SleepAfterStageEvent();
		}

		public static DuelLinkWorld GetCurrentYuGiOhSerie()
		{
			var retry = 0;
			while (retry < 5)
			{
				GoToHomeScreen();
				var appArea = GetDuelLinksAppArea();
				MouseUtil.ClickInRectangle(appArea.TopLeft(), ClickAreas.SwitchWorldsButton);
				GameUtils.SleepAfterClickingSwitchWorldToCheckSerie();
				var matchFound = SearchForAnyFirstMatch(appArea, GxSerieBtn, NormalSerieBtn);
				if (matchFound == null)
				{
					retry++;
					continue;
				}

				if (GxSerieBtn.Equals(matchFound.FoundTemplate))
				{
					LOGGER.Info(LogEntryType.BOT_ACTION, "'Normal' serie detected");
					return DuelLinkWorld.Normal;
				}

				LOGGER.Info(LogEntryType.BOT_ACTION, "'GX' serie detected");
				return DuelLinkWorld.Gx;
			}

			LOGGER.Info(LogEntryType.BOT_ACTION, "Using fallback: 'GX' serie.");
			return DuelLinkWorld.Gx;
		}

		public static DuelArea ChangeToNextDuelArea()
		{
			GoToHomeScreen();

			var appArea = GetDuelLinksAppArea();
			DuelArea duelAreaToChangeTo;
			if (SearchForAnyFirstMatch(appArea, DuelArea.GATE_AREA.AreaButtonSelectedTemplate) != null)
			{
				duelAreaToChangeTo = DuelArea.ARENA_AREA;
			}
			else if (SearchForAnyFirstMatch(appArea, DuelArea.ARENA_AREA.AreaButtonSelectedTemplate)
			         != null)
			{
				duelAreaToChangeTo = DuelArea.SHOP_AREA;
			}
			else if (SearchForAnyFirstMatch(appArea, DuelArea.SHOP_AREA.AreaButtonSelectedTemplate)
			         != null)
			{
				duelAreaToChangeTo = DuelArea.CARD_STUDIO_AREA;
			}
			else
			{
				duelAreaToChangeTo = DuelArea.GATE_AREA;
			}

			EnsureToDisplayDuelArea(duelAreaToChangeTo);
			return duelAreaToChangeTo;
		}

		public static void EnsureToDisplayDuelArea(DuelArea duelArea)
		{
			void GoToHomescreenAndChangeToOtherDuelArea(DuelLinksAppArea appArea)
			{
				GoToHomeScreen();

				var matchFound = SearchForAnyFirstMatch(appArea, duelArea.AreaButtonTemplate);
				if (matchFound != null)
				{
					LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Switching to '{0}'", duelArea.NameOfArea);
					MouseUtil.ClickInRectangle(appArea.TopLeft(), matchFound);
				}
			}

			var duelLinksAppArea = GetDuelLinksAppArea();
			WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[] {duelArea.AreaButtonSelectedTemplate},
				GoToHomescreenAndChangeToOtherDuelArea);
			LOGGER.DebugFormat(LogEntryType.BOT_ACTION, "Current area: '{0}'", duelArea.NameOfArea);
		}

		public static void HandleGateDuels(int numberOfGateDuelsInARow,
			Queue<GateCharConfigDuelistMapping> allGateCharConfigMappings)
		{
			LOGGER.Info(LogEntryType.BOT_ACTION, "Switching to Gate-Farming.");
			var amountOfAlreadyDoneGateDuels = 0;
			var gateCharsToIgnoreForDuels = new List<GateCharConfigDuelistMapping>();
			while (amountOfAlreadyDoneGateDuels < numberOfGateDuelsInARow)
			{
				//check which duelist we should duel against
				var nextGateDuelistToDuelAgainst =
					GameUtils.GetNextGateDuelistToDuelAgainst(ref allGateCharConfigMappings, ref gateCharsToIgnoreForDuels);
				if (nextGateDuelistToDuelAgainst == null)
				{
					LOGGER.Info(LogEntryType.BOT_ACTION, "No more gate duels configured or possible");
					LOGGER.Info(LogEntryType.BOT_ACTION, "Ending Gate-Farming.");
					return;
				}

				//open gate area
				OpenGateAreaIfNotDoneYet();
				//TODO switch to correct gate
				//switch to the duelist and correct level
				var charFound = SwitchToGateDuelistAndLevelReturningTrueIfFound(nextGateDuelistToDuelAgainst.GateDuelist,
					nextGateDuelistToDuelAgainst.GateCharConfigWrapper.DuelistLevel);
				if (!charFound)
				{
					gateCharsToIgnoreForDuels.Add(nextGateDuelistToDuelAgainst);
					continue;
				}

				var duelingSuccessful = StartGateDuelIfPossible();
				if (duelingSuccessful)
				{
					amountOfAlreadyDoneGateDuels++;
					if (nextGateDuelistToDuelAgainst.GateCharConfigWrapper.Repetitions > 0)
					{
						nextGateDuelistToDuelAgainst.GateCharConfigWrapper.Repetitions--;
					}
				}
				else
				{
					gateCharsToIgnoreForDuels.Add(nextGateDuelistToDuelAgainst);
				}
			}

			LOGGER.Info(LogEntryType.BOT_ACTION, "Ending Gate-Farming.");
		}

		private static void OpenGateAreaIfNotDoneYet()
		{
			var appArea = GetDuelLinksAppArea();
			//if gate not open
			if (SearchForAnyFirstMatch(appArea, GateOpenedScreen) != null)
			{
				return;
			}

			//go to gate area
			EnsureToDisplayDuelArea(DuelArea.GATE_AREA);
			//open gate
			MouseUtil.ClickInRectangle(appArea.TopLeft(), ClickAreas.OpenGateArea);
			WaitUntilAnyTemplateFoundWhileDoingWork(appArea, new[] {GateOpenedScreen});
			GameUtils.SleepAfterLongUiAction();
		}

		private static bool SwitchToGateDuelistAndLevelReturningTrueIfFound(DuelistMatcher.GateDuelist gateDuelist,
			DuelistLevel duelistLevel)
		{
			var appArea = GetDuelLinksAppArea();
			while (SearchForAnyFirstMatch(appArea, gateDuelist) == null)
			{
				//change to next duelist
				MouseUtil.ClickInRectangle(appArea.TopLeft(), ClickAreas.RightArrow);
				GameUtils.SleepAfterLongUiAction();
			}
			//TODO implement fallback if duelist not found, otherwise endless loop and return false!!

			//duelist selected -> ensure correct level is set
			MouseUtil.ClickInRectangle(appArea.TopLeft(), gateDuelist.GetClickAreaForDuelistLevel(duelistLevel));
			GameUtils.SleepAfterGeneralAction();
			return true;
		}

		private static bool StartGateDuelIfPossible()
		{
			var appArea = GetDuelLinksAppArea();

			//TODO check for grey duel button
			//click duell!! button
			ClickInSpecificAreaAction(ClickAreas.GateDuelBtn).Invoke(appArea);

			try
			{
				//click auto-duel (with mod only)
				var duelResult = SearchForAutoDuelBtnAndPlayDuel(DuelType.Gate, appArea);
				StatisticsUtil.GetStatisticsViewModel().DuelStats.AddDuelResult(duelResult);
				return true;
			}
			catch (DuelLinksTemplateTimeoutException)
			{
				LOGGER.Error("Gate duel could not be started. Is the dll MOD installed?");
				return false;
			}
		}

		public static void WatchStreetReplayIfNotDoneYet(DuelLinkWorld currentYuGiOhSerie, DuelArea currentDuelArea)
		{
			GoToHomeScreen();
			if (DuelArea.CARD_STUDIO_AREA != currentDuelArea)
			{
				LOGGER.DebugFormat(LogEntryType.WARNING, "Watching Street-Replay requested, but duelArea was {0}.",
					currentDuelArea);
				return;
			}

			var streetReplayBtnLocation = DuelLinkWorld.Normal == currentYuGiOhSerie
				? ClickAreas.NormalStartStreetReplay
				: ClickAreas.GxStartStreetReplay;
			var duelLinksAppArea = GetDuelLinksAppArea();
			LOGGER.Info(LogEntryType.BOT_ACTION, "Checking for Street-Replay...");
			MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), streetReplayBtnLocation);
			GameUtils.SleepAfterLongUiAction();
			if (SearchForAnyFirstMatch(duelLinksAppArea, StreetReplayPopup) == null)
			{
				return;
			}

			if (SearchForAnyFirstMatch(duelLinksAppArea, StreetReplayDailyAlreadyReceivedMsg) != null)
			{
				//street replay already watched for today -> no more gems can be earned
				LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Skipping Street-Replay, gems already earned.");
				var noBtn = SearchForAnyFirstMatch(duelLinksAppArea, NoBtn);
				if (noBtn != null)
				{
					MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), noBtn);
				}

				return;
			}

			//start street replay
			var yesBtn = SearchForAnyFirstMatch(duelLinksAppArea, YesBtn);
			if (yesBtn != null)
			{
				MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), yesBtn);
				LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Starting Street-Replay-Duel...");
				//duel starts...
				WaitForDuelEndAndReturnResult(duelLinksAppArea);
				LOGGER.InfoFormat(LogEntryType.BOT_ACTION, "Street-Replay-Duel-End detected.");
				//wait for "End" button being displayed
				var endBtnMatch = WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[] {EndBtn});
				MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), endBtnMatch);
				GameUtils.SleepAfterGeneralAction();
			}
		}

		public static void CollectGifts()
		{
			GoToHomeScreen();

			var duelLinksAppArea = GetDuelLinksAppArea();
			//click gift box to open it
			LOGGER.Info(LogEntryType.BOT_ACTION, "Checking for gifts...");
			var receiveGiftsBtn = WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[]
			{
				BackBtn,
				ReceiveAllGiftsBtn, ReceiveAllGiftsUnavailBtn
			}, ClickInSpecificAreaAction(ClickAreas.OpenGiftBox));
			//receive gifts
			if (ReceiveAllGiftsBtn.Equals(receiveGiftsBtn.FoundTemplate))
			{
				GameUtils.SleepAfterLongUiAction();
				LOGGER.Info(LogEntryType.BOT_ACTION, "Collecting gifts...");
				MouseUtil.ClickInRectangle(duelLinksAppArea.TopLeft(), receiveGiftsBtn);
			}

			GoToHomeScreen();
		}

		public static bool IsHomeScreenVisible()
		{
			var appArea = GetDuelLinksAppArea();
			var homescreenVisible = SearchForAnyFirstMatch(appArea, SettingsBtn) != null;
			LOGGER.DebugFormat(LogEntryType.RUN_INFO, "Homescreen is visible: {0}", homescreenVisible);
			return homescreenVisible;
		}

		//TODO implement
		public static void StartRiseOfYubelEventFarming()
		{
			OpenGateAreaIfNotDoneYet();
			//switch to event gate

			var duelLinksAppArea = GetDuelLinksAppArea();

			void ActionToDoWhileWaiting(DuelLinksAppArea appArea)
			{
				//search for 'switch gate' button and click it
				var switchGateBtn = SearchForAnyFirstMatch(appArea, SwitchGateBtn);
				ClickInSpecificAreaAction(switchGateBtn);
				GameUtils.SleepAfterGeneralAction();
				//click event gate
//				ClickInSpecificAreaAction()
			}

			WaitUntilAnyTemplateFoundWhileDoingWork(duelLinksAppArea, new[] {RiseOfYubelEventGateScreen},
				ActionToDoWhileWaiting);
		}


		private static TemplateMatch WaitUntilAnyTemplateFoundWhileDoingWork(DuelLinksAppArea duelLinksAppArea,
			Template[] templates, Action<DuelLinksAppArea> actionToDoWhileWaiting = null, bool withTimeout = true)
		{
			if (actionToDoWhileWaiting == null)
			{
				actionToDoWhileWaiting = NOOP_ACTION;
			}

			var retries = 0;
			TemplateMatch foundTemplate;
			while ((foundTemplate = SearchForAnyFirstMatch(duelLinksAppArea, templates)) == null)
			{
				var retryOrRebootTemplateMatch = CheckForRetryOrRebootError(duelLinksAppArea);
				HandleRebootOrRetryOrThrowExceptionOnTimeout(retryOrRebootTemplateMatch, ref retries, withTimeout);

				actionToDoWhileWaiting(duelLinksAppArea);
				GameUtils.SleepAfterGeneralAction();
				retries++;
			}

			return foundTemplate;
		}

		private static void WaitUntilTemplateNotFoundAnymoreWhileDoingWork(DuelLinksAppArea duelLinksAppArea,
			Template template,
			Action<DuelLinksAppArea> actionToDoWhileWaiting = null, bool withTimeout = true)
		{
			if (actionToDoWhileWaiting == null)
			{
				actionToDoWhileWaiting = NOOP_ACTION;
			}

			var retries = 0;
			TemplateMatch retryOrRebootTemplateMatch = null;
			while (SearchForAnyFirstMatch(duelLinksAppArea, template) != null
			       || (retryOrRebootTemplateMatch = CheckForRetryOrRebootError(duelLinksAppArea)) != null)
			{
				HandleRebootOrRetryOrThrowExceptionOnTimeout(retryOrRebootTemplateMatch, ref retries, withTimeout);

				actionToDoWhileWaiting(duelLinksAppArea);
				GameUtils.SleepAfterGeneralAction();
				retries++;
			}
		}

		private static void HandleRebootOrRetryOrThrowExceptionOnTimeout(TemplateMatch retryOrRebootTemplateMatch,
			ref int retries, bool withTimeout)
		{
			if (retryOrRebootTemplateMatch != null && RetryBtn == retryOrRebootTemplateMatch.FoundTemplate)
			{
				LOGGER.ErrorFormat("Network Error");
				StatisticsUtil.GetStatisticsViewModel().OtherStats.Others.IncrementNetworkErrorCounter();
				ClickInSpecificAreaAction(retryOrRebootTemplateMatch).Invoke(GetDuelLinksAppArea());
				//reset retries since we clicked retry (last action could be successful now)
				retries = 0;
			}
			else if (retryOrRebootTemplateMatch != null && RebootBtn == retryOrRebootTemplateMatch.FoundTemplate)
			{
				ClickInSpecificAreaAction(retryOrRebootTemplateMatch).Invoke(GetDuelLinksAppArea());
				//reboot triggered, we need to cancel this action
				LOGGER.ErrorFormat("Reboot Error");
				StatisticsUtil.GetStatisticsViewModel().OtherStats.Others.IncrementOtherErrorCounter();
				throw new DuelLinksRebootDetectedException();
			}
			else if (withTimeout && retries >= ProgramConstants.MAX_RETRIES_WHEN_WAITING_FOR_TEMPLATES)
			{
				//neither retry nor reboot detected, problem must be something else
				throw new DuelLinksTemplateTimeoutException();
			}
		}

		private static TemplateMatch CheckForRetryOrRebootError(DuelLinksAppArea appArea)
		{
			return SearchForAnyFirstMatch(appArea, RetryBtn, RebootBtn);
		}

		private static Action<DuelLinksAppArea> ClickInSpecificAreaAction(TemplateMatch templateMatch)
		{
			return appArea => MouseUtil.ClickInRectangle(appArea.TopLeft(), templateMatch);
		}

		private static Action<DuelLinksAppArea> ClickInSpecificAreaAction(Rectangle clickArea)
		{
			return appArea => MouseUtil.ClickInRectangle(appArea.TopLeft(), clickArea);
		}

		private static TemplateMatch SearchForAnyFirstMatch(DuelLinksAppArea appArea, params Template[] templates)
		{
			if (templates.Length == 0)
			{
				return null;
			}

			foreach (var template in templates)
			{
				var match = appArea.Find(template);
				if (match != null)
				{
					//match found, just double check
					GameUtils.SleepBeforeTemplateDoublecheck();
					match = appArea.Find(template);
					if (match != null)
					{
						return CalculateClickableArea(template, match);
					}

					LOGGER.Debug(LogEntryType.OTHER, "Doublechecking template: false...");
				}
			}

			return null;
		}

		private static TemplateMatch CalculateClickableArea(Template template, Match match)
		{
			if (!(template is ClickableTemplate))
			{
				//template has no clickable area
				return new TemplateMatch(null, null);
			}

			var clickableTemplate = (ClickableTemplate) template;
			//add offset of clickable area to match of templateImage
			var clickableX = clickableTemplate.ClickableArea.X + match.Rectangle.X;
			var clickableY = clickableTemplate.ClickableArea.Y + match.Rectangle.Y;
			var calculatedClickableArea = new Rectangle(clickableX, clickableY, clickableTemplate.ClickableArea.Width,
				clickableTemplate.ClickableArea.Height);
			return new TemplateMatch(calculatedClickableArea, template);
		}

		internal static DuelLinksAppArea GetDuelLinksAppArea()
		{
			var appArea = new Area(AppContext.Instance.DuelLinksApp.GetMainWindowRectangle());
			var rect = appArea.Rectangle;
			return new DuelLinksAppArea(appArea.SubArea(370, 0, rect.Width - 740, rect.Height));
		}
	}
}