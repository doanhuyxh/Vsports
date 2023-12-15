using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vsports.Models.MatchScheduleAndResultsVM
{
    public class MatchScheduleAndResultsVM:BaseEntity
    {
        public int Id { get; set; }
        public int? SeasonOnTournamentId { get; set; }

        public int? RoundId { get; set; }
        public int? BoardId { get; set; }

        public DateTime Schedule { get; set; }

        public int SportClubId_1 { get; set; }
        
        public int SportClubId_2 { get; set; }      

        public string Status { get; set; } //Pending, Playing, Finished, Cancel
        public string Winner { set; get; } // SportClubId_1 or SportClubId_2
        public string SportClub2_Name { get; set; }
        public string SportClub1_Name { get; set; }


        public SeasonOnTournaments? SeasonOnTournaments { get; set; }
        public Round? Round { get; set; }
        public SportClub? SportClub2 { get; set; }
        public SportClub? SportClub1 { get; set; }


        public static implicit operator MatchScheduleAndResultsVM(MatchScheduleAndResults item)
        {
            return new MatchScheduleAndResultsVM
            {
                Id = item.Id,
                SeasonOnTournamentId = item.SeasonOnTournamentId,
                RoundId = item.RoundId,
                BoardId = item.BoardId,
                SportClubId_1 = item.SportClubId_1,
                SportClubId_2 = item.SportClubId_2,
                SportClub1_Name = item.SportClub1_Name,
                SportClub2_Name = item.SportClub2_Name,
                Status = item.Status,
                Schedule = item.Schedule,
                Winner = item.Winner,
                IsDelete = item.IsDelete,
                Created = item.Created,
            };
        }
        public static implicit operator MatchScheduleAndResults(MatchScheduleAndResultsVM item)
        {
            return new MatchScheduleAndResults
            {
                Id = item.Id,
                SeasonOnTournamentId = item.SeasonOnTournamentId,
                RoundId = item.RoundId,
                BoardId = item.BoardId,
                SportClubId_1 = item.SportClubId_1,
                SportClubId_2 = item.SportClubId_2,
                SportClub1_Name = item.SportClub1_Name,
                SportClub2_Name = item.SportClub2_Name,
                Status = item.Status,
                Schedule = item.Schedule,
                Winner = item.Winner,
                IsDelete = item.IsDelete,
                Created = item.Created,
            };
        }

    }
}
