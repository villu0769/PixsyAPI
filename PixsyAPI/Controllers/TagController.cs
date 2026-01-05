using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixsyAPI.Models;
using PixsyAPI.DTOs;
using AppContext = PixsyAPI.Data.AppContext;
using static PixsyAPI.DTOs.TagDTO;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TagController : ControllerBase
	{
		private readonly AppContext _context;

		public TagController(AppContext context)
		{
			_context = context;
		}

		// POST: api/Tag
		[HttpPost]
		public async Task<ActionResult<Tag>> CreateTag([FromBody] CreateTagDto dto)
		{
			if (dto == null)
				return BadRequest("Invalid tag data.");

			var tag = new Tag
			{
				Name = dto.Name
			};

			_context.Tags.Add(tag);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTagById), new { tagId = tag.TagID }, tag);
		}

		// GET: api/Tag
		[HttpGet]
		public async Task<ActionResult<List<Tag>>> GetAllTags()
		{
			var tags = await _context.Tags.ToListAsync();
			return Ok(tags);
		}

		// GET: api/Tag/{tagId}
		[HttpGet("{tagId}")]
		public async Task<ActionResult<Tag>> GetTagById(int tagId)
		{
			var tag = await _context.Tags.FindAsync(tagId);
			if (tag == null)
				return NotFound("Tag not found.");

			return Ok(tag);
		}

		// DELETE: api/Tag/{tagId}
		[HttpDelete("{tagId}")]
		public async Task<IActionResult> DeleteTag(int tagId)
		{
			var tag = await _context.Tags.FindAsync(tagId);
			if (tag == null)
			{
				return NotFound("Tag not found.");
			}

			foreach(var pic in _context.Pictures.Where(x=>x.TagsIds.Contains(tagId)))
			{ 
				pic.TagsIds.Remove(tagId); 
				await _context.SaveChangesAsync();
			}

			_context.Tags.Remove(tag);
			await _context.SaveChangesAsync();
			
			return NoContent();
		}
	}
}

