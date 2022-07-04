using System.Drawing;

namespace DuelLinksBot.Utilities.Internal.Game
{
	public static class ClickAreas
	{
		/// <summary>
		///  The button to switch between GX and Classic
		/// </summary>
		public static readonly Rectangle SwitchWorldsButton = new Rectangle(23, 241, 20, 20);

		/// <summary>
		///  The area where to click in order to open the gate (to duel against gate duelists)
		/// </summary>
		public static readonly Rectangle OpenGateArea = new Rectangle(330, 310, 60, 60);
		public static readonly Rectangle OpenGiftBox = new Rectangle(328, 112, 25, 25);

		public static readonly Rectangle RightArrow = new Rectangle(522, 369, 20, 36);
		public static readonly Rectangle GateLvl_10 = new Rectangle(113, 507, 45, 25);
		public static readonly Rectangle GateLvl_20 = new Rectangle(204, 507, 45, 25);
		public static readonly Rectangle GateLvl_30 = new Rectangle(295, 507, 45, 25);
		public static readonly Rectangle GateLvl_40 = new Rectangle(386, 507, 45, 25);
		public static readonly Rectangle GateDuelBtn = new Rectangle(183, 635, 180, 30);

		/// <summary>
		///  The area to click if there is anything displayed with no real button, but just needs a click somewhere.
		/// </summary>
		public static readonly Rectangle ClickAnywhereRectangle = new Rectangle(10, 485, 20, 20);

		//StageEvents
		public static readonly Rectangle NormalGateAreaStageEvent = new Rectangle(257, 350, 30, 20);
		public static readonly Rectangle NormalArenaAreaStageEvent = new Rectangle(108, 450, 15, 15);
		public static readonly Rectangle NormalShopAreaStageEvent = new Rectangle(235, 345, 80, 15);
		public static readonly Rectangle NormalCardStudioAreaStageEvent = new Rectangle(180, 570, 15, 30);
		public static readonly Rectangle NormalStartStreetReplay = new Rectangle(245, 465, 30, 20);

		public static readonly Rectangle GxGateAreaStageEvent = new Rectangle(257, 350, 30, 20);
		public static readonly Rectangle GxArenaAreaStageEvent = new Rectangle(92, 555, 15, 35);
		public static readonly Rectangle GxShopAreaStageEvent = new Rectangle(235, 345, 80, 15);
		public static readonly Rectangle GxCardStudioAreaStageEvent = new Rectangle(180, 570, 15, 30);
		public static readonly Rectangle GxStartStreetReplay = new Rectangle(245, 465, 30, 20);
	}
}