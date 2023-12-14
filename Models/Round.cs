using System.ComponentModel.DataAnnotations;

namespace vsports.Models
{
    public class Round:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string RoundName { get; set; }
        public int SeasonOnTournamentId { get; set; } // rout theo mùa giải
        public virtual ICollection<MatchScheduleAndResults> MatchScheduleAndResults { get; set;}

        public virtual ICollection<Board> Boards { get; set; }
    }
}
