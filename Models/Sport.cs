using System.ComponentModel.DataAnnotations;

namespace vsports.Models
{
    public class Sport:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SportClub> SportClubs { get; set; }
        public virtual ICollection<Tournaments> Tournaments { get; set; }
    }
}
