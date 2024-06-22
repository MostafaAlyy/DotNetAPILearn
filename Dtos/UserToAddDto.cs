namespace DotNetAPILearn.Dtos
{
    public class UserToAddDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Gender { get; set; }
        public required bool Active { get; set; }

    }
}