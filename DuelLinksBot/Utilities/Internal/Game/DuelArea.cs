using System.Drawing;
using DuelLinksBot.Utilities.Internal.Graphics;

namespace DuelLinksBot.Utilities.Internal.Game
{
	public class DuelArea
	{
		public static readonly DuelArea GATE_AREA =
			new DuelArea("Gate", TemplatePictures.GateAreaBtn, TemplatePictures.GateAreaSelectedBtn,
				new Rectangle(40, 400, 390, 260),
				new Rectangle(40, 400, 390, 260),
				ClickAreas.NormalGateAreaStageEvent,
				ClickAreas.GxGateAreaStageEvent);

		public static readonly DuelArea ARENA_AREA =
			new DuelArea("Arena", TemplatePictures.ArenaAreaBtn, TemplatePictures.ArenaAreaSelectedBtn,
				new Rectangle(40, 400, 385, 260),
				new Rectangle(120, 400, 310, 260),
				ClickAreas.NormalArenaAreaStageEvent,
				ClickAreas.GxArenaAreaStageEvent);

		public static readonly DuelArea SHOP_AREA =
			new DuelArea("Shop", TemplatePictures.ShopAreaBtn, TemplatePictures.ShopAreaSelectedBtn,
				new Rectangle(40, 405, 390, 230),
				new Rectangle(40, 405, 390, 230),
				ClickAreas.NormalShopAreaStageEvent,
				ClickAreas.GxShopAreaStageEvent);

		public static readonly DuelArea CARD_STUDIO_AREA =
			new DuelArea("CardStudio", TemplatePictures.CardStudioAreaBtn, TemplatePictures.CardStudioAreaSelectedBtn,
				new Rectangle(70, 410, 365, 200),
				new Rectangle(170, 430, 270, 170),
				ClickAreas.NormalCardStudioAreaStageEvent,
				ClickAreas.GxCardStudioAreaStageEvent);

		private DuelArea(string identifier, TemplatePictures.Template areaBtnTemplate,
			TemplatePictures.Template areaBtnSelectedTemplate,
			Rectangle duelistsSubAreaRectangleForNormalWorld,
			Rectangle duelistsSubAreaRectangleForGxWorld,
			Rectangle stageEventAreaForNormalWorld,
			Rectangle stageEventAreaForGxWorld)
		{
			NameOfArea = identifier;
			AreaButtonTemplate = areaBtnTemplate;
			AreaButtonSelectedTemplate = areaBtnSelectedTemplate;
			SubAreaForNormalWorld = duelistsSubAreaRectangleForNormalWorld;
			SubAreaForGxWorld = duelistsSubAreaRectangleForGxWorld;
			StageEventAreaForNormalWorld = stageEventAreaForNormalWorld;
			StageEventAreaForGxWorld = stageEventAreaForGxWorld;
		}

		public string NameOfArea { get; }

		public TemplatePictures.Template AreaButtonTemplate { get; }

		public TemplatePictures.Template AreaButtonSelectedTemplate { get; }

		public Rectangle SubAreaForNormalWorld { get; }

		public Rectangle SubAreaForGxWorld { get; }
		public Rectangle StageEventAreaForNormalWorld { get; }
		public Rectangle StageEventAreaForGxWorld { get; }

		public Rectangle SubAreaForGivenDuelLinksWorld(DuelLinkWorld duelLinkWorld)
		{
			return DuelLinkWorld.Normal.Equals(duelLinkWorld) ? SubAreaForNormalWorld : SubAreaForGxWorld;
		}

		public Rectangle StageEventAreaForGivenDuelLinksWorld(DuelLinkWorld duelLinkWorld)
		{
			return DuelLinkWorld.Normal.Equals(duelLinkWorld) ? StageEventAreaForNormalWorld : StageEventAreaForGxWorld;
		}
	}
}