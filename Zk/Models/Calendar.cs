using System;
using System.Collections.Generic;

namespace Zk.Models
{
	public class Calendar
	{
		public User User { get; set; }
		public FarmingMonth January { get; set; }
		public FarmingMonth February { get; set; }
		public FarmingMonth March { get; set; }
		public FarmingMonth April { get; set; }
		public FarmingMonth May { get; set; }
		public FarmingMonth June { get; set; }
		public FarmingMonth July { get; set; }
		public FarmingMonth August { get; set; }
		public FarmingMonth September { get; set; }
		public FarmingMonth October { get; set; }
		public FarmingMonth November { get; set; }
		public FarmingMonth December { get; set; }

	}
}