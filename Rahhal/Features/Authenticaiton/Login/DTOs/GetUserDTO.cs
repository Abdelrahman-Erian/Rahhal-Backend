namespace Rahhal.Features.Authenticaiton.Login.DTOs
{
    public class GetUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string HashedPassword { get; set; }
        public string SaltPassword { get; set; }

        public string Username { get; set; }


    }
}
