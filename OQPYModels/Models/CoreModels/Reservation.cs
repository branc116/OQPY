using System;

namespace OQPYModels.Models.CoreModels
{
    public class BaseReservation
    {
        public virtual string Id { get; set; }

        public virtual DateTime StartReservationTime { get; set; }

        public virtual DateTime EndReservationTime { get; set; }
        public virtual BaseResource Resource { get; set; }

        /// <summary>
        /// Could be just a name and/or surname
        /// </summary>
        public virtual string SecretCode { get; set; }
    }
}