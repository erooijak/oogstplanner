﻿namespace Oogstplanner.Models
{
    /// <summary>
    /// The crop data to calculate certain stuff.
    /// </summary>
    public class Crop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Category { get; set; }
        public int GrowingTime { get; set; }
        public double? AreaPerCrop { get; set; }
        public double? AreaPerBag { get; set; }
        public decimal? PricePerBag { get; set; }
        public Months SowingMonths { get; set;}
        public Months HarvestingMonths { get; set;}
    }       
}
