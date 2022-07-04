namespace DuelLinksBot.Exceptions
{
	public class DuelLinksRebootDetectedException : DuelLinksException
	{
		public DuelLinksRebootDetectedException() : base("Reboot detected.")
		{
		}
	}
}