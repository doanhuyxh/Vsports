using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace vsports.Models
{
    public class ClubMember:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int SportClubId { set; get; }
        [ForeignKey("SportClubId")]
        public SportClub SportClub { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]   
        public ApplicationUser User { get; set; }
    }
}
