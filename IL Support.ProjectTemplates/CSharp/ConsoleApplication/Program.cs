using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Runtime.CompilerServices;
using System.Text;

namespace $safeprojectname$
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern int Square(int number);
	}
}
