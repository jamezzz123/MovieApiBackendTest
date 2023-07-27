namespace MovieApi.Models;

public class MovieItem
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Year { get; set; }
    public string? imdbID { get; set; }
    public string? Type { get; set; }
    public string? Poster { get; set; }
}