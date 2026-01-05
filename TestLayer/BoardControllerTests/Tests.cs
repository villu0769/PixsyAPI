using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PixsyAPI.Controllers;
using PixsyAPI.Data;
using PixsyAPI.Models;
using PixsyAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppContext = PixsyAPI.Data.AppContext;

namespace PixsyAPI.Tests.Controllers
{
	[TestFixture]
	public class BoardControllerTests
	{
		private AppContext _context;
		private BoardController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new AppContext(options);
			_controller = new BoardController(_context);
		}

		// ---------------- CREATE BOARD ----------------

		[Test]
		public async Task CreateBoard_ReturnsNotFound_WhenUserDoesNotExist()
		{
			var dto = new BoardDTO.BoardCreateDto
			{
				Name = "Test",
				Description = "Test Desc"
			};

			var result = await _controller.CreateBoard(1, dto);

			Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
		}

		[Test]
		public async Task CreateBoard_CreatesBoard_WhenUserExists()
		{
			_context.Users.Add(new User { UserID = 1 });
			_context.SaveChanges();

			var dto = new BoardDTO.BoardCreateDto
			{
				Name = "Board 1",
				Description = "Desc"
			};

			var result = await _controller.CreateBoard(1, dto);

			Assert.That(result, Is.TypeOf<OkObjectResult>());
			Assert.That(_context.Boards.Count(), Is.EqualTo(1));
		}

		// ---------------- GET BOARD ----------------

		[Test]
		public void GetBoardById_ReturnsNotFound_WhenMissing()
		{
			var result = _controller.GetBoardById(1);

			Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void GetBoardById_ReturnsBoard_WhenExists()
		{
			_context.Boards.Add(new Board { Name = "newBoard", Description="Desc"});
			_context.SaveChanges();

			var result = _controller.GetBoardById(1);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
		}

		// ---------------- ADD PICTURE ----------------

		[Test]
		public async Task AddPictureToBoard_ReturnsNotFound_WhenBoardMissing()
		{
			var result = await _controller.AddPictureToBoard(1, 1);

			Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
		}

		[Test]
		public async Task AddPictureToBoard_AddsPicture_WhenValid()
		{
			_context.Boards.Add(new Board
			{
				Name ="nov tgestg bor",
				Description ="opisanie",
				PictureIds = new List<int>()
			});

			_context.Pictures.Add(new Picture
			{
				TagsIds = new List<int>()
			});

			_context.SaveChanges();

			var result = await _controller.AddPictureToBoard(1, 1);

			Assert.That(result, Is.TypeOf<NoContentResult>());
			Assert.That(_context.Boards.First().PictureIds.Contains(1), Is.True);
		}

		// ---------------- GET PICTURES ----------------

		[Test]
		public void GetPicturesForBoard_ReturnsNotFound_WhenBoardMissing()
		{
			var result = _controller.GetPicturesForBoard(1);

			Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void GetPicturesForBoard_ReturnsPictures_WhenExists()
		{
			_context.Boards.Add(new Board
			{
				Name = "newBoard",
				Description = "Desc",
				PictureIds = new List<int> { 1 }
			});

			_context.Pictures.Add(new Picture { PictureID = 1 });

			_context.SaveChanges();

			var result = _controller.GetPicturesForBoard(1);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
		}

		// ---------------- GET USER BOARDS ----------------

		[Test]
		public void GetBoardsForUser_ReturnsNotFound_WhenUserMissing()
		{
			var result = _controller.GetBoardsForUser(1);

			Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void GetBoardsForUser_ReturnsBoards_WhenUserExists()
		{
			_context.Users.Add(new User { UserID = 1 });
			_context.Boards.Add(new Board { Name = "newBoard", Description="Desc", UserID = 1 });
			_context.SaveChanges();

			var result = _controller.GetBoardsForUser(1);

			Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
		}

		// ---------------- DELETE BOARD ----------------

		[Test]
		public async Task DeleteBoard_ReturnsNotFound_WhenMissing()
		{
			var result = await _controller.DeleteBoard(1);

			Assert.That(result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public async Task DeleteBoard_DeletesBoard_WhenExists()
		{
			_context.Boards.Add(new Board { Name = "newBoard", Description ="Desc"});
			_context.SaveChanges();

			var result = await _controller.DeleteBoard(1);

			Assert.That(result, Is.TypeOf<NoContentResult>());
			Assert.That(_context.Boards.Any(), Is.False);
		}
	}
}
