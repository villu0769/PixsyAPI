namespace PixsyAPI.DTOs
{
	public class PictureDTO
	{
		public class PictureReadDto
		{
			public int PictureID { get; set; }
			public List<string> Tags { get; set; } = new List<string>();
		}
	}
}
