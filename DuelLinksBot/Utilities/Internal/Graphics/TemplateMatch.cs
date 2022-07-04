using System.Drawing;

namespace DuelLinksBot.Utilities.Internal.Graphics
{
	public class TemplateMatch
	{
		public TemplateMatch(Rectangle? clickableArea, TemplatePictures.Template foundTemplate)
		{
			ClickableArea = clickableArea;
			FoundTemplate = foundTemplate;
		}

		public Rectangle? ClickableArea { get; }

		public TemplatePictures.Template FoundTemplate { get; }
	}
}