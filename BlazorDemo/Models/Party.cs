using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorDemo.Models
{
    //public class Grower
    //{
    //    public int Id { get; set; }
    //    public string GrowerName { get; set; }

    //    public ICollection<Allocation> Allocations { get; set; }
    //    public ICollection<SubGrower> SubGrowers { get; set; }

    //    public Agreement Agreement { get; set; }
    //}
    /// <summary>
    /// [Table("party")]
    /// </summary>
    public class Party
    {
        [Key]
        public int partyid { get; set; }

        [Column("partyname")]
        public string? PartyName { get; set; }

      
        public Agreement? Agreement { get; set; }

        public ICollection<Allocation> Allocation { get; set; }
        public ICollection<SubGrower> SubGrowers { get; set; }
    }

}
