namespace FootballSimulator.Core.DTOs
{
    public class UserSearchListItem
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
    }
}
