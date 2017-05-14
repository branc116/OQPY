using System;
using System.Collections.Generic;
using System.Text;

namespace OQPYModels.Models.CoreModels
{
    public class FacebookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Reservation> Reservations { get; set; }
        
        public FacebookUser(string userName)
        {
        }

        public FacebookUser()
        {
        }
    }
}
