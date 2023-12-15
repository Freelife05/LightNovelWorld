using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LightNovelSite.Data;
using LightNovelSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;

namespace LightNovelSite.Controllers
{
    public class NovelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NovelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Novels
        public async Task<IActionResult> Index()
        {
            return _context.Novels != null ?
                        View(await _context.Novels.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Novels'  is null.");
        }
        //Novels/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Novels != null ?
                        View(await _context.Novels.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Novels'  is null.");
        }
        //Novels/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchTitle)
        {
            return _context.Novels != null ?
                        View("Index", await _context.Novels.Where(j => j.Title.Contains(SearchTitle)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Novels'  is null.");
        }

        // GET: Novels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novels = await _context.Novels
                .FirstOrDefaultAsync(m => m.Title == id);
            if (novels == null)
            {
                return NotFound();
            }

            return View(novels);
        }

        public async Task<IActionResult> AddChapter(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novels = await _context.Novels
                .FirstOrDefaultAsync(m => m.Title == id);
            Chapter ch = new Chapter();
            ch.NovelTitle = novels.Title;
            ch.ChapterNumber = novels.Chapters;
            novels.Chapters++;
            _context.SaveChanges();
            if (novels == null)
            {
                return NotFound();
            }

            return View(ch);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChapter([Bind("NovelTitle,ChapterTitle,ChapterCount,Content,ChapterNumber")] Chapter chapter)

        {
            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chapter);
        }

        public async Task<IActionResult> Read(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }
            var novel = await _context.Novels.FindAsync(id);
            var chapters = _context.Chapter.First(item => item.NovelTitle == id && item.ChapterNumber == novel.CurrentChapter);
            if (chapters == null)
            {
                return NotFound();
            }
            return View(chapters);
        }

        public async Task<IActionResult> Next(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }
            var novel = await _context.Novels.FindAsync(id);
            if (novel.CurrentChapter == novel.Chapters)
            {
                return View(Index);
            }
            else
            {
                var chapters = _context.Chapter.First(item => item.NovelTitle == id && item.ChapterNumber == novel.CurrentChapter + 1);
                novel.CurrentChapter++;
                await _context.SaveChangesAsync();
                if (chapters == null)
                {
                    return NotFound();
                }
                return View("Read", chapters);
            }
        }

        public async Task<IActionResult> Previous(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }
            var novel = await _context.Novels.FindAsync(id);
            if (novel.CurrentChapter == 0)
            {
                return View("Index");
            }
            else
            {
                var chapters = _context.Chapter.First(item => item.NovelTitle == id && item.ChapterNumber == novel.CurrentChapter - 1);
                novel.CurrentChapter--;
                await _context.SaveChangesAsync();
                if (chapters == null)
                {
                    return NotFound();
                }
                return View("Read", chapters);
            }
        }

        // GET: Novels/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Novels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Chapters")] Novels novels)

        {
            if (ModelState.IsValid)
            {
                novels.CurrentChapter = 0;
                _context.Add(novels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(novels);
        }

        // GET: Novels/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novels = await _context.Novels.FindAsync(id);
            if (novels == null)
            {
                return NotFound();
            }
            return View(novels);
        }

        // POST: Novels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Title,Chapters")] Novels novels)
        {
            if (id != novels.Title)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(novels);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovelsExists(novels.Title))
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
            return View(novels);
        }

        // GET: Novels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novels = await _context.Novels
                .FirstOrDefaultAsync(m => m.Title == id);
            if (novels == null)
            {
                return NotFound();
            }

            return View(novels);
        }

        // POST: Novels/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Novels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Novels'  is null.");
            }
            var novels = await _context.Novels.FindAsync(id);
            if (novels != null)
            {
                _context.Novels.Remove(novels);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NovelsExists(string id)
        {
            return (_context.Novels?.Any(e => e.Title == id)).GetValueOrDefault();
        }
    }
}
