using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using vsports.Models;

namespace vsports.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ClubMember> ClubMember { get; set; }
        public DbSet<SportClub> SportClub { get; set; }
        public DbSet<Friendships> Friendships { get; set; }
        public DbSet<MatchScheduleAndResults> MatchScheduleAndResults { get; set; }
        public DbSet<Round> Round { get; set; }
        public DbSet<SeasonOnTournaments> SeasonOnTournaments { get; set; }
        public DbSet<Sport> Sport { get; set; }
        public DbSet<SportClubOnTournaments> SportClubOnTournaments { get; set; }
        public DbSet<Tournaments> Tournaments { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendships>()
                        .HasOne(f => f.UserSend)
                        .WithMany(f => f.Friendships)
                        .HasForeignKey(f => f.UserSendId)
                        .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Friendships>()
            //            .HasOne(f => f.Receiver)
            //            .WithMany(f => f.Friendships)
            //            .HasForeignKey(f => f.ReceiverId)
            //            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SportClub>()
                        .HasOne(s => s.Owner)
                        .WithMany(u => u.SportClubs)
                        .HasForeignKey(sc => sc.OwnerId);
            
            modelBuilder.Entity<SportClub>()
                        .HasOne(s => s.Sport)
                        .WithMany(u => u.SportClubs)
                        .HasForeignKey(sc => sc.SportId);

            modelBuilder.Entity<ClubMember>()
                        .HasOne(s => s.SportClub)
                        .WithMany(u => u.ClubMembers)
                        .HasForeignKey(sc => sc.SportClubId)
                        .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<ClubMember>()
                        .HasOne(s => s.User)
                        .WithMany(u => u.ClubMembers)
                        .HasForeignKey(sc => sc.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tournaments>()
                        .HasOne(s => s.Sport)
                        .WithMany(u => u.Tournaments)
                        .HasForeignKey(sc => sc.SportId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tournaments>()
                        .HasOne(tournament => tournament.Organizer)
                        .WithMany(user => user.Tournaments)
                        .HasForeignKey(tournament => tournament.UserId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SportClubOnTournaments>()
                        .HasOne(s => s.Tournaments)
                        .WithMany(u => u.SportClubOnTournaments)
                        .HasForeignKey(sc => sc.TournamentsId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SportClubOnTournaments>()
                        .HasOne(s => s.SportClub)
                        .WithMany(u => u.SportClubOnTournaments)
                        .HasForeignKey(sc => sc.SportClubId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SeasonOnTournaments>()
                        .HasOne(s => s.Tournaments)
                        .WithMany(u => u.SeasonOnTournaments)
                        .HasForeignKey(sc => sc.TournamentsId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchScheduleAndResults>()
                        .HasOne(s => s.Round)
                        .WithMany(u => u.MatchScheduleAndResults)
                        .HasForeignKey(sc => sc.RoundId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchScheduleAndResults>()
                        .HasOne(s => s.SeasonOnTournaments)
                        .WithMany(u => u.MatchScheduleAndResults)
                        .HasForeignKey(sc => sc.SeasonOnTournamentId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchScheduleAndResults>()
                        .HasOne(f => f.SportClub1)
                        .WithMany()
                        .HasForeignKey(f => f.SportClubId_1)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchScheduleAndResults>()
                        .HasOne(f => f.SportClub2)
                        .WithMany()
                        .HasForeignKey(f => f.SportClubId_2)
                        .OnDelete(DeleteBehavior.NoAction);

        }
        #endregion
    }
}
