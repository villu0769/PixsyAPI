namespace PixsyAPI.Models
{
	public class Board
	{
		public int BoardID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int UserID { get; set; }
		public List<int> PictureIds { get; set; } = new List<int>();
		public Visibility BoardVisibility { get; set; } = Visibility.Public;
	}
	public enum Visibility
	{
		Public,
		Private
	}
}
