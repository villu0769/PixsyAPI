using PixsyAPI.Models;

namespace PixsyAPI.DTOs
{
	public class BoardDTO
	{
		public class BoardCreateDto
		{
			public string Name { get; set; } = string.Empty;
			public string Description { get; set; } = string.Empty;
		}
		public class BoardReadDto
		{
			public int BoardID { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Description { get; set; } = string.Empty;
			public Visibility BoardVisibility { get; set; }
			public List<int> PictureIDs { get; set; } = new List<int>();
			public int UserID { get; set; }
		}

	}
}
