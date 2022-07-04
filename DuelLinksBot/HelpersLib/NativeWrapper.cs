using System;
using System.Drawing;
using System.Drawing.Imaging;
using DuelLinksBot.HelpersLib.Native;

namespace DuelLinksBot.HelpersLib
{
	public static class NativeWrapper
	{
		public static void SetTopMost(IntPtr wndHandle, bool topMost)
		{
			NativeMethods.GetWindowRect(wndHandle, out var rect);
			NativeMethods.SetWindowPos(wndHandle,
				topMost ? NativeConstants.HWND_TOPMOST : NativeConstants.HWND_NOT_TOPMOST, rect.Left, rect.Top,
				rect.Right - rect.Left, rect.Bottom - rect.Top, SetWindowPosFlags.TOPMOST_FLAGS);
		}

		public static Bitmap PrintWindow(IntPtr wndHandle)
		{
			var rect = GetWindowRectangle(wndHandle);
			var bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format48bppRgb);
			using (var gfxBmp = Graphics.FromImage(bmp))
			{
				var hdcBitmap = gfxBmp.GetHdc();
				try
				{
					NativeMethods.PrintWindow(wndHandle, hdcBitmap,
						NativeConstants.PW_CLIENTONLY);
				}
				finally
				{
					gfxBmp.ReleaseHdc(hdcBitmap);
				}
			}

			return bmp;
		}

		private static Rectangle GetWindowRectangle(IntPtr handle)
		{
			var rect = Rectangle.Empty;
			if (NativeMethods.IsDwmEnabled())
			{
				if (NativeMethods.GetExtendedFrameBounds(handle, out var tempRect))
				{
					rect = tempRect;
				}
			}

			if (rect.IsEmpty)
			{
				rect = NativeMethods.GetWindowRect(handle);
			}

			if (!Helpers.IsWindows10OrGreater() && NativeMethods.IsZoomed(handle))
			{
				rect = NativeMethods.MaximizedWindowFix(handle, rect);
			}

			return rect;
		}
	}
}