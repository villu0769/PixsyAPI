using Microsoft.AspNetCore.Mvc;
using PixsyAPI.Models;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BoardController : ControllerBase
	{
		private static readonly List<User> users = new();
		private static readonly List<Board> boards = new();
		private static readonly List<Picture> pictures = new();

		// POST: api/boards/user/{userId}
		// Create a new board for a user
		[HttpPost("user/{userId}")]
		public ActionResult<Board> CreateBoard(int userId, [FromBody] Board board)
		{
			var user = users.FirstOrDefault(u => u.UserID == userId);
			if (user == null) return NotFound("User not found.");

			board.BoardID = boards.Count + 1;
			board.UserID = userId;

			boards.Add(board);

			// Track user's boards
			user.BoardsIds.Add(board.BoardID);

			return Ok(board);  // Changed from CreatedAtAction to Ok
		}


		// GET: api/boards/{boardId}
		[HttpGet("{boardId}")]
		public ActionResult<Board> GetBoardById(int boardId)
		{
			var board = boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound();

			return Ok(board);
		}

		// POST: api/boards/{boardId}/pictures/{pictureId}
		// Add a picture to a board
		[HttpPost("{boardId}/pictures/{pictureId}")]
		public ActionResult AddPictureToBoard(int boardId, int pictureId)
		{
			var board = boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound("Board not found.");

			var picture = pictures.FirstOrDefault(p => p.PictureID == pictureId);
			if (picture == null) return NotFound("Picture not found.");

			if (!board.PictureIDS.Contains(pictureId))
				board.PictureIDS.Add(pictureId);

			return NoContent();
		}

		// GET: api/boards/{boardId}/pictures
		// Get all pictures in a board
		[HttpGet("{boardId}/pictures")]
		public ActionResult<List<Picture>> GetPicturesForBoard(int boardId)
		{
			var board = boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound();

			var boardPictures = pictures.Where(p => board.PictureIDS.Contains(p.PictureID)).ToList();
			return Ok(pictures);
		}

		// GET: api/boards/user/{userId}
		// Get all boards for a user
		[HttpGet("user/{userId}")]
		public ActionResult<List<Board>> GetBoardsForUser(int userId)
		{
			var user = users.FirstOrDefault(u => u.UserID == userId);
			if (user == null) return NotFound();

			var userBoards = boards.Where(b => b.UserID == userId).ToList();
			return Ok(boards);
		}
	}
}
