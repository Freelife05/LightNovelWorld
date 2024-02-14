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
using Microsoft.AspNetCore.Mvc.RazorPages;


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

        private string ReplaceWordWithLink(string userInput, string wordToReplace, string replacementLink)
        {
            string[] words = userInput.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].ToLower() == wordToReplace.ToLower())
                {
                    words[i] = $"<a href='{replacementLink}' target='_blank'>{wordToReplace}</a>";
                }
            }
            return string.Join(" ", words);
        }


        //[HttpPost]
        //public ActionResult ReplaceWord(string userInput, string wordToReplace)
        //{
        //    // Check if the word exists in the database
        //    var wordEntity = ApplicationDbContext.Name.FirstOrDefault(w => w.Word.ToLower() == wordToReplace.ToLower());

        //    if (wordEntity != null)
        //    {
        //        string replacementLink = wordEntity.Link;
        //        string replacedText = ReplaceWordWithLink(userInput, wordToReplace, replacementLink);
        //        return View("Index", replacedText);
        //    }
        //    else
        //    {
        //        return View("Index", "Word not found in the database.");
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChapter([Bind("NovelTitle,ChapterTitle,ChapterCount,Content,ChapterNumber")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                NamesToLinks[] array = _context.NamesToLinks.Where(opts => opts.NovelTitle == chapter.NovelTitle).ToArray();
                foreach (var i in array)
                {
                    chapter.Content = ReplaceWordWithLink(chapter.Content, i.Word,i.Link);
                    
                }
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
            chapters.Content = "<p>" + chapters.Content  + "</p>";
            return View(chapters);
        }

        public async Task<IActionResult> Next(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novel = await _context.Novels.FindAsync(id);
            if (novel == null)
            {
                return NotFound();
            }

            if (novel.CurrentChapter == novel.Chapters)
            {
                return RedirectToAction("Index");
            }

            var chapters = _context.Chapter.FirstOrDefault(item => item.NovelTitle == id && item.ChapterNumber == novel.CurrentChapter + 1);
            if (chapters == null)
            {
                return RedirectToAction("Index");
            }

            novel.CurrentChapter++;
            await _context.SaveChangesAsync();

            return View("Read", chapters);
        }


        public async Task<IActionResult> Previous(string id)
        {
            if (id == null || _context.Novels == null)
            {
                return NotFound();
            }

            var novel = await _context.Novels.FindAsync(id);
            if (novel == null)
            {
                return NotFound();
            }

            if (novel.CurrentChapter == 0)
            {
                return RedirectToAction("Index");
            }

            var chapters = _context.Chapter.FirstOrDefault(item => item.NovelTitle == id && item.ChapterNumber == novel.CurrentChapter - 1);
            if (chapters == null)
            {

                return RedirectToAction("Index");
            }

            novel.CurrentChapter--;
            await _context.SaveChangesAsync();

            return View("Read", chapters);
        }


        // GET: Novels/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Novels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Chapters,ImageURL,Description")] Novels novels, string[] Words, string[] link)
        {
            if (ModelState.IsValid)
            {
                novels.CurrentChapter = 0;
                for (int i = 0; i < Words.Length; i++)
                {
                await _context.NamesToLinks.AddAsync(new NamesToLinks(novels.Title,Words[i], link[i]));
                }
                await _context.AddAsync(novels);
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
        public async Task<IActionResult> Edit(string id, [Bind("Title,Chapters")] Novels novels, string wordToReplace, string replacementLink, string wordToDelete)
        {
            if (id != novels.Title)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingNovel = await _context.Novels.FindAsync(id);
                    if (existingNovel == null)
                    {
                        return NotFound();
                    }



                    // Update other properties
                    existingNovel.Title = novels.Title;
                    existingNovel.Chapters = novels.Chapters;

                    _context.Update(existingNovel);
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

        private string RemoveWord(string userInput, string wordToDelete)
        {
            // Remove the specified word from the text
            string[] words = userInput.Split(' ');
            words = words.Where(word => !string.Equals(word, wordToDelete, StringComparison.OrdinalIgnoreCase)).ToArray();
            return string.Join(" ", words);
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
            var chapters = _context.Chapter.Where(Chapter => Chapter.NovelTitle == id);
            var namelinks = _context.NamesToLinks.Where(namelink =>  namelink.NovelTitle == id);
            if (novels != null)
            {
                _context.Novels.Remove(novels);
            }
            if (chapters != null)
            {
                _context.Chapter.RemoveRange(chapters);
            }
            if (namelinks != null)
            {
                _context.NamesToLinks.RemoveRange(namelinks);
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
