using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class Friendships : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public string UserSendId { get; set; }
        [ForeignKey("UserSendId")]
        public ApplicationUser UserSend { get; set; }

        public string ReceiverId { get; set; }
        //[ForeignKey("ReceiverId")]
        //public ApplicationUser Receiver { get; set; }

        public DateTime RequestDate { get; set; }
        public string Status { get; set; } //Accepted, Pending
    }
}
