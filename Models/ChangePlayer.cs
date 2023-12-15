using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vsports.Models
{
    public class ChangePlayer:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int MatchScheduleAndResultId { get; set; }
        [ForeignKey("MatchScheduleAndResultsId")]
        public MatchScheduleAndResults matchScheduleAndResults { get; set; }
        public int SportClubId { get; set; }
        public string time {  get; set; }
        public string playerIn {  get; set; }
        public string playerOut { get; set; }

    }
}
