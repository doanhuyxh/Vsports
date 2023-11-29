using System.ComponentModel.DataAnnotations;

namespace vsports.Models
{
    public class Round:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string RoundName { get; set; }

        public virtual ICollection<MatchScheduleAndResults> MatchScheduleAndResults { get; set;}
    }
}
