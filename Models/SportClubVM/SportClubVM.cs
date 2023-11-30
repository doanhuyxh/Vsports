namespace vsports.Models.SportClubVM
{
	public class SportClubVM:BaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int SportId { get; set; }
		public string OwnerId { get; set; }
		public string SportsCoach { get; set; } // huấn luyên viên
		public string AvatarImage { get; set; }
		public string BackgroudImage { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Description { get; set; }
		public string ClubRules { get; set; }
		public string Status { get; set; } //public or private
		public int Point { get; set; } //điểm dễ xếp loại nổi bật
		
		public string? OwnerName { get; set; }
		public string? SportName { get; set; }
		public IFormFile? AvatarFile { get; set; }
		public IFormFile? BackgroudFile { get; set; }

		public List<ClubMember>? clubMembers { get; set; }

		public static implicit operator SportClubVM(SportClub item)
		{
			return new SportClubVM
			{
				Id= item.Id,
				Name= item.Name,
				SportId= item.SportId,
				OwnerId= item.OwnerId,
				SportsCoach= item.SportsCoach,
				AvatarImage= item.AvatarImage,
				BackgroudImage = item.BackgroudImage,
				Address= item.Address,
				PhoneNumber= item.PhoneNumber,
				Email= item.Email,
				Description= item.Description,
				ClubRules= item.ClubRules,
				Status= item.Status,
				Point= item.Point,
				IsDelete= item.IsDelete,
				Created = item.Created,
			};
		}
		public static implicit operator SportClub(SportClubVM item)
		{
			return new SportClub
			{
				Id= item.Id,
				SportId= item.SportId,
				Name = item.Name,
				OwnerId= item.OwnerId,
				SportsCoach= item.SportsCoach,
				AvatarImage= item.AvatarImage,
				BackgroudImage = item.BackgroudImage,
				Address= item.Address,
				PhoneNumber= item.PhoneNumber,
				Email= item.Email,
				Description= item.Description,
				ClubRules= item.ClubRules,
				Status= item.Status,
				Point= item.Point,
				IsDelete= item.IsDelete,
				Created = item.Created,
			};
		}
	}
}
