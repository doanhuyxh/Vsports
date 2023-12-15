using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models
{
    public class GhiBan:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int MatchScheduleAndResultId { get; set; }
        [ForeignKey("MatchScheduleAndResultId")]
        public MatchScheduleAndResults matchScheduleAndResults { get; set; }

        public int SportClubId { get; set; }
        public string playerGhiBan { get; set; }
        public string playerKienTao { get; set; }
        public string time { get; set; }
        public string LoaiBanThang { get; set; } // Phản lưới, Penalty
    }
}
