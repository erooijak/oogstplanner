using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Zk.Models
{
	/// <summary>
	/// 	A farming month consists of the crops that are sowed and harvested in a particular month.
	///     The sowing and harvesting is stored as a dictionary (a list of key-value pairs) where the
	///     key is the specific crop and the value the amount that needs to be harvested or sowed. 
	///     For sowing this dictionary is called SowingPattern and for harvesting HarvestingPattern.
	///     The dictionaries are stored as JSON (JavaScript Object Notation) in the database.
	/// 	To convert dictionaries to and from JSON helper methods are used.
	/// <example>
	///  	A user has a farming month where two broccoli's and one tomato is sowed and nothing is
	///  	harvested. Assume broccoli has an CropID of 1000 and tomato of 2000. Then the 
	///     SowingPattern is stored in JSON format like { 1000: 2, 2000: 1}.
	/// </example> 
	/// </summary>
	public class FarmingMonth
	{
		public int Id { get; set; }
		public Month Month { get; set; } // A farming month needs to know which month it is.

		// Sowing and harvesting patterns are stored as JSON in the database and not mapped with Entity Framework
		[NotMapped]
		public IDictionary<Crop, int> SowingPattern { get; set; }

		[NotMapped]
		public IDictionary<Crop, int> HarvestingPattern { get; set; }

		public string SowingPatternAsJson
		{
			get { return ToJson(SowingPattern); }
			set { SowingPattern = FromJson(value); }
		}

		public string HarvestingPatternAsJson
		{
			get { return ToJson(HarvestingPattern); }
			set { HarvestingPattern = FromJson(value); }
		}

		// Helper methods to convert to and from JSON.
		// See: http://stackoverflow.com/questions/8973027/ef-code-first-map-dictionary-or-custom-type-as-an-nvarchar#8973093

		private static string ToJson(IDictionary<Crop, int> pattern)
		{
			throw new NotImplementedException ();
		}

		private static IDictionary<Crop, int> FromJson(string json)
		{
			throw new NotImplementedException();
		}
	}       
		
}