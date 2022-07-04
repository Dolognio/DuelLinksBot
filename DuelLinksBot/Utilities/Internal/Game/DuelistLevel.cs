using System.ComponentModel;
using DuelLinksBot.Utilities.Internal.UI;

// ReSharper disable InconsistentNaming

namespace DuelLinksBot.Utilities.Internal.Game
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum DuelistLevel
	{
		[Description("10")]
		Level_10,
		[Description("20")]
		Level_20,
		[Description("30")]
		Level_30,
		[Description("40")]
		Level_40
	}
}