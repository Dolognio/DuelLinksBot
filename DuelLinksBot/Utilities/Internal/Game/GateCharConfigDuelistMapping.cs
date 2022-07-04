using DuelLinksBot.Configuration;
using DuelLinksBot.Utilities.Internal.Graphics;

namespace DuelLinksBot.Utilities.Internal.Game
{
	public class GateCharConfigDuelistMapping
	{
		public GateCharConfigDuelistMapping(DuelistMatcher.GateDuelist gateDuelist,
			GateCharConfigWrapper gateCharConfigWrapper)
		{
			GateDuelist = gateDuelist;
			GateCharConfigWrapper = gateCharConfigWrapper;
		}

		public DuelistMatcher.GateDuelist GateDuelist { get; }
		public GateCharConfigWrapper GateCharConfigWrapper { get; }
	}
}