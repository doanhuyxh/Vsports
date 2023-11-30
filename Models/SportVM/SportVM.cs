using System.ComponentModel.DataAnnotations;

namespace vsports.Models.SportVM
{
	public class SportVM : BaseEntity
	{
		public int Id { get; set; }
		[Display(Name = "Tên môn thể thao")]
		public string Name { get; set; }

		public static implicit operator SportVM(Sport item)
		{
			return new SportVM
			{
				Id = item.Id,
				Name = item.Name,
				Created = item.Created,
				IsDelete = item.IsDelete,
			};
		}
		public static implicit operator Sport(SportVM item)
		{
			return new Sport
			{
				Id = item.Id,
				Name = item.Name,
				Created = item.Created,
				IsDelete = item.IsDelete,
			};
		}

	}
}
