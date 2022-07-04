using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DuelLinksBot.Utilities.Internal.Game;

namespace DuelLinksBot.Configuration
{
	public class GateCharConfigWrapper : INotifyPropertyChanged
	{
		private DuelistLevel duelistLevel = DuelistLevel.Level_10;
		private bool enabled;
		private int repetitions;

		public bool Enabled
		{
			get => enabled;
			set => SetField(ref enabled, value);
		}

		public DuelistLevel DuelistLevel
		{
			get => duelistLevel;
			set => SetField(ref duelistLevel, value);
		}

		public int Repetitions
		{
			get => repetitions;
			set => SetField(ref repetitions, value);
		}

		// boiler-plate
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// ReSharper disable once UnusedMethodReturnValue.Local
		private bool SetField<T>(ref T field, T value,
			[CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}

			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}