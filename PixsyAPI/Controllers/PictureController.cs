using Microsoft.AspNetCore.Mvc;
using PixsyAPI.Models;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PictureController : ControllerBase
	{
		private static readonly List<User> users;
		private static readonly List<Picture> pictures;

		// POST: api/pictures/user/{userId}
		// Upload a new picture for a user with tags
		[HttpPost("user/{userId}")]
		public ActionResult<Picture> UploadPicture(int userId, [FromBody] List<int> tags)
		{

			var user = users.FirstOrDefault(u => u.UserID == userId);
			if (user == null) return NotFound("User not found.");

			var picture = new Picture
			{
				PictureID = pictures.Count + 1,
				UserID = userId,
				TagsIds = tags ?? new List<int>()
			};

			pictures.Add(picture);

			// Track user's uploads
			user.UploadsIds.Add(picture.PictureID);

			return Ok(picture);  // Changed from CreatedAtAction to Ok
		}


		// GET: api/pictures/{pictureId}
		[HttpGet("{pictureId}")]
		public ActionResult<Picture> GetPictureById(int pictureId)
		{
			var picture = pictures.FirstOrDefault(p => p.PictureID == pictureId);
			if (picture == null) return NotFound();

			return Ok(picture);
		}

		// GET: api/pictures/user/{userId}
		// Get all pictures uploaded by a user
		[HttpGet("user/{userId}")]
		public ActionResult<List<Picture>> GetPicturesByUser(int userId)
		{
			var user = users.FirstOrDefault(u => u.UserID == userId);
			if (user == null) return NotFound("User not found.");

			var userPictures = pictures.Where(p => p.UserID == userId).ToList();

			return Ok(pictures);
		}
	}
}
