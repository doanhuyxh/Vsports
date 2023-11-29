using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace vsports.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public string avatarImage { get; set; } = string.Empty;
        public string backgroudImage { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<SportClub> SportClubs { get; set; }
        public virtual ICollection<ClubMember> ClubMembers { get; set; }
        public virtual ICollection<Friendships> Friendships { get; set; }
    }
}
