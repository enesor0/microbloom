namespace microbloom.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ScoreType { get; set; }
        public double LastYearBaseScore { get; set; }
        public int LastYearBaseRanking { get; set; }
        public int UniversityId { get; set; }
        public virtual University? University { get; set; }
    }
}