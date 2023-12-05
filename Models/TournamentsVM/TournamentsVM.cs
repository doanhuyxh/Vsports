using System.ComponentModel.DataAnnotations.Schema;

namespace vsports.Models.TournamentsVM
{
    public class TournamentsVM:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public int SportId { get; set; }
        public string UserId { get; set; }

        public string? SportName { get; set; }
        public string? OrganizerName { get; set; }

        public List<SeasonOnTournaments>? SeasonOnTournaments { get; set; }
        public List<SportClubOnTournaments>? SportClubOnTournaments { get; set; }

        public static implicit operator TournamentsVM(Tournaments item)
        {
            return new TournamentsVM
            {
                Id = item.Id,
                Name = item.Name,
                Point = item.Point,
                SportId = item.SportId,
                UserId = item.UserId,
                Created = item.Created,
                IsDelete = item.IsDelete,
            };
        }
        public static implicit operator Tournaments(TournamentsVM item)
        {
            return new Tournaments
            {

                Id = item.Id,
                Name = item.Name,
                Point = item.Point,
                SportId = item.SportId,
                UserId=item.UserId,
                Created = item.Created,
                IsDelete = item.IsDelete,
            };
        }
    }
}
