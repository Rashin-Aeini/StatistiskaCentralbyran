using System.Collections.Generic;

namespace StatistiskaCentralbyran.Models.Domains
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Population> Years { get; set; }
    }
}
