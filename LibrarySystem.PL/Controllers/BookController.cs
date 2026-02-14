using LibrarySystem.Application.Features.Book.Create;
using LibrarySystem.Application.Features.Book.Delete;
using LibrarySystem.Application.Features.Book.GetAllBooks;
using LibrarySystem.Application.Features.Book.GetOneBook;
using LibrarySystem.Application.Features.Book.Search;
using LibrarySystem.Application.Features.Book.Update;
using LibrarySystem.PL.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Controllers;

[Authorize(Roles = "Admin")]
public class BookController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetAllBooksCommand());

        if(result.IsSuccess)
        {
            var books = result.Value.Select(b => new Models.Book
            {
                Id = b.Id,
                Title = b.Title,
                ImageURL = b.ImageURL,

            }).ToList();

            return View(books);
        }

        ViewData["Error"] = $"{result.Error}";

        return View();
    }
    [AllowAnonymous]
    public ActionResult Search() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Search(string searchType,string searchInput)
    {
        if(searchInput is null)
        {
            ViewData["EmptySearchInput"] = "Enter some words to find!";
            return View();
        }
        try
        {
            string searchText = searchInput.Trim();

            switch(searchType)
            {
                case "title":

                    var titleResult = await _mediator.Send(new SearchBookCommand("title",searchText));

                    if(titleResult is null || titleResult.Count == 0)
                    {
                        ViewData["NotFound"] = "There is no books with this title";
                        return View();
                    }

                    return View(titleResult.Select(x => new Book { Author = x.Author,ImageURL = x.ImageURL }));

                case "author":

                    var authorResult = await _mediator.Send(new SearchBookCommand("author",searchText));

                    if(authorResult is null || authorResult.Count == 0)
                    {
                        ViewData["NotFound"] = "No books here for This author";
                        return View();
                    }

                    return View(authorResult.Select(x => new Book { Author = x.Author,ImageURL = x.ImageURL }));

                case "isdn":

                    var isdnResult = await _mediator.Send(new SearchBookCommand("isdn",searchText));

                    if(isdnResult is null || isdnResult.Count == 0)
                    {
                        ViewData["NotFound"] = "Wrong ISDN number";
                        return View();
                    }

                    return View(isdnResult.Select(x => new Book { Author = x.Author,ImageURL = x.ImageURL }));

                default:

                    return View();
            }
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }

    //[AllowAnonymous]
    //public async Task<IActionResult> Details([Required] int id)
    //{
    //    ClaimsPrincipal claimUser = HttpContext.User;

    //    try
    //    {
    //        var book = await _mediator.Send(new GetOneBookCommand(id));

    //        if(book is null)
    //            return NotFound();

    //        bool isAvailable = await _borrowingService.IsAvailable(id);

    //        BorrowingViewModel model = new BorrowingViewModel
    //        {
    //            IsAvilable = isAvailable,
    //            Title = book.Title,
    //            Author = book.Author,
    //            Description = book.Description,
    //            ISBN = book.ISBN,
    //            BookId = id,
    //            ImageURL = book.ImageURL,
    //        };


    //        if(!isAvailable && claimUser.Identity.IsAuthenticated)
    //        {
    //            var user = await _mediator.Send(new GetUserCommand(User.Identity.Name));

    //            bool borrowedByMe = await _borrowingService.IsBorrowedByMe(user.Id,id);

    //            if(borrowedByMe)
    //            {
    //                model.IsBorrowedByMe = true;
    //            }
    //        }

    //        return View(model);
    //    }

    //    catch(Exception)
    //    {
    //        ViewData["Error"] = "Something went wrong!";
    //        return RedirectToAction(nameof(Index));
    //    }
    //}


    public IActionResult Create() => View();
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Book book)
    {
        try
        {

            var request = new CreateBookRequest(book.Title,book.Author,book.ISBN,book.Description,book.File);

            //if(bookIsdnCheck is not null)
            //{
            //    ViewData["Error"] = "Sorry this ISDN is exist!";
            //    return View();
            //}

            await _mediator.Send(new CreateBookCommand(request));

            return RedirectToAction(nameof(Index));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }

    }


    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetOneBookCommand(id));


            if(result is null)
            {
                return NotFound();
            }

            var book = new Models.Book
            {
                Id = result.Id,
                Title = result.Title,
                Author = result.Author,
                Description = result.Description,
                ISBN = result.ISBN,
                ImageURL = result.ImageUrl,
            };

            return View(book);
        }


        catch(Exception)
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
            var request = new UpdateBookRequest(book.Id,book.Title,book.Author,book.ISBN,book.Description,book.File);

            await _mediator.Send(new UpdateBookCommand(request));

            return RedirectToAction(nameof(Index));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return RedirectToAction(nameof(Index));
        }

    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetOneBookCommand(id));


            if(result is null)
            {
                return NotFound();
            }

            var book = new Models.Book
            {
                Id = result.Id,
                Title = result.Title,
                Author = result.Author,
                Description = result.Description,
                ISBN = result.ISBN,
                ImageURL = result.ImageUrl,
            };

            return View(book);
        }
        catch(Exception)
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
            await _mediator.Send(new DeleteBookCommand(id));

            return RedirectToAction(nameof(Index));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return RedirectToAction(nameof(Index));
        }
    }
}