using System.Drawing;
using DuelLinksBot.Configuration;
using DuelLinksBot.Logging;
using DuelLinksBot.Utilities.Internal.Graphics;
using Quellatalo.Nin.TheHands;

namespace DuelLinksBot.Utilities.Internal
{
	public static class MouseUtil
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(MouseUtil));

		private static readonly MouseHandler MOUSE =
			new MouseHandler {DefaultMouseActionDelay = ProgramConstants.DEFAULT_MOUSE_ACTION_DELAY};

		public static void ClickInRectangle(Point topLeftOffsetOfAppArea, TemplateMatch templateMatch)
		{
			if (templateMatch?.ClickableArea == null)
			{
				LOGGER.DebugFormat(LogEntryType.WARNING, "Mouse should click, but no clickable area was defined.");
				return;
			}

			ClickInRectangle(topLeftOffsetOfAppArea, templateMatch.ClickableArea.Value);
		}

		public static void ClickInRectangle(Point topLeftOffsetOfAppArea, Rectangle clickableRectangle)
		{
			//add offset of game window on screen to clickable rectangle position
			var location = new Point(clickableRectangle.X + topLeftOffsetOfAppArea.X,
				clickableRectangle.Y + topLeftOffsetOfAppArea.Y);
			//and finally click
			MOUSE.ClickWithRandomOffset(location, clickableRectangle.Width,
				clickableRectangle.Height);
			LOGGER.DebugFormat(LogEntryType.OTHER, "Clicked somewhere in rectangle [x={0}, y={1}, w={2}, h={3}]",
				location.X, location.Y, clickableRectangle.Width, clickableRectangle.Height);
		}

		public static Point GetMousePosition(Point topLeftOffsetOfAppArea)
		{
			var positionOnScreen = MOUSE.GetPosition();
			var pointOnGame = new Point(positionOnScreen.X - topLeftOffsetOfAppArea.X,
				positionOnScreen.Y - topLeftOffsetOfAppArea.Y);
			return pointOnGame;
		}
	}
}