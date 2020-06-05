using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Book Book { get; set; }

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Book = new Book();
            if(id==null)
            {
                //Create
                return View(Book);
            }

            //Try to find book in db
            Book = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);

            if(Book == null)
            {
                return NotFound();
            }

            //Update
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    //Create
                    await _db.Books.AddAsync(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //Update
            return View(Book);
        }

        #region API Calls
        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting book" });
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted successfully" });
        }


        #endregion
    }
}