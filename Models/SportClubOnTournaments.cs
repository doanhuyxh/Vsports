using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class SportClubOnTournaments:BaseEntity
    {
        [Key] public int Id { get; set; }
        public int TournamentsId { get; set; }
        [ForeignKey("TournamentsId")]
        public Tournaments Tournaments { get; set; }

        public int SportClubId { get; set; }
        [ForeignKey("SportClubId")]
        public SportClub SportClub { get; set; }
        public int Point {  get; set; }
        public string Status { get; set; } 
    }
}
