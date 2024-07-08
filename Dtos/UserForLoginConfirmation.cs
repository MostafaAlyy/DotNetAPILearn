namespace DotNetAPILearn.Data
{
    public class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        UserForLoginConfirmationDto()
        {
            PasswordHash ??= [];
            PasswordSalt ??= [];
        }
    }
}