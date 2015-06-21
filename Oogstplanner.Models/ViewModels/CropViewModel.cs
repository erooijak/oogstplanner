namespace Oogstplanner.Models
{
    /// <summary>
    /// View model used for formatting the crop a bit.
    /// </summary>
    public class CropViewModel
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public string Category { get; set; }
        public string AreaPerCrop { get; set; }
        public string AreaPerBag { get; set; }
        public string PricePerBag { get; set; }
        public string SowingMonths { get; set; }
        public string HarvestingMonths { get; set; }
    }
}
