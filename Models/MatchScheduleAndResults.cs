using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class MatchScheduleAndResults : BaseEntity
    {
        [Key] public int Id { get; set; }
        public int SeasonOnTournamentId { get; set; }
        [ForeignKey("SeasonOnTournamentId")]
        public SeasonOnTournaments SeasonOnTournaments { get; set; }
        public int RoundId { get; set; }
        [ForeignKey("RoundId")]
        public Round Round { get; set; }
        public DateTime Schedule { get; set; }

        public int SportClubId_1 { get; set; }
        [ForeignKey("SportClubId_1")]
        public SportClub SportClub1 { get; set; }

        public int SportClubId_2 { get; set; }
        [ForeignKey("SportClubId_2")]
        public SportClub SportClub2 { get; set; }

        public string Status { get; set; } //Pending, Playing, Finished, Cancel
        public string Winner { set; get; } // SportClubId_1 or SportClubId_2
    }
}
