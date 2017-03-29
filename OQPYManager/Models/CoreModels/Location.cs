using OQPYModels.Models.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace OQPYManager.Models.CoreModels
{
    public class Location : BaseLocation
    {
        [Required]
        public override string Id { get => base.Id; set => base.Id = value; }
    }
}