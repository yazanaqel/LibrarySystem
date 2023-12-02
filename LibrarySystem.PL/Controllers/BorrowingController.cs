using LibrarySystem.BLL.Services.BookService;
using LibrarySystem.BLL.Services.BorrowingService;
using LibrarySystem.BLL.Services.UserService;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Controllers;

[Authorize]
public class BorrowingController : Controller
{
	private readonly IBorrowingService _borrowingService;
	private readonly IUserService _userService;
	private readonly IBookService _bookService;

	public BorrowingController(IBorrowingService borrowingService, IUserService userService, IBookService bookService)
	{
		_borrowingService = borrowingService;
		_userService = userService;
		_bookService = bookService;
	}
	public async Task<IActionResult> MyBooks()
	{
		try
		{
			var user = await _userService.SelectUserByEmail(User.Identity.Name);

			IEnumerable<Borrowing> userBooks = await _borrowingService.SelectAllUserBorrowingsAsync(user.Id);

			if(userBooks is null || userBooks.Count() == 0)
			{
				ViewData["NoBooks"] = "You shoud borrow some books";
				return View();
			}

			List<Book> userBooksDetails = new List<Book>();

			foreach (var item in userBooks)
			{
				Book book = await _bookService.SelectBookAsync(item.BookId);

				userBooksDetails.Add(book);
			}
			return View(userBooksDetails);

		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}

	}

	[HttpGet]
	public async Task<IActionResult> IsAvailable(int bookId)
	{
		try
		{


			bool isAvailable = await _borrowingService.IsAvailable(bookId);
			var book = await _bookService.SelectBookAsync(bookId);


			BorrowingViewModel model = new BorrowingViewModel
			{
				IsAvilable = isAvailable,
				Title = book.Title,
				Author = book.Author,
				Description = book.Description,
				ISBN = book.ISBN,
				BookId = bookId
			};

			return View(model);
		}
		catch (Exception)
		{
			return RedirectToAction("Index", "Book");
		}
	}



    public async Task<IActionResult> BorrowBook(int id)
    {
        try
        {
            var book = await _bookService.SelectBookAsync(id);

            return View(book);
        }
        catch (Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }

    [HttpPost]
    [ActionName("BorrowBook")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BorrowBookConfirmed(int id)
	{
		try
		{
			var user = await _userService.SelectUserByEmail(User.Identity.Name);

			Borrowing borrowing = new Borrowing
			{
				UserId = user.Id,
				BookId = id
			};

			await _borrowingService.InsertBorrowingAsync(borrowing);

			return RedirectToAction(nameof(Success));
		}
		catch (Exception)
		{
			return RedirectToAction("Index", "Book");
		}

	}

	public IActionResult Success() => View();

	public async Task<IActionResult> Delete(int id)
	{
		try
		{
            var book = await _bookService.SelectBookAsync(id);

            return View(book);
        }
		catch (Exception)
		{
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
	}

	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		try
		{
			var user = await _userService.SelectUserByEmail(User.Identity.Name);

			await _borrowingService.DeleteBorrowingAsync(user.Id, id);

			return RedirectToAction(nameof(MyBooks));
		}
		catch (Exception)
		{
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
	}
}
