using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookshop.Data;
using Bookshop.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Administrator")]
public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Books
    public async Task<IActionResult> Index()
    {
        return _context.Book != null ?
                   View(await _context.Book.ToListAsync()) :
                   Problem("Entity set 'ApplicationDbContext.Book' is null.");
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Tytul,ImieAutor,NazwiskoAutor,Gatunek,Opis,RokWydania,Cena,Okladka")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,ImieAutor,NazwiskoAutor,Gatunek,Opis,RokWydania,Cena,Okladka")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Book == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Book' is null.");
        }

        var book = await _context.Book.FindAsync(id);
        if (book != null)
        {
            _context.Book.Remove(book);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
    }
   
    public async Task<IActionResult> NadajRoleAdministratora()
    {
        // Znajdź użytkownika o adresie e-mail "roman@wp.pl"
        var user = await _userManager.FindByEmailAsync("roman@wp.pl");

        if (user != null)
        {
            // Dodaj użytkownika do roli "Administrator"
            await _userManager.AddToRoleAsync(user, "Administrator");

            // Tutaj możesz wykonać dodatkowe czynności po nadaniu roli

            return RedirectToAction("~Views//books/index");
        }

        // Jeżeli użytkownik o podanym adresie e-mail nie istnieje
        return View("UzytkownikNieIstnieje");
    }
}

