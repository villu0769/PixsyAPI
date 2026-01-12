using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PixsyAPI.Controllers;
using PixsyAPI.Data;
using PixsyAPI.Models;
using PixsyAPI.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppContext = PixsyAPI.Data.AppContext;

namespace PixsyAPI.Tests.Controllers
{
	[TestFixture]
	public class PictureControllerTests
	{
		private AppContext _context;
		private PictureController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new AppContext(options);
			_controller = new PictureController(_context);
		}

		// ---------------- UPLOAD PICTURE ----------------

		[Test]
		public async Task UploadPicture_ReturnsNotFound_WhenUserDoesNotExist()
		{
			var dto = new PictureDTO.UploadPictureDto
			{
				Tags = new() { 1, 2 }
			};

			var result = await _controller.UploadPicture(1, dto);

			Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
		}

		[Test]
		public async Task UploadPicture_CreatesPicture_WhenUserExists()
		{
			_context.Users.Add(new User
			{
				UserID = 1,
				UploadsIds = new()
			});
			_context.SaveChanges();

			var dto = new PictureDTO.UploadPictureDto
			{
				Tags = new() { 1, 2 }
			};

			var result = await _controller.UploadPicture(1, dto);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
			Assert.That(_context.Pictures.Count(), Is.EqualTo(1));
			Assert.That(_context.Users.First().UploadsIds.Count, Is.EqualTo(1));
		}

		// ---------------- GET PICTURE BY ID ----------------

		[Test]
		public async Task GetPictureById_ReturnsNotFound_WhenMissing()
		{
			var result = await _controller.GetPictureById(1);

			Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public async Task GetPictureById_ReturnsPicture_WhenExists()
		{
			_context.Pictures.Add(new Picture
			{
				PictureID = 1,
				UserID = 1
			});
			_context.SaveChanges();

			var result = await _controller.GetPictureById(1);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
		}

		// ---------------- GET PICTURES BY USER ----------------

		[Test]
		public async Task GetPicturesByUser_ReturnsNotFound_WhenUserMissing()
		{
			var result = await _controller.GetPicturesByUser(1);

			Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
		}

		[Test]
		public async Task GetPicturesByUser_ReturnsPictures_WhenUserExists()
		{
			_context.Users.Add(new User { UserID = 1 });
			_context.Pictures.Add(new Picture { UserID = 1 });
			_context.Pictures.Add(new Picture { UserID = 1 });
			_context.SaveChanges();

			var result = await _controller.GetPicturesByUser(1);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
		}

		// ---------------- DELETE PICTURE ----------------

		[Test]
		public async Task DeletePicture_ReturnsNotFound_WhenMissing()
		{
			var result = await _controller.DeletePicture(1);

			Assert.That(result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public async Task DeletePicture_DeletesPicture_WhenExists()
		{
			_context.Pictures.Add(new Picture
			{
				PictureID = 1,
				UserID = 1
			});
			_context.SaveChanges();

			var result = await _controller.DeletePicture(1);

			Assert.That(result, Is.TypeOf<NoContentResult>());
			Assert.That(_context.Pictures.Any(), Is.False);
		}
	}
}

