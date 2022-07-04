using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DuelLinksBot.Utilities.Internal.Statistics
{
	public class DuelStatisticEntry : INotifyPropertyChanged, IClearableStatistics
	{
		private List<bool> allDuelResults;
		private List<TimeSpan> allTimeSpans;
		private int amountOfDuels;
		private TimeSpan? averageTimeForDuel;
		private decimal? winRate;

		public DuelStatisticEntry(string description)
		{
			Description = description;
			InitializeStatistics();
		}

		public string Description { get; }

		public int AmountOfDuels
		{
			get => amountOfDuels;
			private set => SetField(ref amountOfDuels, value);
		}

		public TimeSpan? AverageTimeForDuel
		{
			get => averageTimeForDuel;
			private set => SetField(ref averageTimeForDuel, value);
		}

		public decimal? WinRate
		{
			get => winRate;
			private set => SetField(ref winRate, value);
		}

		public void ResetStats()
		{
			InitializeStatistics();
		}

		// boiler-plate
		public event PropertyChangedEventHandler PropertyChanged;

		private void InitializeStatistics()
		{
			allTimeSpans = new List<TimeSpan>();
			allDuelResults = new List<bool>();
			AmountOfDuels = 0;
			AverageTimeForDuel = null;
			WinRate = null;
		}

		public void AddDuelStats(TimeSpan duration, bool duelWon)
		{
			AmountOfDuels++;
			AverageTimeForDuel = UpdateAverageTimeSpan(duration);
			WinRate = UpdateWinRate(duelWon);
		}

		private TimeSpan UpdateAverageTimeSpan(TimeSpan newTimeSpanToAdd)
		{
			allTimeSpans.Add(newTimeSpanToAdd);
			var doubleAverageTicks = allTimeSpans.Average(timeSpan => timeSpan.Ticks);
			var longAverageTicks = Convert.ToInt64(doubleAverageTicks);

			return new TimeSpan(longAverageTicks);
		}

		private decimal? UpdateWinRate(bool newDuelResultToAdd)
		{
			allDuelResults.Add(newDuelResultToAdd);
			var amountOfAllDuels = allDuelResults.Count;
			if (amountOfAllDuels == 0)
			{
				return null;
			}

			var allWonDuels = allDuelResults.Count(duelWon => duelWon);
			return (decimal) allWonDuels / amountOfAllDuels * 100;
		}

		private bool SetField<T>(ref T field, T value,
			[CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}

			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}
}