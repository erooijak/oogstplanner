namespace Zk.Models
{
	/// <summary>
	/// 	A farming month consists of the crops that are sowed and harvested in a particular month.
	/// </summary>
	public class FarmingAction
	{
		public int Id { get; set; }
        public Month Month { get; set; }
        public FarmType Action { get; set; }
        public int CropCount { get ; set ; }

        public virtual Crop Crop { get; set; }
        public virtual Calendar Calendar { get; set; }
       
	}       
		
}