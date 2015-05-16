using System;
using System.ComponentModel;

namespace Oogstplanner.Models
{
    /// <summary>
    /// Enumeration for the months of the year. Multiple months can be selected.
    /// </summary>
    [Flags]
    public enum Month
    {
        /* 0b000000000000 */
        NotSet      = 0,

        /* 0b000000000001 */
        [Description("Januari")]
        January     = 1,

        /* 0b000000000010 */
        [Description("Februari")]
        February    = 2, 

        /* 0b000000000100 */
        [Description("Maart")]
        March       = 4,

        /* 0b000000001000 */
        [Description("April")]
        April       = 8,

        /* 0b000000010000 */
        [Description("Mei")]
        May         = 16,

        /* 0b000000100000 */
        [Description("Juni")]
        June        = 32,

        /* 0b000001000000 */
        [Description("Juli")]
        July        = 64,  

        /* 0b000010000000 */
        [Description("Augustus")]
        August    = 128,   

        /* 0b000100000000 */
        [Description("September")]
        September   = 256,

        /* 0b001000000000 */
        [Description("Oktober")]
        October     = 512,

        /* 0b010000000000 */
        [Description("November")]
        November    = 1024,

        /* 0b100000000000 */
        [Description("December")]
        December    = 2048
    }
}
