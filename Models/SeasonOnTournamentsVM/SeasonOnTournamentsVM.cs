using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vsports.Models.SeasonOnTournamentsVM
{
    public class SeasonOnTournamentsVM:BaseEntity
    {
        public int Id { get; set; }
        public int TournamentsId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string AvatarImage { get; set; }
        public string BackgroudImage { get; set; }
        public string Address { get; set; }
        public string CompetitionForm { get; set; }
        public string SeasonRules { get; set; }
        public string Name { get; set; }

        public IFormFile? BackgroudFile { get; set; }
        public IFormFile? AvatarFile { get; set; }

        public Tournaments? Tournaments { get; set; }
        public List<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }
        public List<SeasonOnTournaments> SeasonOnTournamentsList { get; set; }
        public Tournaments TournamentsMain { get; set; }
        public SeasonOnTournaments SeasonOnTournamentsMain { get; set; }

        public static implicit operator SeasonOnTournamentsVM(SeasonOnTournaments item)
        {
            return new SeasonOnTournamentsVM
            {
                Id = item.Id,
                TournamentsId = item.TournamentsId,
                Start = item.Start,
                End = item.End,
                AvatarImage = item.AvatarImage,
                BackgroudImage = item.BackgroudImage,
                Address = item.Address,
                CompetitionForm = item.CompetitionForm,
                SeasonRules = item.SeasonRules,
                Name = item.Name,
            };
        }
        public static implicit operator SeasonOnTournaments(SeasonOnTournamentsVM item)
        {
            return new SeasonOnTournaments
            {
                Id = item.Id,
                TournamentsId = item.TournamentsId,
                Start = item.Start,
                End = item.End,
                AvatarImage = item.AvatarImage,
                BackgroudImage = item.BackgroudImage,
                Address = item.Address,
                CompetitionForm = item.CompetitionForm,
                SeasonRules = item.SeasonRules,
                Name = item.Name,
            };
        }

    }
}
