using OQPYModels.Models.CoreModels;
using System.Collections.Generic;

namespace OQPYModels.Models
{
    /// <summary>
    /// I created this class in order to simplify database creation.
    /// </summary>
    public class BaseOwner : BaseApplicationUser
    {
        /// <summary>
        /// Owner can own multiple venues.
        /// </summary>
        public virtual List<BaseVenue> Venues { get; set; }
    }
}