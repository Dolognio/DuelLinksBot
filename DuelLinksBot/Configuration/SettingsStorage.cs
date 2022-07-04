using System.IO;
using SettingsProviderNet;

namespace DuelLinksBot.Configuration
{
	public class SettingsStorage : JsonSettingsStoreBase
	{
		private readonly string folderForSettings;

		public SettingsStorage(string folderForSettings)
		{
			this.folderForSettings = folderForSettings;
		}

		protected override void WriteTextFile(string filename, string fileContents)
		{
			var settingsFolder = folderForSettings;
			if (!Directory.Exists(settingsFolder))
			{
				Directory.CreateDirectory(settingsFolder);
			}

			File.WriteAllText(Path.Combine(settingsFolder, filename), fileContents);
		}

		protected override string ReadTextFile(string filename)
		{
			var path = Path.Combine(folderForSettings, filename);
			return File.Exists(path) ? File.ReadAllText(path) : null;
		}
	}
}