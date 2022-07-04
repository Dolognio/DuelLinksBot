using System;
using log4net;

namespace DuelLinksBot.Logging
{
	public static class DuelLinksLogManager
	{
		public static DuelLinksLogger GetLogger(Type type)
		{
			var logger = LogManager.GetLogger(type);
			return new DuelLinksLogger(logger);
		}
	}
}