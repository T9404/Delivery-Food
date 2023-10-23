namespace WebApplication.Models.Requests;

public record SetRatingDishRequest
{
    public int Rating { get; private set; }

    public SetRatingDishRequest(int rating)
    {
        if (rating < 0 || rating > 10)
        {
            throw new Exception("Rating must be between 0 and 10");
        }

        Rating = rating;
    }
}