using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Runtime.CompilerServices;
using System.Text;

namespace $safeprojectname$
{
	public class Class1
	{
		[MethodImpl(MethodImplOptions.ForwardRef)]
		public extern int Square(int number);
	}
}
