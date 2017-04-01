using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models
{
    public class BaseEmployee : BaseApplicationUser
    {
        /// <summary>
        /// Venue where worker works.
        /// Should we put multiple venues if he works in multiple venues?
        /// note : very unlikely situation altough possible
        /// Better List<Venue>, what if he is a manager, but not the owner and he is managing more venues
        /// </summary>
        public virtual BaseVenue Venue { get; set; }

        public BaseEmployee(string userName) : base(userName)
        {

        }
        public BaseEmployee(string userName, BaseVenue workPlace) : base(userName)
        {
            this.Venue = workPlace;
        }
        public static IEnumerable<BaseEmployee> RandomEmployees(int n, BaseVenue workPlace){
            return from _ in new string(' ', n)
                   let email = RandomEmail()
                   let userName = RandomName()
                   let name = RandomName()
                   let lastName = RandomName()
                   let employ = new BaseEmployee(userName, workPlace)
                   {
                       Email = email,
                       Name = name,
                       Surname = lastName
                   }
                   select employ;
        }
    }
}