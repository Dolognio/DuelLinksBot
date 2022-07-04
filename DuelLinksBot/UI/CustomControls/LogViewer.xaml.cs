using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;

namespace DuelLinksBot.UI.CustomControls
{
	public partial class LogViewer
	{
		private bool autoScroll = true;

		public LogViewer()
		{
			InitializeComponent();
		}

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			// User scroll event : set or unset autoscroll mode
			if (e.ExtentHeightChange == 0)
			{
				// Content unchanged : user scroll event
				autoScroll = ((ScrollViewer) e.Source).VerticalOffset == ((ScrollViewer) e.Source).ScrollableHeight;
			}

			// Content scroll event : autoscroll eventually
			if (autoScroll && e.ExtentHeightChange != 0)
			{
				// Content changed and autoscroll mode set
				// Autoscroll
				((ScrollViewer) e.Source).ScrollToVerticalOffset(((ScrollViewer) e.Source).ExtentHeight);
			}
		}
	}
}