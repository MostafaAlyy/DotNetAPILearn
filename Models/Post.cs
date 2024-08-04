namespace DotNetAPILearn.Models;

public class Post
{
    public required int PostId { get; set; }
    public required int UserId { get; set; }
    public required string PostTitle { get; set; }
    public string PostContent { get; set; }
    public required DateTime PostCreated { get; set; }
    public DateTime PostUpdated { get; set; }

    Post()


    {
        PostContent ??= "";
        PostTitle ??= "";
    }
}