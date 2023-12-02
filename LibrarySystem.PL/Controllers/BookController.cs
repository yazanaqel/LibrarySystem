using LibrarySystem.BLL.Services.BookService;
using LibrarySystem.BLL.Services.BorrowingService;
using LibrarySystem.BLL.Services.ImageService;
using LibrarySystem.BLL.Services.UserService;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrarySystem.PL.Controllers;

[Authorize(Roles = "Admin")]
public class BookController : Controller
{
	private readonly IBookService _bookService;
	private readonly IBorrowingService _borrowingService;
	private readonly IUserService _userService;
	private readonly IImageService _imageService;
	private readonly IWebHostEnvironment _webHost;

	public BookController(IBookService bookService, IBorrowingService borrowingService, IUserService userService, IImageService imageService, IWebHostEnvironment webHost)
	{
		_bookService = bookService;
		_borrowingService = borrowingService;
		_userService = userService;
		_imageService = imageService;
		_webHost = webHost;
	}

	[AllowAnonymous]
	public async Task<IActionResult> Index()
	{
		try
		{
			return View(await _bookService.SelectAllBooksAsync());
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}
	}
	[AllowAnonymous]
	public ActionResult Search() => View();

	[HttpPost]
	[AllowAnonymous]
	public ActionResult Search(string searchType, string searchInput)
	{
		if (searchInput is null)
		{
			ViewData["EmptySearchInput"] = "Enter some words to find!";
			return View();
		}
		try
		{
			string searchText = searchInput.Trim();

			switch (searchType)
			{
				case "title":

					var result = _bookService.SearchByTitle(searchText);

					if(result is null || result.Count == 0)
					{
						ViewData["NotFound"] = "There is no books with this title";
						return View();
					}

					return View(result);

				case "author":

					result = _bookService.SearchByAuthor(searchText);

					if (result is null || result.Count == 0)
					{
						ViewData["NotFound"] = "No books here for This author";
						return View();
					}
					return View(result);

				case "isdn":

					result = _bookService.SearchByIsbn(searchText);

					if (result is null || result.Count == 0)
					{
						ViewData["NotFound"] = "Wrong ISDN number";
						return View();
					}
					return View(result);

				default:

					return View();
			}
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}
	}

	[AllowAnonymous]
	public async Task<IActionResult> Details(int id)
	{
		ClaimsPrincipal claimUser = HttpContext.User;

		try
		{
			var book = await _bookService.SelectBookAsync(id);

			if (book is null)
				return NotFound();

			bool isAvailable = await _borrowingService.IsAvailable(id);

			BorrowingViewModel model = new BorrowingViewModel
			{
				IsAvilable = isAvailable,
				Title = book.Title,
				Author = book.Author,
				Description = book.Description,
				ISBN = book.ISBN,
				BookId = id,
				ImageURL = book.ImageURL,
			};


			if (!isAvailable && claimUser.Identity.IsAuthenticated)
			{
				var user = await _userService.SelectUserByEmail(User.Identity.Name);

				bool borrowedByMe = await _borrowingService.IsBorrowedByMe(user.Id, id);

				if (borrowedByMe)
				{
					model.IsBorrowedByMe = true;
				}
			}

			return View(model);
		}

		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return RedirectToAction(nameof(Index));
		}
	}


	public IActionResult Create() => View();
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Book book)
	{
		try
		{
			var bookIsdnCheck = await _bookService.SelectBookAsync(book.Id);


			if (bookIsdnCheck is not null)
			{
				ViewData["Error"] = "Sorry this ISDN is exist!";
				return View();
			}

			string fileName = _imageService.UpLoadFile(book.File, _webHost) ?? string.Empty;

			book.ImageURL = fileName;

			await _bookService.InsertBookAsync(book);

			return RedirectToAction(nameof(Index));
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}

	}


	public async Task<IActionResult> Edit(int id)
	{
		try
		{
			var book = await _bookService.SelectBookAsync(id);


			if (book == null)
			{
				return NotFound();
			}

			return View(book);
		}


		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return RedirectToAction(nameof(Index));
		}
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(Book book)
	{

		try
		{
			var bookCheck = await _bookService.SelectBookAsync(book.Id);

			if(book.File is not null)
			{
				string fileName = _imageService.UpLoadFile(book.File, _webHost);

				if(book.ImageURL is not null)
				{
					_imageService.Delete(book.ImageURL, _webHost);
				}

				book.ImageURL = fileName;

			}
			else
			{
				book.ImageURL = bookCheck.ImageURL;
			}

			book.ISBN = bookCheck.ISBN;

			await _bookService.UpdateBookAsync(book);

			return RedirectToAction(nameof(Index));
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return RedirectToAction(nameof(Index));
		}

	}

	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			var book = await _bookService.SelectBookAsync(id);

			if (book == null)
			{
				return NotFound();
			}

			return View(book);
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return RedirectToAction(nameof(Index));
		}
	}
	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		try
		{
			var book = await _bookService.SelectBookAsync(id);


			if (book.ImageURL is not null)
			{
				_imageService.Delete(book.ImageURL, _webHost);
			}

			await _bookService.DeleteBookAsync(id);

			return RedirectToAction(nameof(Index));
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return RedirectToAction(nameof(Index));
		}
	}
}