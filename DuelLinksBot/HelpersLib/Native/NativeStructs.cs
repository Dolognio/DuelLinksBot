using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DuelLinksBot.HelpersLib.Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct RECT
	{
		public int Left, Top, Right, Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
		{
		}

		public int X
		{
			get => Left;
			set
			{
				Right -= Left - value;
				Left = value;
			}
		}

		public int Y
		{
			get => Top;
			set
			{
				Bottom -= Top - value;
				Top = value;
			}
		}

		public int Width
		{
			get => Right - Left;
			set => Right = value + Left;
		}

		public int Height
		{
			get => Bottom - Top;
			set => Bottom = value + Top;
		}

		public Point Location
		{
			get => new Point(Left, Top);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		public Size Size
		{
			get => new Size(Width, Height);
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public static implicit operator Rectangle(RECT r)
		{
			return new Rectangle(r.Left, r.Top, r.Width, r.Height);
		}

		public static implicit operator RECT(Rectangle r)
		{
			return new RECT(r);
		}

		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}

		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}

		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}

		public override bool Equals(object obj)
		{
			if (obj is RECT)
			{
				return Equals((RECT) obj);
			}

			if (obj is Rectangle)
			{
				return Equals(new RECT((Rectangle) obj));
			}

			return false;
		}

		public override int GetHashCode()
		{
			return ((Rectangle) this).GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top,
				Right, Bottom);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct SIZE
	{
		public int Width;
		public int Height;

		public SIZE(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public static explicit operator Size(SIZE s)
		{
			return new Size(s.Width, s.Height);
		}

		public static explicit operator SIZE(Size s)
		{
			return new SIZE(s.Width, s.Height);
		}

		public override string ToString()
		{
			return string.Format("{0}x{1}", Width, Height);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct POINT
	{
		public int X;
		public int Y;

		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static explicit operator Point(POINT p)
		{
			return new Point(p.X, p.Y);
		}

		public static explicit operator POINT(Point p)
		{
			return new POINT(p.X, p.Y);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct WINDOWINFO
	{
		public uint cbSize;
		public RECT rcWindow;
		public RECT rcClient;
		public uint dwStyle;
		public uint dwExStyle;
		public uint dwWindowStatus;
		public uint cxWindowBorders;
		public uint cyWindowBorders;
		public ushort atomWindowType;
		public ushort wCreatorVersion;

		public WINDOWINFO(bool? filler) :
			this() // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
		{
			cbSize = (uint) Marshal.SizeOf(typeof(WINDOWINFO));
		}
	}

	/// <summary>
	///  Structure, which contains information about a stream and how it is compressed and saved.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct AVICOMPRESSOPTIONS
	{
		/// <summary>
		///  Four-character code indicating the stream type.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int type;

		/// <summary>
		///  Four-character code for the compressor handler that will compress this video stream when it is saved.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int handler;

		/// <summary>
		///  Maximum period between video key frames.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int keyFrameEvery;

		/// <summary>
		///  Quality value passed to a video compressor.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int quality;

		/// <summary>
		///  Video compressor data rate.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int bytesPerSecond;

		/// <summary>
		///  Flags used for compression.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int flags;

		/// <summary>
		///  Pointer to a structure defining the data format.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int format;

		/// <summary>
		///  Size, in bytes, of the data referenced by <b>format</b>.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int formatSize;

		/// <summary>
		///  Video-compressor-specific data; used internally.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int parameters;

		/// <summary>
		///  Size, in bytes, of the data referenced by <b>parameters</b>.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int parametersSize;

		/// <summary>
		///  Interleave factor for interspersing stream data with data from the first stream.
		/// </summary>
		[MarshalAs(UnmanagedType.I4)] public int interleaveEvery;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct BITMAPFILEHEADER
	{
		public static readonly short BM = 0x4d42;
		public short bfType;
		public int bfSize;
		public short bfReserved1;
		public short bfReserved2;
		public int bfOffBits;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct BITMAPINFOHEADER
	{
		[FieldOffset(0)] public uint biSize;
		[FieldOffset(4)] public int biWidth;
		[FieldOffset(8)] public int biHeight;
		[FieldOffset(12)] public ushort biPlanes;
		[FieldOffset(14)] public ushort biBitCount;
		[FieldOffset(16)] public BI_COMPRESSION biCompression;
		[FieldOffset(20)] public uint biSizeImage;
		[FieldOffset(24)] public int biXPelsPerMeter;
		[FieldOffset(28)] public int biYPelsPerMeter;
		[FieldOffset(32)] public uint biClrUsed;
		[FieldOffset(36)] public uint biClrImportant;
		[FieldOffset(40)] public uint bV5RedMask;
		[FieldOffset(44)] public uint bV5GreenMask;
		[FieldOffset(48)] public uint bV5BlueMask;
		[FieldOffset(52)] public uint bV5AlphaMask;
		[FieldOffset(56)] public uint bV5CSType;
		[FieldOffset(60)] public CIEXYZTRIPLE bV5Endpoints;
		[FieldOffset(96)] public uint bV5GammaRed;
		[FieldOffset(100)] public uint bV5GammaGreen;
		[FieldOffset(104)] public uint bV5GammaBlue;
		[FieldOffset(108)] public uint bV5Intent;
		[FieldOffset(112)] public uint bV5ProfileData;
		[FieldOffset(116)] public uint bV5ProfileSize;
		[FieldOffset(120)] public uint bV5Reserved;

		public const int DIB_RGB_COLORS = 0;

		public BITMAPINFOHEADER(int width, int height, ushort bpp)
		{
			biSize = (uint) Marshal.SizeOf(typeof(BITMAPINFOHEADER));
			biPlanes = 1;
			biCompression = BI_COMPRESSION.BI_RGB;
			biWidth = width;
			biHeight = height;
			biBitCount = bpp;
			biSizeImage = (uint) (width * height * (bpp >> 3));
			biXPelsPerMeter = 0;
			biYPelsPerMeter = 0;
			biClrUsed = 0;
			biClrImportant = 0;
			bV5RedMask = (uint) 255 << 16;
			bV5GreenMask = (uint) 255 << 8;
			bV5BlueMask = 255;
			bV5AlphaMask = (uint) 255 << 24;
			bV5CSType = 1934772034;
			bV5Endpoints = new CIEXYZTRIPLE();
			bV5Endpoints.ciexyzBlue = new CIEXYZ(0);
			bV5Endpoints.ciexyzGreen = new CIEXYZ(0);
			bV5Endpoints.ciexyzRed = new CIEXYZ(0);
			bV5GammaRed = 0;
			bV5GammaGreen = 0;
			bV5GammaBlue = 0;
			bV5Intent = 4;
			bV5ProfileData = 0;
			bV5ProfileSize = 0;
			bV5Reserved = 0;
		}

		public uint OffsetToPixels
		{
			get
			{
				if (biCompression == BI_COMPRESSION.BI_BITFIELDS)
				{
					return biSize + 3 * 4;
				}

				return biSize;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct CIEXYZ
	{
		public uint ciexyzX;
		public uint ciexyzY;
		public uint ciexyzZ;

		public CIEXYZ(uint FXPT2DOT30)
		{
			ciexyzX = FXPT2DOT30;
			ciexyzY = FXPT2DOT30;
			ciexyzZ = FXPT2DOT30;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct CIEXYZTRIPLE
	{
		public CIEXYZ ciexyzRed;
		public CIEXYZ ciexyzGreen;
		public CIEXYZ ciexyzBlue;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct InputUnion
	{
		[FieldOffset(0)] public MOUSEINPUT Mouse;

		[FieldOffset(0)] public KEYBDINPUT Keyboard;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct MOUSEINPUT
	{
		public int dx;
		public int dy;
		public uint mouseData;
		public MouseEventFlags dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct KEYBDINPUT
	{
		public VirtualKeyCode wVk;
		public ushort wScan;
		public KeyboardEventFlags dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}
}