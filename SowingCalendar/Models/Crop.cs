using System;
using System.Web.UI.WebControls;

namespace SowingCalendar.Models
{
    public class Crop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Month SowingMonths { get; set;}
    }       
}