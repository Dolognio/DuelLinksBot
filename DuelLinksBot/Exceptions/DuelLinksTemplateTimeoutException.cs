namespace DuelLinksBot.Exceptions
{
	public class DuelLinksTemplateTimeoutException : DuelLinksException
	{
		public DuelLinksTemplateTimeoutException() : base("Timeout during waiting for specific templates to be shown.")
		{
		}
	}
}