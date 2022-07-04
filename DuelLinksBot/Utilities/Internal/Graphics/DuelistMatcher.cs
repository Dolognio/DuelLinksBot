using System;
using System.Collections.Generic;
using System.Drawing;
using DotImaging;
using DuelLinksBot.Properties;
using DuelLinksBot.Utilities.Internal.Game;
using TemplatePyramid = Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplatePyramid<
	Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplate>;

namespace DuelLinksBot.Utilities.Internal.Graphics
{
	public static class DuelistMatcher
	{
		public static readonly GateDuelist YamiYugi = new GateDuelist("Yami Yugi", Resources.yami_yugi);
		public static readonly GateDuelist Kaiba = new GateDuelist("Kaiba", Resources.kaiba);
		public static readonly GateDuelist Joey = new GateDuelist("Joey", Resources.joey);
		public static readonly GateDuelist Mai = new GateDuelist("Mai", Resources.mai);
		public static readonly GateDuelist Tea = new GateDuelist("Tea", Resources.tea);
		public static readonly GateDuelist Weevil = new GateDuelist("Weevil", Resources.weevil);
		public static readonly GateDuelist Rex = new GateDuelist("Rex", Resources.rex);
		public static readonly GateDuelist Mako = new GateDuelist("Mako", Resources.mako);
		public static readonly GateDuelist BanditKeith = new GateDuelist("Bandit Keith", Resources.bandit_keith);
		public static readonly GateDuelist Ishizu = new GateDuelist("Ishizu", Resources.ishizu);
		public static readonly GateDuelist Odion = new GateDuelist("Odion", Resources.odion);
		public static readonly GateDuelist YugiMuto = new GateDuelist("Yugi Muto", Resources.yugi_muto);
		public static readonly GateDuelist Marik = new GateDuelist("Marik", Resources.marik);
		public static readonly GateDuelist Bakura = new GateDuelist("Bakura", Resources.bakura);
		public static readonly GateDuelist Pegasus = new GateDuelist("Pegasus", Resources.pegasus);
		public static readonly GateDuelist Mokuba = new GateDuelist("Mokuba", Resources.mokuba);
		public static readonly GateDuelist ParadoxBrothers = new GateDuelist("Paradox Bros", Resources.paradox_brothers);
		public static readonly GateDuelist Arkana = new GateDuelist("Arkana", Resources.arkana);
		public static readonly GateDuelist Bones = new GateDuelist("Bones", Resources.bones);

		static DuelistMatcher()
		{
			var templates = new List<TemplatePyramid>
			{
				BitmapToTemplatePyramid("test_colored_1", Resources.test_colored_1),
				BitmapToTemplatePyramid("test_colored_2", Resources.test_colored_2),
				BitmapToTemplatePyramid("test_colored_3", Resources.test_colored_3),
				BitmapToTemplatePyramid("test_colored_4", Resources.test_colored_4),
				BitmapToTemplatePyramid("test_colored_5", Resources.test_colored_5),
				BitmapToTemplatePyramid("test_colored_6", Resources.test_colored_6),
				BitmapToTemplatePyramid("test_colored_7", Resources.test_colored_7),
				BitmapToTemplatePyramid("test_colored_8", Resources.test_colored_8),
				BitmapToTemplatePyramid("test_colored_9", Resources.test_colored_9),
				BitmapToTemplatePyramid("test_colored_10", Resources.test_colored_10),
				BitmapToTemplatePyramid("test_colored_11", Resources.test_colored_11),
				BitmapToTemplatePyramid("test_colored_12", Resources.test_colored_12),
				BitmapToTemplatePyramid("test_colored_13", Resources.test_colored_13),
				BitmapToTemplatePyramid("test_colored_14", Resources.test_colored_14),
				BitmapToTemplatePyramid("test_colored_15", Resources.test_colored_15),
				BitmapToTemplatePyramid("test_colored_16", Resources.test_colored_16),
				BitmapToTemplatePyramid("test_colored_17", Resources.test_colored_17),
				BitmapToTemplatePyramid("test_colored_18", Resources.test_colored_18),
				BitmapToTemplatePyramid("test_colored_19", Resources.test_colored_19),
				BitmapToTemplatePyramid("test_colored_20", Resources.test_colored_20),
				BitmapToTemplatePyramid("test_colored_21", Resources.test_colored_21),
				BitmapToTemplatePyramid("test_colored_22", Resources.test_colored_22),
				BitmapToTemplatePyramid("test_colored_23", Resources.test_colored_23),
				BitmapToTemplatePyramid("test_colored_24", Resources.test_colored_24),
				BitmapToTemplatePyramid("test_colored_25", Resources.test_colored_25),
				BitmapToTemplatePyramid("test_colored_26", Resources.test_colored_26),
				BitmapToTemplatePyramid("test_colored_27", Resources.test_colored_27),
				BitmapToTemplatePyramid("test_colored_28", Resources.test_colored_28),
				BitmapToTemplatePyramid("test_colored_29", Resources.test_colored_29),
				BitmapToTemplatePyramid("test_colored_30", Resources.test_colored_30)
			};

			DuelistTemplates = templates;
		}

		public static List<TemplatePyramid> DuelistTemplates { get; }

		private static TemplatePyramid BitmapToTemplatePyramid(string classLabel, Bitmap resourceImage)
		{
			var bgrImage = resourceImage.ToBgr();
			return TemplatePyramid.CreatePyramid(bgrImage, classLabel);
		}

		public class GateDuelist : TemplatePictures.Template
		{
			internal GateDuelist(string duelistName, Bitmap templateImage, double similarityThreshold = 0.95) : base(
				templateImage,
				similarityThreshold)
			{
				DuelistName = duelistName;
				Level_10_Area = ClickAreas.GateLvl_10;
				Level_20_Area = ClickAreas.GateLvl_20;
				Level_30_Area = ClickAreas.GateLvl_30;
				Level_40_Area = ClickAreas.GateLvl_40;
			}

			public string DuelistName { get; }

			private Rectangle Level_10_Area { get; }
			private Rectangle Level_20_Area { get; }
			private Rectangle Level_30_Area { get; }
			private Rectangle Level_40_Area { get; }

			public Rectangle GetClickAreaForDuelistLevel(DuelistLevel duelistLevel)
			{
				switch (duelistLevel)
				{
					case DuelistLevel.Level_10:
						return Level_10_Area;
					case DuelistLevel.Level_20:
						return Level_20_Area;
					case DuelistLevel.Level_30:
						return Level_30_Area;
					case DuelistLevel.Level_40:
						return Level_40_Area;
					default:
						throw new ArgumentOutOfRangeException(nameof(duelistLevel), duelistLevel, null);
				}
			}
		}
	}
}