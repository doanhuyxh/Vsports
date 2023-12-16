using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class Board:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoundId { get; set; }
        [ForeignKey("RoundId")]
        public Round Round { get; set; }
        public virtual ICollection<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }

    }
}
