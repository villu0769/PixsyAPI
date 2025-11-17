namespace PixsyAPI.DTOs
{
	public class PictureDTO
	{
		public class UploadPictureDto
		{
			public List<int> Tags { get; set; } = new List<int>();
		}

		public class PictureReadDto
		{
			public int PictureID { get; set; }
			public List<int> Tags { get; set; } = new List<int>();
		}
	}
}
