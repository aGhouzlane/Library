using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Library.Controllers
{
  public class AuthorController : Controller
  {
    private readonly LibraryContext _db;

    public AuthorController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Author> model = _db.Authors.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author, int bookId)
    {
      _db.Authors.Add(author);
      _db.SaveChanges();
      if (bookId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { BookId = bookId, AuthorId = author.AuthorId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisAuthor = _db.Author
          .Include(author => author.JoinEntities)
          .ThenInclude(join => join.Book)
          .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }
    public ActionResult Edit(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Name");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult Edit(Author author, int bookId)
    {
      if (bookId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { BookId = bookId, AuthorId = author.AuthorId });
      }
      _db.Entry(author).State = EntityState.Modified;
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}

