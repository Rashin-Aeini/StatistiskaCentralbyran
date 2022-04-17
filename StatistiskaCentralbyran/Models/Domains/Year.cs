using System.Collections.Generic;

namespace StatistiskaCentralbyran.Models.Domains
{
    public class Year
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public virtual ICollection<Population> Regions { get; set; }
    }
}
