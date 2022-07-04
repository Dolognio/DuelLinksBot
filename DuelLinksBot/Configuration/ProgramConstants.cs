using System;
using System.IO;

namespace DuelLinksBot.Configuration
{
	public static class ProgramConstants
	{
		public const string BOT_NAME = "DuelLinksBot";
		public const string DUEL_LINKS_WINDOW_NAME = "DUEL LINKS";
		public const string DUEL_LINKS_PROCESS_FULLNAME = DUEL_LINKS_WINDOW_NAME + ".exe";

		public const string LOG_ENTRY_TYPE_CONTEXT = "LogEntryTypeContext";
		public const int MAXIMUM_LOG_ENTRIES = 5000;
		public const int DEFAULT_MOUSE_ACTION_DELAY = 100;
		public const int MAX_RETRIES_WHEN_WAITING_FOR_TEMPLATES = 30;
		public const int RANDOMIZE_OFFSET = 30;

		public static readonly string MY_DOCS_BOT_FOLDER =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), BOT_NAME);
	}
}