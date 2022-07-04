using System;

// ReSharper disable InconsistentNaming

namespace DuelLinksBot.HelpersLib.Native
{
	internal static class NativeConstants
	{
		internal const uint PW_CLIENTONLY = 0x1;

		internal static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		internal static readonly IntPtr HWND_NOT_TOPMOST = new IntPtr(-2);
	}
}