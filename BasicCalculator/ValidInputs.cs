using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCalculator
{
	public enum ValidInputs
	{
		None,
		Digit_1, Digit_2, Digit_3, Digit_4, Digit_5, Digit_6, Digit_7, Digit_8, Digit_9, Digit_0, Decimal_Separator,
		Operation_Plus, Operation_Minus, Operation_Multiply, Operation_Divide, Operation_Clear, Operation_ClearAll,
		Operation_Equal, Operation_InvertSign
	}
}
