using System.Collections.Generic;

namespace BlazorDemo.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }

        public ICollection<Chamber> chamber { get; set; }
    }
}
