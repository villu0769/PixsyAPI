using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixsyAPI.Models;
using PixsyAPI.Data;
using AppContext = PixsyAPI.Data.AppContext;
using static PixsyAPI.DTOs.PictureDTO;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PictureController : ControllerBase
	{
		private readonly AppContext _context;

		public PictureController(AppContext context)
		{
			_context = context;
		}

		// POST: api/pictures/user/{userId}
		[HttpPost("user/{userId}")]
		public async Task<ActionResult<Picture>> UploadPicture(int userId, [FromBody] UploadPictureDto dto)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
			if (user == null)
				return NotFound("User not found.");

			var picture = new Picture
			{
				UserID = userId,
				TagsIds = dto.Tags ?? new List<int>()
			};

			_context.Pictures.Add(picture);
			await _context.SaveChangesAsync();

			user.UploadsIds.Add(picture.PictureID);
			await _context.SaveChangesAsync();

			return Ok(picture);
		}

		// GET: api/pictures/{pictureId}
		[HttpGet("{pictureId}")]
		public async Task<ActionResult<Picture>> GetPictureById(int pictureId)
		{
			var picture = await _context.Pictures.FindAsync(pictureId);

			if (picture == null)
				return NotFound();

			return Ok(picture);
		}

		// GET: api/pictures/user/{userId}
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<List<Picture>>> GetPicturesByUser(int userId)
		{
			var exists = await _context.Users.AnyAsync(u => u.UserID == userId);

			if (!exists)
				return NotFound("User not found.");

			var pictures = await _context.Pictures
				.Where(p => p.UserID == userId)
				.ToListAsync();

			return Ok(pictures);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePicture(int id)
		{
			var picture = _context.Pictures.FirstOrDefault(u => u.PictureID == id);
			if (picture == null) return NotFound();

			_context.Pictures.Remove(picture);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
