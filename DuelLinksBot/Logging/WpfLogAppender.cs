using System;
using System.Windows;
using DuelLinksBot.Configuration;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace DuelLinksBot.Logging
{
	/// <summary>
	///  The appender we are going to bind to for our logging.
	/// </summary>
	public class WpfLogAppender : AppenderSkeleton
	{
		/// <inheritdoc />
		/// <summary>
		///  Append the log information to the notification.
		/// </summary>
		/// <param name="loggingEvent">The log event.</param>
		protected override void Append(LoggingEvent loggingEvent)
		{
			var o = LogicalThreadContext.Properties[ProgramConstants.LOG_ENTRY_TYPE_CONTEXT];
			var type = o is LogEntryType entryType ? entryType : LogEntryType.OTHER;
			var renderedLoggingEvent = RenderLoggingEvent(loggingEvent);
			var logEntry = new LogEntry {Timestamp = loggingEvent.TimeStamp, Message = renderedLoggingEvent, Type = type};
			if (AppContext.Instance.LogEntries == null)
			{
				//maybe null if somthing is logged before initialization of program is finished
				return;
			}

			if (Application.Current != null && Application.Current.Dispatcher != null)
			{
				Application.Current.Dispatcher.BeginInvoke(new Action(() =>
				{
					//reset logentries if too many
					if (AppContext.Instance.LogEntries.Count >= ProgramConstants.MAXIMUM_LOG_ENTRIES)
					{
						AppContext.Instance.LogEntries.Clear();
					}
					AppContext.Instance.LogEntries.Add(logEntry);
				}));
			}
		}
	}
}