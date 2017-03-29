using OQPYModels.Models.CoreModels;
namespace OQPYModels
{
    public interface IApplicationUser
    {

        string Name { get; set; }
        string Surname { get; set; }
        BaseVenue Venue { get; set; }
    }
}
