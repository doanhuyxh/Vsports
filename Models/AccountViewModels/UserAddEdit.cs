namespace vsports.Models.AccountViewModels
{
	public class UserAddEdit
	{
		public string Id { get; set; }
		public string avatarImage { get; set; } = string.Empty;
		public string backgroudImage { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public string Address { get; set; } = string.Empty;
		public IFormFile? avatarFile { get; set; }
		public IFormFile? backgroudFile { get; set; }

		public static implicit operator ApplicationUser (UserAddEdit item)
		{
			return new ApplicationUser
			{
				FullName = item.FullName,
				Email = item.Email,
				PhoneNumber = item.PhoneNumber,
				Address = item.Address,
				IsActive = true
			};
		}
		public static implicit operator UserAddEdit(ApplicationUser item)
		{
			return new UserAddEdit
			{
				FullName = item.FullName,
				Email = item.Email,
				PhoneNumber = item.PhoneNumber,
				Address = item.Address,
				IsActive = item.IsActive
			};
		}
	}
}
