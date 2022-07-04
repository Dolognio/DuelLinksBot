using System.Drawing;
using System.Windows;
using DuelLinksBot.HelpersLib;
using Quellatalo.Nin.TheEyes;
using Point = System.Drawing.Point;

namespace DuelLinksBot.Utilities.Internal.Game
{
	public class DuelLinksAppArea
	{
		private readonly Area delegatedArea;

		public DuelLinksAppArea(Area appArea)
		{
			delegatedArea = appArea;
		}

		public Point TopLeft()
		{
			return delegatedArea.TopLeft;
		}

		public Bitmap GetDisplayingImage()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				AppContext.Instance.DuelLinksApp.ToFront();
				NativeWrapper.SetTopMost(AppContext.Instance.DuelLinksApp.Process.MainWindowHandle, true);
			});
			return delegatedArea.GetDisplayingImage();
		}

		public DuelLinksAppArea SubArea(Rectangle rectangle)
		{
			var subArea = delegatedArea.SubArea(rectangle);
			return new DuelLinksAppArea(subArea);
		}

		public void Highlight(Pen pen)
		{
			delegatedArea.Highlight(pen);
		}

		public Match Find(Pattern pattern)
		{
			return delegatedArea.Find(pattern);
		}
	}
}