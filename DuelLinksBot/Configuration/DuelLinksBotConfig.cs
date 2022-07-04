using System.Collections.Generic;
using System.ComponentModel;
using DuelLinksBot.Utilities.Internal.Game;
using static DuelLinksBot.Utilities.Internal.Graphics.DuelistMatcher;

namespace DuelLinksBot.Configuration
{
	public class DuelLinksBotConfig
	{
		private Queue<GateCharConfigDuelistMapping> allGateConfigMappings;
		private GateCharConfigWrapper banditKeithGateConfig;
		private GateCharConfigWrapper ishizuGateConfig;
		private GateCharConfigWrapper joeyGateConfig;
		private GateCharConfigWrapper maiGateConfig;
		private GateCharConfigWrapper makoGateConfig;
		private GateCharConfigWrapper rexGateConfig;
		private GateCharConfigWrapper setoKaibaGateConfig;
		private GateCharConfigWrapper teaGateConfig;
		private GateCharConfigWrapper weevilGateConfig;
		private GateCharConfigWrapper yamiYugiGateConfig;
		private GateCharConfigWrapper odionGateConfig;
		private GateCharConfigWrapper yugiMutoGateConfig;
		private GateCharConfigWrapper marikGateConfig;
		private GateCharConfigWrapper bakuraGateConfig;
		private GateCharConfigWrapper pegasusGateConfig;
		private GateCharConfigWrapper mokubaGateConfig;
		private GateCharConfigWrapper paradoxBrosGateConfig;
		private GateCharConfigWrapper arkanaGateConfig;
		private GateCharConfigWrapper bonesGateConfig;

		[DefaultValue(false)]
		[DisplayName("Farm Stage-Events")]
		public bool FarmStageEvent { get; set; }

		[DefaultValue(false)]
		[DisplayName("Farm Street-Replay")]
		public bool FarmStreetReplay { get; set; }

		[DefaultValue(false)]
		[Description("Farm NPCs")]
		public bool FarmNpc { get; set; }

		[DefaultValue(false)]
		[Description("Farm Gate")]
		public bool FarmGate { get; set; }

		[DefaultValue(1)]
		[Description("Amount of Gate Duels in a row")]
		public int GateDuelsInARow { get; set; }

		[DefaultValue(false)]
		[Description("Only debugging?")]
		public bool DebugOnly { get; set; }

		[DefaultValue(false)]
		[Description("Receive Gifts")]
		public bool ReceiveGifts { get; set; }

		[DefaultValue("")]
		[Description("API Token to access Pushbullet")]
		public string PushbulletApiToken { get; set; }

		[DefaultValue(400)]
		[Description("Sleep time (ms) for template double-checking")]
		public int SleepForTemplateDoubleCheck { get; set; }

		[DefaultValue(1300)]
		[Description("Sleep time (ms) after duelist search")]
		public int SleepAfterDuelistSearch { get; set; }

		[DefaultValue(3000)]
		[Description("Sleep time (ms) after stage events")]
		public int SleepAfterStageEvents { get; set; }

		[DefaultValue(600)]
		[Description("Sleep time (ms) for checking world")]
		public int SleepForCheckingWorld { get; set; }

		[DefaultValue(500)]
		[Description("Sleep time (ms) after general action")]
		public int SleepAfterGeneralAction { get; set; }

		[DefaultValue(2000)]
		[Description("Sleep time (ms) after long UI action")]
		public int SleepAfterLongUiAction { get; set; }

		public GateCharConfigWrapper YamiYugiGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref yamiYugiGateConfig);
			set => yamiYugiGateConfig = value;
		}

		public GateCharConfigWrapper SetoKaibaGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref setoKaibaGateConfig);
			set => setoKaibaGateConfig = value;
		}

		public GateCharConfigWrapper JoeyGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref joeyGateConfig);
			set => joeyGateConfig = value;
		}

		public GateCharConfigWrapper MaiGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref maiGateConfig);
			set => maiGateConfig = value;
		}

		public GateCharConfigWrapper TeaGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref teaGateConfig);
			set => teaGateConfig = value;
		}

		public GateCharConfigWrapper WeevilGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref weevilGateConfig);
			set => weevilGateConfig = value;
		}

		public GateCharConfigWrapper RexGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref rexGateConfig);
			set => rexGateConfig = value;
		}

		public GateCharConfigWrapper MakoGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref makoGateConfig);
			set => makoGateConfig = value;
		}

		public GateCharConfigWrapper BanditKeithGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref banditKeithGateConfig);
			set => banditKeithGateConfig = value;
		}

		public GateCharConfigWrapper IshizuGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref ishizuGateConfig);
			set => ishizuGateConfig = value;
		}

		public GateCharConfigWrapper OdionGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref odionGateConfig);
			set => odionGateConfig = value;
		}

		public GateCharConfigWrapper YugiMutoGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref yugiMutoGateConfig);
			set => yugiMutoGateConfig = value;
		}

		public GateCharConfigWrapper MarikGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref marikGateConfig);
			set => marikGateConfig = value;
		}

		public GateCharConfigWrapper BakuraGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref bakuraGateConfig);
			set => bakuraGateConfig = value;
		}

		public GateCharConfigWrapper PegasusGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref pegasusGateConfig);
			set => pegasusGateConfig = value;
		}

		public GateCharConfigWrapper MokubaGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref mokubaGateConfig);
			set => mokubaGateConfig = value;
		}

		public GateCharConfigWrapper ParadoxBrosGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref paradoxBrosGateConfig);
			set => paradoxBrosGateConfig = value;
		}

		public GateCharConfigWrapper ArkanaGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref arkanaGateConfig);
			set => arkanaGateConfig = value;
		}

		public GateCharConfigWrapper BonesGateConfig
		{
			get => InitOrGetGateCharPropertyWrapper(ref bonesGateConfig);
			set => bonesGateConfig = value;
		}

		public Queue<GateCharConfigDuelistMapping> GetAllGateConfigs()
		{
			if (allGateConfigMappings == null)
			{
				allGateConfigMappings = new Queue<GateCharConfigDuelistMapping>();
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(YamiYugi, YamiYugiGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Kaiba, SetoKaibaGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Joey, JoeyGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Mai, MaiGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Tea, TeaGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Weevil, WeevilGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Rex, RexGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Mako, MakoGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(BanditKeith, BanditKeithGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Ishizu, IshizuGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Odion, OdionGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(YugiMuto, YugiMutoGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Marik, MarikGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Bakura, BakuraGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Pegasus, PegasusGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Mokuba, MokubaGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(ParadoxBrothers, ParadoxBrosGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Arkana, ArkanaGateConfig));
				allGateConfigMappings.Enqueue(new GateCharConfigDuelistMapping(Bones, BonesGateConfig));
			}

			return allGateConfigMappings;
		}

		private static GateCharConfigWrapper InitOrGetGateCharPropertyWrapper(ref GateCharConfigWrapper propToInitialize)
		{
			return propToInitialize ?? (propToInitialize = new GateCharConfigWrapper());
		}
	}
}