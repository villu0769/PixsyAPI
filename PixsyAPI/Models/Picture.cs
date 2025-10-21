namespace PixsyAPI.Models
{
	public class Picture
	{
		public int PictureID { get; set; }
		public int UserID { get; set; }
		public List<string> Tags { get; set; } = new List<string>();
	}
}
