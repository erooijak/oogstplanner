using System;

namespace Zk
{

    /// <summary>
    ///     Enumeration for the months of the year. Multiple months can be selected.
    /// </summary>
    [Flags]
    public enum Month
    {
        NotSet      = 0,        /* 0b000000000000 */
        Januari     = 1,        /* 0b000000000001 */
        Februari    = 2,        /* 0b000000000010 */
        Maart       = 4,        /* 0b000000000100 */
        April       = 8,        /* 0b000000001000 */
        Mei         = 16,       /* 0b000000010000 */
        Juni        = 32,       /* 0b000000100000 */    
        Juli        = 64,       /* 0b000001000000 */        
        Augustus    = 128,      /* 0b000010000000 */    
        September   = 256,      /* 0b000100000000 */
        Oktober     = 512,      /* 0b001000000000 */
        November    = 1024,     /* 0b010000000000 */
        December    = 2048      /* 0b100000000000 */
    }

    /// <summary>
    ///     Enumeration for the type of farming
    /// </summary>
    public enum ActionType
    {
        Harvesting = 1,
        Sowing = 2
    }

}