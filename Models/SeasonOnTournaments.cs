using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class SeasonOnTournaments:BaseEntity
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public int TournamentsId { get; set; }
        [ForeignKey("TournamentsId")]
        public Tournaments Tournaments { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public string AvatarImage { get; set; }
        public string BackgroudImage { get; set; }
        public string Address { get; set; }
        public string CompetitionForm { get; set; }
        public string SeasonRules { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }

        public int TeamsNumber { get; set; }
        public int MemberOfTeam { get; set; }
        public int numberOfRoundsPerMatch { get; set; }
        public int numberOfRounds { get; set; }
        public int numberOfCoaches { get; set; }
        public int TimeFight { get; set; }
        public int NumberBoard {  get; set; }
        public int NumberTeamOnRoud { get; set; }



        public virtual ICollection<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }
    }
}
