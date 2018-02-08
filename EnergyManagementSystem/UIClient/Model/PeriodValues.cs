using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIClient.Model
{
	public enum PeriodValues:int
	{
		[Description("Last Hour")]
		Last_Hour=0,
		[Description("Today")]
		Today,
		[Description("Last Month")]
		Last_Month,
		[Description("Last four months")]
		Last_4_Month,
		[Description("Last Year")]
		Last_Year
	}
}
