using System;
using System.Collections.Generic;

namespace Zk.Models
{
	public class FarmingMonth
	{
		public int Id { get; set; }
		public Month Month { get; set; } // A farming month needs to know which month it is.
		public IList<KeyValuePair<Crop, int>> SowingPattern { get; set; }
		public IList<KeyValuePair<Crop, int>> HarvestingPattern { get; set; }
	}       
		
}