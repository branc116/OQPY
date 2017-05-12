using System.Collections.Generic;
using System.Threading.Tasks;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IReviewDbRepository : IBaseDbRepository<Review>
    {
        /// <summary>
        /// Get all reviews for a venue.
        /// </summary>
        /// <param name="venueId">Id of the venue for which we want reviews loaded.</param>
        /// <returns>Collection of reviews</returns>
        Task<IEnumerable<Review>> GetAllReviews(string venueId);

        /// <summary>
        /// Rates review like on steam products review.
        /// </summary>
        /// <param name="reviewId">Id of the review to be rated</param>
        /// <param name="like"></param>
        Task RateReview(string reviewId, string like);

        /// <summary>
        /// Add new review.
        /// </summary>
        /// <param name="venueId">Id of the venue to be rated</param>
        /// <param name="review">Review of a venue.</param>
        Task AddNewReview(string venueId, Review review);
    }
}