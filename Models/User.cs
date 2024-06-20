namespace DotNetAPILearn.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Gender { get; set; }
        public required string Active { get; set; }

    }
}