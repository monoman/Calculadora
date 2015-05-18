using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using BasicCalculator;

namespace Calculadora
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private readonly CalculatorCore _core = new CalculatorCore();

		public CalculatorCore Core
		{
			get { return _core; }
		} 


		private void button_Click(object sender, RoutedEventArgs e)
		{
			try {
				var button = (FrameworkElement)sender;
				_core.Process((ValidInputs)Enum.Parse(typeof(ValidInputs), (string)button.Tag));
				labelDisplay.Content = _core.ValueToDisplay;
			} catch (Exception ex) {
				Debug.WriteLine(ex);
				labelDisplay.Content = "Error";
			}
		}
	}
}