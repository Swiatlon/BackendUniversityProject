namespace Application.Dtos
{
    public class AccountOnlyDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }
}
