using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DuelLinksBot.HelpersLib.Native
{
	internal static partial class NativeMethods
	{
		private static bool GetBorderSize(IntPtr handle, out Size size)
		{
			var wi = new WINDOWINFO();

			var result = GetWindowInfo(handle, ref wi);

			size = result ? new Size((int) wi.cxWindowBorders, (int) wi.cyWindowBorders) : Size.Empty;

			return result;
		}

		internal static bool IsDwmEnabled()
		{
			return Helpers.IsWindowsVistaOrGreater() && DwmIsCompositionEnabled();
		}

		internal static bool GetExtendedFrameBounds(IntPtr handle, out Rectangle rectangle)
		{
			var result = DwmGetWindowAttribute(handle, (int) DwmWindowAttribute.ExtendedFrameBounds, out RECT rect,
				Marshal.SizeOf(typeof(RECT)));
			rectangle = rect;
			return result == 0;
		}

		internal static Rectangle GetWindowRect(IntPtr handle)
		{
			GetWindowRect(handle, out var rect);
			return rect;
		}

		internal static Rectangle MaximizedWindowFix(IntPtr handle, Rectangle windowRect)
		{
			if (GetBorderSize(handle, out var size))
			{
				windowRect = new Rectangle(windowRect.X + size.Width, windowRect.Y + size.Height,
					windowRect.Width - size.Width * 2, windowRect.Height - size.Height * 2);
			}

			return windowRect;
		}
	}
}