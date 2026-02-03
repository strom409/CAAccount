using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorDemo.Models
{
    public class UnitMaster
    {
        [Key]
        public int id { get; set; }

        public string? Ucode { get; set; }

        public string? UnitName { get; set; }

        public string? Stat { get; set; }   // Assuming it's a bit field in SQL

        public string? details { get; set; }

        // Optional: collection navigation (one UnitMaster → many MixChambers)
        public ICollection<Chamber> chamber { get; set; }

    }
}
