using System;
using System.Collections.Generic;
using System.Threading;
using DuelLinksBot.Configuration;
using DuelLinksBot.Utilities.Internal.Game;

namespace DuelLinksBot.Utilities.Internal
{
	public static class GameUtils
	{
		private static readonly Random RANDOM = new Random();

		public static GateCharConfigDuelistMapping GetNextGateDuelistToDuelAgainst(
			ref Queue<GateCharConfigDuelistMapping> gateCharConfigs,
			ref List<GateCharConfigDuelistMapping> gateCharsToIgnoreForDuels)
		{
			//get the first (is also the next) gateCharConfig in the queue
			var firstGateCharConfig = gateCharConfigs.Dequeue();
			if (firstGateCharConfig == null)
			{
				return null;
			}

			var currentGateCharConfig = firstGateCharConfig;
			do
			{
				//add the removed gateConfig again at the 'end' (circular behaviour)
				gateCharConfigs.Enqueue(currentGateCharConfig);
				/*
				 * Conditions:
				 * - gate duelist must not be on ignore list (could be there because no keys available)
				 * - repetitions are either set to -1 (endless until no keys anymore) or greater than 0
				 */
				if (!gateCharsToIgnoreForDuels.Contains(currentGateCharConfig)
				    && (currentGateCharConfig.GateCharConfigWrapper.Repetitions == -1 ||
				        currentGateCharConfig.GateCharConfigWrapper.Repetitions > 0))
				{
					return currentGateCharConfig;
				}

				currentGateCharConfig = gateCharConfigs.Dequeue();
			} while (firstGateCharConfig != currentGateCharConfig);

			// no gate duelist found to duel against
			return null;
		}

		public static void SleepBeforeTemplateDoublecheck()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepForTemplateDoubleCheck));
		}

		public static void SleepWhileWaitingForBonusOrDuelist()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepAfterDuelistSearch));
		}

		public static void SleepAfterStageEvent()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepAfterStageEvents));
		}

		public static void SleepAfterClickingSwitchWorldToCheckSerie()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepForCheckingWorld));
		}

		public static void SleepAfterGeneralAction()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepAfterGeneralAction));
		}

		public static void SleepAfterLongUiAction()
		{
			Thread.Sleep(GetRandomNumberAround(AppContext.Instance.DuelLinksBotConfig.SleepAfterLongUiAction));
		}

		private static int GetRandomNumberAround(int midNumber)
		{
			var minValue = midNumber - ProgramConstants.RANDOMIZE_OFFSET;
			var maxValue = midNumber + ProgramConstants.RANDOMIZE_OFFSET;
			return RANDOM.Next(minValue, maxValue);
		}
	}
}