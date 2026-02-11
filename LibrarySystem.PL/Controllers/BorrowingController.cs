using LibrarySystem.Application.Features.Book.GetOneBook;
using LibrarySystem.Application.Features.Borrowing.Create;
using LibrarySystem.Application.Features.Borrowing.Delete;
using LibrarySystem.Application.Features.User.GetUser;
using LibrarySystem.PL.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.PL.Controllers;

[Authorize]
public class BorrowingController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    //public async Task<IActionResult> MyBooks()
    //{
    //    try
    //    {
    //        var user = await _mediator.Send(new GetUserCommand(User.Identity.Name));

    //        IEnumerable<Borrowing> userBooks = await _borrowingService.SelectAllUserBorrowingsAsync(user.Id);

    //        if(userBooks is null || userBooks.Count() == 0)
    //        {
    //            ViewData["NoBooks"] = "You shoud borrow some books";
    //            return View();
    //        }

    //        List<Book> userBooksDetails = new List<Book>();

    //        foreach(var item in userBooks)
    //        {
    //            var book = await _mediator.Send(new GetOneBookCommand(item.BookId));

    //            userBooksDetails.Add(book);
    //        }
    //        return View(userBooksDetails);

    //    }
    //    catch(Exception)
    //    {
    //        ViewData["Error"] = "Something went wrong!";
    //        return View();
    //    }

    //}

    [HttpGet]
    public async Task<IActionResult> IsAvailable(int bookId)
    {
        try
        {


            //bool isAvailable = await _borrowingService.IsAvailable(bookId);

            var book = await _mediator.Send(new GetOneBookCommand(bookId));


            Book model = new Book
            {
                IsAvilable = false,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN,
                Id = book.Id
            };

            return View(model);
        }
        catch(Exception)
        {
            return RedirectToAction("Index","Book");
        }
    }



    public async Task<IActionResult> BorrowBook([Required]int id)
    {
        try
        {
            var book = await _mediator.Send(new GetOneBookCommand(id));

            return View(book);
        }
        catch(Exception)
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
            var user = await _mediator.Send(new GetUserCommand(User.Identity.Name));

            await _mediator.Send(new InsertBorrowingCommand(user.Id,id));

            return RedirectToAction(nameof(Success));
        }
        catch(Exception)
        {
            return RedirectToAction("Index","Book");
        }

    }

    public IActionResult Success() => View();

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var book = await _mediator.Send(new GetOneBookCommand(id));

            return View(book);
        }
        catch(Exception)
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
            var user = await _mediator.Send(new GetUserCommand(User.Identity.Name));

            await _mediator.Send(new DeleteBorrowingCommand(user.Id,id));

            return View(); // RedirectToAction(nameof(MyBooks));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }
}
