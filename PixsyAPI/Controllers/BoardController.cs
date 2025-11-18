using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixsyAPI.Data;
using PixsyAPI.Models;
using static PixsyAPI.DTOs.BoardDTO;
using AppContext = PixsyAPI.Data.AppContext;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BoardController : ControllerBase
	{
		private readonly AppContext _context;

		public BoardController(AppContext context)
		{
			_context = context;
		}

		// POST: api/boards/user/{userId}
		// Create a new board for a user
		[HttpPost("user/{userId}")]
		public async Task<IActionResult>  CreateBoard(int userId, [FromBody] BoardCreateDto dto)
		{
			var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
			if (user == null)
				return NotFound("User not found.");

			// Map DTO → Entity
			var board = new Board
			{
				Name = dto.Name,
				Description = dto.Description,
				UserID = userId
			};

			_context.Boards.Add(board);
			await _context.SaveChangesAsync();

			// Track user's boards
			user.BoardsIds.Add(board.BoardID);
			_context.SaveChanges();

			return Ok(board);
		}



		// GET: api/boards/{boardId}
		[HttpGet("{boardId}")]
		public ActionResult<Board> GetBoardById(int boardId)
		{
			var board = _context.Boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound();

			return Ok(board);
		}

		// POST: api/boards/{boardId}/pictures/{pictureId}
		// Add a picture to a board
		[HttpPost("{boardId}/pictures/{pictureId}")]
		public ActionResult AddPictureToBoard(int boardId, int pictureId)
		{
			var board = _context.Boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound("Board not found.");

			var picture = _context.Pictures.FirstOrDefault(p => p.PictureID == pictureId);
			if (picture == null) return NotFound("Picture not found.");

			if (!board.PictureIds.Contains(pictureId))
				board.PictureIds.Add(pictureId);

			return NoContent();
		}

		// GET: api/boards/{boardId}/pictures
		// Get all pictures in a board
		[HttpGet("{boardId}/pictures")]
		public ActionResult<List<Picture>> GetPicturesForBoard(int boardId)
		{
			var board = _context.Boards.FirstOrDefault(b => b.BoardID == boardId);
			if (board == null) return NotFound();

			var boardPictures = _context.Pictures.Where(p => board.PictureIds.Contains(p.PictureID)).ToList();
			return Ok(boardPictures);
		}

		// GET: api/boards/user/{userId}
		// Get all boards for a user
		[HttpGet("user/{userId}")]
		public ActionResult<List<Board>> GetBoardsForUser(int userId)
		{
			var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
			if (user == null) return NotFound();

			var userBoards = _context.Boards.Where(b => b.UserID == userId).ToList();
			return Ok(userBoards);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBoard(int id)
		{
			var board = _context.Boards.FirstOrDefault(u => u.BoardID == id);
			if (board == null) return NotFound();

			_context.Boards.Remove(board);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
