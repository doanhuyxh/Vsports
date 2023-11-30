using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class SportClub : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser Owner { get; set; }
        public string SportsCoach { get; set; } // huấn luyên viên
        public string AvatarImage { get; set; }
        public string BackgroudImage { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ClubRules { get; set; }
        public string Status { get; set; } //public or private
        public int Point { get; set; } //điểm dễ xếp loại nổi bật

        public virtual ICollection<ClubMember> ClubMembers { get; set; } 
        public virtual ICollection<SportClubOnTournaments> SportClubOnTournaments { get; set; } 

    }
}
