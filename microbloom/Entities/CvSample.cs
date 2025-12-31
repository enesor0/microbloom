namespace microbloom.Entities
{
    public class CvSample
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? FileDownloadUrl { get; set; }
        public string? ThumbnailImageUrl { get; set; }
    }
}
