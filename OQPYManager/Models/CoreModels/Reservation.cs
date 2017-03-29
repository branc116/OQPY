using OQPYModels.Models.CoreModels;
using System;

namespace OQPYManager.Models.CoreModels
{
    public class Reservation : BaseReservation
    {
        public string Id { get; set; }

        public DateTime StartReservationTime { get; set; }

        public DateTime EndReservationTime { get; set; }

        public Resource Resource { get; set; }

        /// <summary>
        /// Could be just a name and/or surname
        /// </summary>
        public string SecretCode { get; set; }
    }
}