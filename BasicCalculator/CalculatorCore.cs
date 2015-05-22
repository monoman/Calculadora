using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicCalculator
{
	public class CalculatorCore
	{
		public string ValueToDisplay { get { return AdjustTrailingZeroes(_currentValue.ToString()); } }

		public void Process(ValidInputs input)
		{
			switch (input) {
				case ValidInputs.Decimal_Separator:
					if (_placeAfterSeparator == 0) {
						ResetResult();
						_placeAfterSeparator = 1;
					}
					break;

				case ValidInputs.Digit_0:
					CompoundDigit(0);
					break;

				case ValidInputs.Digit_1:
					CompoundDigit(1);
					break;

				case ValidInputs.Digit_2:
					CompoundDigit(2);
					break;

				case ValidInputs.Digit_3:
					CompoundDigit(3);
					break;

				case ValidInputs.Digit_4:
					CompoundDigit(4);
					break;

				case ValidInputs.Digit_5:
					CompoundDigit(5);
					break;

				case ValidInputs.Digit_6:
					CompoundDigit(6);
					break;

				case ValidInputs.Digit_7:
					CompoundDigit(7);
					break;

				case ValidInputs.Digit_8:
					CompoundDigit(8);
					break;

				case ValidInputs.Digit_9:
					CompoundDigit(9);
					break;

				case ValidInputs.Operation_Clear:
					_currentValue = 0;
					break;

				case ValidInputs.Operation_ClearAll:
					_currentValue = 0;
					StackOperation(ValidInputs.None);
					break;

				case ValidInputs.Operation_Plus:
				case ValidInputs.Operation_Multiply:
				case ValidInputs.Operation_Minus:
				case ValidInputs.Operation_Divide:
					PerformPendingOperation();
					StackOperation(input);
					break;

				case ValidInputs.Operation_InvertSign:
					_currentValue = -_currentValue;
					break;

				case ValidInputs.Operation_Equal:
					PerformPendingOperation();
					break;
			}
		}

		private decimal _currentValue = 0m;

		private decimal _lastValue = 0m;

		private ValidInputs _pendingOperation = ValidInputs.None;

		private int _placeAfterSeparator = 0;

		private bool _resetValue = false;

		private static string NumberDecimalSeparator
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			}
		}

		private static string AddNonSignificantZeroes(string baseValue, string decimalSeparator, int neededPlaces)
		{
			var lastSeparator = baseValue.LastIndexOf(decimalSeparator);
			var firstPlaceAfterSeparator = 0;
			if (lastSeparator == -1) {
				baseValue += decimalSeparator;
				firstPlaceAfterSeparator = baseValue.LastIndexOf(decimalSeparator) + decimalSeparator.Length;
			} else
				firstPlaceAfterSeparator = lastSeparator + decimalSeparator.Length;
			var missingZeroes = neededPlaces - (baseValue.Length - firstPlaceAfterSeparator);
			for (int i = missingZeroes; i > 0; i--)
				baseValue += "0";
			return baseValue;
		}

		private static string RemoveNonSignificantZeroes(string baseValue, string decimalSeparator)
		{
			var lastSeparator = baseValue.LastIndexOf(decimalSeparator);
			if (lastSeparator != -1) {
				for (int i = baseValue.Length - 1; i >= lastSeparator; i--)
					if (baseValue[i] != '0') {
						baseValue = baseValue.Substring(0, i + 1);
						break;
					}
				if (baseValue.EndsWith(decimalSeparator))
					baseValue = baseValue.Substring(0, baseValue.Length - decimalSeparator.Length);
			}
			return baseValue;
		}

		private string AdjustTrailingZeroes(string baseValue)
		{
			var decimalSeparator = NumberDecimalSeparator;
			baseValue = RemoveNonSignificantZeroes(baseValue, decimalSeparator);
			if (_placeAfterSeparator <= 0)
				return baseValue;
			if (_placeAfterSeparator == 1)
				return baseValue + decimalSeparator;
			return AddNonSignificantZeroes(baseValue, decimalSeparator, _placeAfterSeparator - 1);
		}

		private void CompoundDigit(int digit)
		{
			ResetResult();
			if (_placeAfterSeparator > 0) {
				_currentValue = _currentValue + Convert.ToDecimal(Math.Pow(10, -_placeAfterSeparator) * digit);
				_placeAfterSeparator++;
			} else
				_currentValue = _currentValue * 10 + digit;
		}

		private void PerformPendingOperation()
		{
			switch (_pendingOperation) {
				case ValidInputs.Operation_Plus:
					_currentValue += _lastValue;
					break;

				case ValidInputs.Operation_Multiply:
					_currentValue *= _lastValue;
					break;

				case ValidInputs.Operation_Minus:
					_currentValue = _lastValue - _currentValue;
					break;

				case ValidInputs.Operation_Divide:
					if (_currentValue != 0m)
						_currentValue = _lastValue / _currentValue;
					break;

				default:
					return;
			}
			_lastValue = 0;
			_placeAfterSeparator = 0;
			_resetValue = true;
			_pendingOperation = ValidInputs.None;
		}

		private void ResetResult()
		{
			if (_resetValue) {
				_currentValue = 0;
				_placeAfterSeparator = 0;
				_resetValue = false;
			}
		}

		private void StackOperation(ValidInputs input)
		{
			_lastValue = _currentValue;
			_pendingOperation = input;
			_resetValue = true;
		}
	}
}