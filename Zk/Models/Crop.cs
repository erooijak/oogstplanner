using System;
using System.Web.UI.WebControls;

namespace Zk.Models
{
	public class Crop
	{
		public int Id { get; set; }
		public string Category { get; set; }
		public string Name { get; set; }
		public double AreaPerCrop { get; set; }
		public double AreaPerBag { get; set; }
		public decimal PricePerBag { get; set; }
		public Month SowingMonths { get; set;}
		public Month HarvestingMonths { get; set;}
	}       
}