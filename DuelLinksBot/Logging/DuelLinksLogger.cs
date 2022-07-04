using System;
using log4net;
using static DuelLinksBot.Configuration.ProgramConstants;

namespace DuelLinksBot.Logging
{
	public class DuelLinksLogger
	{
		private readonly ILog delegatedLogger;

		public DuelLinksLogger(ILog logger)
		{
			delegatedLogger = logger;
		}

		public void Debug(object message)
		{
			Debug(LogEntryType.OTHER, message);
		}

		public void Debug(LogEntryType logEntryType, object message)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.Debug(message);
		}

		public void DebugFormat(LogEntryType logEntryType, string format, params object[] args)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.DebugFormat(format, args);
		}

		public void Info(object message)
		{
			Info(LogEntryType.OTHER, message);
		}

		public void Info(LogEntryType logEntryType, object message)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.Info(message);
		}

		public void InfoFormat(LogEntryType logEntryType, string format, params object[] args)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.InfoFormat(format, args);
		}

		public void Warn(object message)
		{
			Warn(LogEntryType.OTHER, message);
		}

		public void Warn(LogEntryType logEntryType, object message)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.Warn(message);
		}

		public void WarnFormat(LogEntryType logEntryType, string format, params object[] args)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.WarnFormat(format, args);
		}

		public void Error(object message)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = LogEntryType.ERROR;
			delegatedLogger.Error(message);
		}

		public void Error(object message, Exception exception)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = LogEntryType.ERROR;
			delegatedLogger.Error(message, exception);
		}

		public void ErrorFormat(string format, params object[] args)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = LogEntryType.ERROR;
			delegatedLogger.ErrorFormat(format, args);
		}

		public void Fatal(object message)
		{
			Fatal(LogEntryType.OTHER, message);
		}

		public void Fatal(LogEntryType logEntryType, object message)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.Fatal(message);
		}

		public void Fatal(LogEntryType logEntryType, object message, Exception exception)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.Fatal(message, exception);
		}

		public void FatalFormat(LogEntryType logEntryType, string format, params object[] args)
		{
			LogicalThreadContext.Properties[LOG_ENTRY_TYPE_CONTEXT] = logEntryType;
			delegatedLogger.FatalFormat(format, args);
		}
	}
}