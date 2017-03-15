using System;

namespace OQPYManager.Models.CoreModels
{
    public class Reservation
    {
        public DateTime StartReservationTime { get; set; }

        public DateTime EndReservationTime { get; set; }

        public Resource ResourceReserved { get; set; }

        /// <summary>
        /// Could be just a name and/or surname
        /// </summary>
        public string SecretCode { get; set; }
    }
}