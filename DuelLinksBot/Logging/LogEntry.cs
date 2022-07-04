using System;
using System.Windows.Media;

namespace DuelLinksBot.Logging
{
	public class LogEntry
	{
		public DateTime Timestamp { get; set; }

		public string Message { get; set; }

		public LogEntryType Type
		{
			set
			{
				switch (value)
				{
					case LogEntryType.RUN_INFO:
						TextColor = Brushes.Blue;
						break;
					case LogEntryType.BOT_ACTION:
						TextColor = Brushes.Green;
						break;
					case LogEntryType.ERROR:
						TextColor = Brushes.Red;
						break;
					case LogEntryType.WARNING:
						TextColor = Brushes.Orange;
						break;
					case LogEntryType.OTHER:
						TextColor = Brushes.Black;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(value), value, null);
				}
			}
		}

		public SolidColorBrush TextColor { get; private set; } = Brushes.Black;
	}
}