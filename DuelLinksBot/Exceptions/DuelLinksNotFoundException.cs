using DuelLinksBot.Configuration;

namespace DuelLinksBot.Exceptions
{
	public class DuelLinksNotFoundException : DuelLinksException
	{
		public DuelLinksNotFoundException() : base("Duel-Links game (window title contains '" +
		                                           ProgramConstants.DUEL_LINKS_PROCESS_FULLNAME +
		                                           "') was not found. Please ensure that Duel-Links is running.")
		{
		}
	}
}