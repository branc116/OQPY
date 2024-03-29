﻿using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    public class Employee: BaseApplicationUser
    {
        /// <summary>
        /// Venue where worker works.
        /// Should we put multiple venues if he works in multiple venues?
        /// note : very unlikely situation altough possible
        /// Better List<Venue>, what if he is a manager, but not the owner and he is managing more venues
        /// </summary>
        public virtual Venue Venue { get; set; }

        public Employee()
        {
        }

        public Employee(string userName) : base(userName)
        {
        }

        public Employee(string userName, Venue workPlace) : base(userName)
        {
            this.Venue = workPlace;
        }

        public static IEnumerable<Employee> RandomEmployees(int n, Venue workPlace)
        {
            return from _ in new string(' ', n)
                   let email = RandomEmail()
                   let userName = RandomName()
                   let name = RandomName()
                   let lastName = RandomName()
                   let employ = new Employee(userName, workPlace)
                   {
                       Email = email,
                       Name = name,
                       Surname = lastName
                   }
                   select employ;
        }
    }
}