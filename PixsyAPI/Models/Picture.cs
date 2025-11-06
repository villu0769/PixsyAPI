namespace PixsyAPI.Models
{
	public class Picture
	{
		public int PictureID { get; set; }
		public int UserID { get; set; }
		public List<int> TagsIds { get; set; } = new List<int>();
	}
}
