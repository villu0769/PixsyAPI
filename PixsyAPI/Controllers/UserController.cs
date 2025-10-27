using Microsoft.AspNetCore.Mvc;
using PixsyAPI.DTOs;
using PixsyAPI.Models;
using PixsyAPI.Service;
using System.Collections.Generic;
using System.Linq;
using static PixsyAPI.DTOs.UserDTO;

namespace PixsyAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private static readonly List<User> users = new();
		private readonly PasswordService passwordService = new();

		[HttpGet("{id}")]
		public ActionResult<UserReadDto> GetUserById(int id)
		{
			var user = users.FirstOrDefault(u => u.UserID == id);
			if (user == null) return NotFound();

			return Ok(new UserReadDto
			{
				UserID = user.UserID,
				UserName = user.UserName,
				DisplayName = user.DisplayName,
				Email = user.Email,
				Role = user.Role
			});
		}

		[HttpPost("register")]
		public ActionResult<UserReadDto> Register(UserCreateDto dto)
		{
			if (users.Any(u => u.UserName == dto.UserName))
				return BadRequest("Username already exists.");

			var user = new User
			{
				UserID = users.Count + 1,
				UserName = dto.UserName,
				DisplayName = dto.DisplayName,
				Email = dto.Email
			};

			var hashed = passwordService.HashPassword(dto.Password);
			user.PasswordHash = Convert.FromBase64String(hashed);

			users.Add(user);

			return Ok(new UserReadDto
			{
				UserID = user.UserID,
				UserName = user.UserName,
				DisplayName = user.DisplayName,
				Email = user.Email,
				Role = user.Role
			});
		}


		[HttpPost("login")]
		public ActionResult<string> Login(UserLoginDto dto)
		{
			var user = users.FirstOrDefault(u => u.UserName == dto.UserName);
			if (user == null) return Unauthorized("Invalid username or password.");

			var storedHash = Convert.ToBase64String(user.PasswordHash);
			if (!passwordService.VerifyPassword(dto.Password, storedHash))
				return Unauthorized("Invalid username or password.");

			// Normally you'd issue a JWT token here
			return Ok($"Welcome, {user.DisplayName}!");
		}

		// DELETE: api/users/{id}
		[HttpDelete("{id}")]
		public ActionResult DeleteUser(int id)
		{
			var user = users.FirstOrDefault(u => u.UserID == id);
			if (user == null) return NotFound();

			users.Remove(user);
			return NoContent();
		}

	}
}
