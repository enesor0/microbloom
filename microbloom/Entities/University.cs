namespace microbloom.Entities
{
    public class University
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string City { get; set; }
        public bool IsStateUniversity { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebSite { get; set; }
        public virtual ICollection<Department>? Departments { get; set; }
    }
}