using System;

namespace DuelLinksBot.Exceptions
{
	public class DuelLinksException : Exception
	{
		protected DuelLinksException(string message) : base(message)
		{
		}

		public DuelLinksException(string message, Exception ex) : base(message, ex)
		{
		}
	}
}