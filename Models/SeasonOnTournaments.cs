using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class SeasonOnTournaments:BaseEntity
    {
        [Key] public int Id { get; set; }
        public int TournamentsId { get; set; }
        [ForeignKey("TournamentsId")]
        public Tournaments Tournaments { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public string Address { get; set; }
        public string CompetitionForm { get; set; }
        public string SeasonRules { get; set; }

        public virtual ICollection<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }
    }
}
