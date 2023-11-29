using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class Tournaments:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Point {  get; set; }
        [ForeignKey("SportId")]
        public int SportId { get; set; }
        public Sport Sport { get; set; }

        public virtual ICollection<SportClubOnTournaments> SportClubOnTournaments { get;}
        public virtual ICollection<SeasonOnTournaments> SeasonOnTournaments { get;}
    }
}
