using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DuelLinksBot.Utilities.Internal.Statistics
{
	public class OtherStatisticEntry : INotifyPropertyChanged, IClearableStatistics
	{
		private int amountOfNetworkErrors;
		private int amountOfOtherErrors;
		private DateTime? botStartTime;

		public OtherStatisticEntry()
		{
			InitializeStats();
		}

		private void InitializeStats()
		{
			AmountOfNetworkErrors = 0;
			AmountOfOtherErrors = 0;
			BotStartTime = null;
		}

		public int AmountOfNetworkErrors
		{
			get => amountOfNetworkErrors;
			private set => SetField(ref amountOfNetworkErrors, value);
		}

		public int AmountOfOtherErrors
		{
			get => amountOfOtherErrors;
			private set => SetField(ref amountOfOtherErrors, value);
		}

		public DateTime? BotStartTime
		{
			get => botStartTime;
			private set => SetField(ref botStartTime, value);
		}

		public void ResetStats()
		{
			InitializeStats();
			BotStartTime = DateTime.Now;
		}

		public void IncrementNetworkErrorCounter()
		{
			AmountOfNetworkErrors++;
		}

		public void IncrementOtherErrorCounter()
		{
			AmountOfOtherErrors++;
		}

		// boiler-plate
		public event PropertyChangedEventHandler PropertyChanged;

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