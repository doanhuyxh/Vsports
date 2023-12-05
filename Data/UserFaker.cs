using Bogus;
using vsports.Models;

namespace vsports.Data
{
	public class UserFaker
	{
		public static Faker<ApplicationUser> GenerateFakeUsers(int count)
		{
			var faker = new Faker<ApplicationUser>()
				.RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
				.RuleFor(u => u.UserName, (f, u) => $"user_{count}")
				.RuleFor(u => u.Email, (f, u) => u.UserName.ToLower() + "@fake.com")
				.RuleFor(u => u.NormalizedEmail, (f, o) => o.Email.ToUpper())
				.RuleFor(u => u.EmailConfirmed, f => true)
				.RuleFor(u => u.SecurityStamp, Guid.NewGuid().ToString())
				.RuleFor(u => u.ConcurrencyStamp, Guid.NewGuid().ToString())
				.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
				.RuleFor(u => u.PhoneNumberConfirmed, f => true)
				.RuleFor(u => u.TwoFactorEnabled, f => false)
				.RuleFor(u => u.LockoutEnabled, f => true)
				.RuleFor(u => u.AccessFailedCount, f => f.Random.Number(1, 5))
				.RuleFor(u => u.FullName, f => f.Name.FullName())
				.RuleFor(u => u.avatarImage, f=> "/upload/img_avatar/blank_avatar.png")
				.RuleFor(u => u.backgroudImage, f => "/upload/img_backgroud/img_bg_bank.png")
				.RuleFor(u => u.IsActive, f => f.Random.Bool())
				.RuleFor(u => u.Address, f => f.Address.StreetAddress());

			return faker;
		}
	}
}
