using System;
using System.Collections.Generic;

namespace Zk.Models
{
	public class Calendar
	{
		public User User { get; set; }
		public IList<FarmingMonth> FarmingMonths { get; set; }
	}
}