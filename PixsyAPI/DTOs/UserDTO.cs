using PixsyAPI.Models;

namespace PixsyAPI.DTOs
{
	public class UserDTO
	{
		public class UserCreateDto
		{
			public string UserName { get; set; } = string.Empty;
			public string DisplayName { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}

		public class UserLoginDto
		{
			public string UserName { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
		public class UserReadDto
		{
			public int UserID { get; set; }
			public string UserName { get; set; } = string.Empty;
			public string DisplayName { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
			public UserRole Role { get; set; }
		}
	}
}
