using System;
using System.Threading;
using DuelLinksBot.Configuration;
using DuelLinksBot.DebugHelpers;
using DuelLinksBot.Exceptions;
using DuelLinksBot.Logging;
using DuelLinksBot.Utilities;
using DuelLinksBot.Utilities.Internal.Game;

namespace DuelLinksBot
{
	public class BotWorker
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(BotWorker));
		private readonly Action cancelBotAction;
		private readonly CancellationTokenSource cancellationTokenSource;

		private readonly DuelLinksBotConfig duelLinksBotConfig;
		private DuelArea currentDuelArea;
		private DuelLinkWorld currentDuelLinkWorld;

		public BotWorker(DuelLinksBotConfig duelLinksBotConfig)
		{
			cancellationTokenSource = new CancellationTokenSource();
			this.duelLinksBotConfig = duelLinksBotConfig;

			cancelBotAction = () => { cancellationTokenSource.Cancel(); };
		}

		public void StartBot()
		{
			var cancellationToken = cancellationTokenSource.Token;
			try
			{
				if (duelLinksBotConfig.DebugOnly)
				{
					LOGGER.Info(LogEntryType.RUN_INFO, "Bot started for debugging only!");
					while (true)
					{
						cancellationToken.ThrowIfCancellationRequested();
					}
				}

				var botThreadFunction = new ThreadStart(BotWorkerThread);
				var botThread = new Thread(botThreadFunction) {IsBackground = true};
				botThread.Start();

				while (true)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						botThread.Abort();
						Thread.Sleep(500);
						botThread.Join(TimeSpan.FromSeconds(3));
						cancellationToken.ThrowIfCancellationRequested();
					}
				}
			}
			catch (OperationCanceledException)
			{
				//ignore (bot was stopped)
			}
			catch (Exception e)
			{
				LOGGER.Error("Error while running bot.", e);
			}
		}

		public CancellationToken GetCancellationToken()
		{
			return cancellationTokenSource.Token;
		}

		public void StopBot()
		{
			LOGGER.Debug(LogEntryType.RUN_INFO, "STOP triggered");
			cancelBotAction.Invoke();
		}

		private void BotWorkerThread()
		{
			try
			{
				LOGGER.Info(LogEntryType.RUN_INFO, "Bot started!");
				LOGGER.Info(LogEntryType.RUN_INFO, "Checking Yu-Gi-Oh! serie...");
				InitializeBot();

				while (true)
				{
					try
					{
						//if configured search for stageevents
						if (duelLinksBotConfig.FarmStageEvent)
						{
							TemplatesUtil.ClickStageEventInDuelArea(currentDuelLinkWorld, currentDuelArea);
						}

						//if configured watch the street replay to earn gems
						if (DuelArea.CARD_STUDIO_AREA == currentDuelArea
						    && duelLinksBotConfig.FarmStreetReplay)
						{
							TemplatesUtil.WatchStreetReplayIfNotDoneYet(currentDuelLinkWorld, currentDuelArea);
						}

						//if configured receive gifts in giftbox
						if (DuelArea.GATE_AREA == currentDuelArea
						    && duelLinksBotConfig.ReceiveGifts)
						{
							TemplatesUtil.CollectGifts();
						}

						//if configured, search for duelists
						if (duelLinksBotConfig.FarmNpc)
						{
							var foundDuelistsOrItems = TemplatesUtil.SearchForDuelists(currentDuelLinkWorld, currentDuelArea);
							foreach (var duelistsOrItemRectangle in foundDuelistsOrItems)
							{
								TemplatesUtil.CheckForDuelOrItem(duelistsOrItemRectangle);
							}
						}

						//if configured, farm at gate
						if (DuelArea.GATE_AREA == currentDuelArea
						    && duelLinksBotConfig.FarmGate)
						{
							TemplatesUtil.HandleGateDuels(duelLinksBotConfig.GateDuelsInARow, duelLinksBotConfig.GetAllGateConfigs());
						}

						//go to next screen
						ChangeToNextDuelArea();
					}
					catch (DuelLinksRebootDetectedException)
					{
						InitializeBot();
					}
				}
			}
			catch (ThreadAbortException)
			{
				//ignore, bot was stopped
			}
			catch (DuelLinksTemplateTimeoutException te)
			{
				LOGGER.Error("Timeout reached.", te);
				//make screenshot of the app to analyse the error later on a lot easier
				DebugUtils.MakeDuelLinksAppScreenshot();
				//send pushbullet notification to notifiy about error
				var apiKey = duelLinksBotConfig.PushbulletApiToken;
				DebugUtils.SendErrorPushbulletNotification(apiKey, te);
			}
			finally
			{
				LOGGER.Debug(LogEntryType.RUN_INFO, "BotWorkerThread was aborted.");
			}

			// ReSharper disable once FunctionNeverReturns
		}

		private void InitializeBot()
		{
			currentDuelLinkWorld = TemplatesUtil.GetCurrentYuGiOhSerie();

			//go to duel area screen
			TemplatesUtil.EnsureToDisplayDuelArea(DuelArea.GATE_AREA);
			currentDuelArea = DuelArea.GATE_AREA;
		}

		private void ChangeToNextDuelArea()
		{
			currentDuelArea = TemplatesUtil.ChangeToNextDuelArea();
		}
	}
}