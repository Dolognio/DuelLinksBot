using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Accord.Extensions.Imaging.Algorithms.LINE2D;
using DotImaging;
using DuelLinksBot.Configuration;
using DuelLinksBot.Logging;
using DuelLinksBot.Utilities;
using DuelLinksBot.Utilities.Internal.Game;
using DuelLinksBot.Utilities.Internal.Graphics;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using TemplatePyramid = Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplatePyramid<
	Accord.Extensions.Imaging.Algorithms.LINE2D.ImageTemplate>;

namespace DuelLinksBot.DebugHelpers
{
	/// <summary>
	///  For Testing only.
	/// </summary>
	public class DebugUtils
	{
		private static readonly DuelLinksLogger LOGGER = DuelLinksLogManager.GetLogger(typeof(DebugUtils));

		public static void HighlightArea(int x, int y, int w, int h)
		{
			var appArea = TemplatesUtil.GetDuelLinksAppArea();
			try
			{
				using (var brush = new Pen(Color.Red, 2))
				{
					var location = new Point(x, y);
					var size = new Size(w, h);
					var rect = new Rectangle(location, size);
					var subArea = appArea.SubArea(rect);
					subArea.Highlight(brush);
				}
			}
			catch (Exception e)
			{
				LOGGER.Error(e);
			}
		}

		public static void TestFindDuelists(DuelArea area)
		{
			var appArea = TemplatesUtil.GetDuelLinksAppArea();
			appArea = appArea.SubArea(area.SubAreaForNormalWorld);
			using (var displayingImage = appArea.GetDisplayingImage())
			{
				var imageToMatchAgainst = displayingImage.ToBgr();

				try
				{
					var linPyr = LinearizedMapPyramid.CreatePyramid(imageToMatchAgainst); //prepare linear-pyramid maps
					var matches = linPyr.MatchTemplates(DuelistMatcher.DuelistTemplates, 96);

					var matchGroups = new MatchClustering(3).Group(matches.ToArray());
					foreach (var group in matchGroups)
					{
						using (var brush = new Pen(Color.Blue, 2))
						{
							var subArea = appArea.SubArea(group.Representative.BoundingRect.ToRect());
							subArea.Highlight(brush);
						}
					}
				}
				catch (Exception e)
				{
					LOGGER.Error(e);
				}
			}
		}

		public static void MakeDuelLinksAppScreenshot()
		{
			var appArea = TemplatesUtil.GetDuelLinksAppArea();
			using (var displayingImage = appArea.GetDisplayingImage())
			{
				var currentDateTime = $"{DateTime.Now:d_M_yyyy__HH_mm_ss}";
				var errorScreenshot = Path.Combine(ProgramConstants.MY_DOCS_BOT_FOLDER, "error_" + currentDateTime + ".png");
				displayingImage.Save(errorScreenshot, ImageFormat.Png);
			}
		}

		public static void SendErrorPushbulletNotification(string apiKey, Exception exception)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				return;
			}

			var client = new PushbulletClient(apiKey);

			//Get information about the user account behind the API key
			var currentUserInformation = client.CurrentUsersInformation();

			//Check if useraccount data could be retrieved
			if (currentUserInformation == null)
			{
				return;
			}

			//Create request
			var request = new PushNoteRequest
			{
				Email = currentUserInformation.Email,
				Title = "DuelLinksBot Error!",
				Body = exception.ToString()
			};

			client.PushNote(request);
		}
	}
}