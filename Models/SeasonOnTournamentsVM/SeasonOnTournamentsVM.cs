using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vsports.Models.SeasonOnTournamentsVM
{
    public class SeasonOnTournamentsVM : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TournamentsId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string AvatarImage { get; set; }
        public string BackgroudImage { get; set; }
        public string Address { get; set; }
        public string CompetitionForm { get; set; }
        public string SeasonRules { get; set; }


        public int TeamsNumber { get; set; }
        public int MemberOfTeam { get; set; }
        public int numberOfRoundsPerMatch { get; set; }
        public int numberOfCoaches { get; set; }
        public int TimeFight { get; set; }
        public int NumberBoard { get; set; }
        public int NumberTeamOnRoud { get; set; }



        public IFormFile? BackgroudFile { get; set; }
        public IFormFile? AvatarFile { get; set; }

        public Tournaments? Tournaments { get; set; }
        public List<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }

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
                TeamsNumber = item.TeamsNumber,
                MemberOfTeam = item.MemberOfTeam,
                numberOfRoundsPerMatch = item.numberOfRoundsPerMatch,
                numberOfCoaches = item.numberOfCoaches,
                TimeFight = item.TimeFight,
                NumberBoard = item.NumberBoard,
                NumberTeamOnRoud = item.NumberTeamOnRoud,
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
                TeamsNumber = item.TeamsNumber,
                MemberOfTeam = item.MemberOfTeam,
                numberOfRoundsPerMatch = item.numberOfRoundsPerMatch,
                numberOfCoaches = item.numberOfCoaches,
                TimeFight = item.TimeFight,
                NumberBoard = item.NumberBoard,
                NumberTeamOnRoud = item.NumberTeamOnRoud,
            };
        }

    }
}
