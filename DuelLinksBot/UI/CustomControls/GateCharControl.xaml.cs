using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DuelLinksBot.Configuration;

namespace DuelLinksBot.UI.CustomControls
{
	public partial class GateCharControl
	{
		public static readonly DependencyProperty GateCharConfigProperty = DependencyProperty.Register(
			"GateCharConfig", typeof(GateCharConfigWrapper), typeof(GateCharControl),
			new PropertyMetadata(default(GateCharConfigWrapper)));

		public static readonly DependencyProperty DuelistProperty = DependencyProperty.Register(
			"Duelist", typeof(string), typeof(GateCharControl), new PropertyMetadata(default(string)));

		public GateCharControl()
		{
			InitializeComponent();
			((FrameworkElement) Content).DataContext = this;
		}

		public GateCharConfigWrapper GateCharConfig
		{
			get => (GateCharConfigWrapper) GetValue(GateCharConfigProperty);
			set => SetValueDp(GateCharConfigProperty, value);
		}

		public string Duelist
		{
			get => (string) GetValue(DuelistProperty);
			set => SetValueDp(DuelistProperty, value);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void SetValueDp(DependencyProperty property, object value, [CallerMemberName] string p = null)
		{
			SetValue(property, value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
		}
	}
}