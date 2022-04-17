namespace StatistiskaCentralbyran.Models.Domains
{
    public class Population
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public int YearId { get; set; }
        public virtual Year Year { get; set; }
        public Gender Gender { get; set; }
        public int Count { get; set; }
    }
}
