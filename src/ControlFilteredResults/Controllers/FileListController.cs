using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlFilteredResults.Data;
using ControlFilteredResults.Models;
using ControlFilteredResults.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ControlFilteredResults.Controllers
{
    [Authorize]
    public class FileListController : Controller
    {
        private readonly FileListContext _context;

        public FileListController(FileListContext context)
        {
            _context = context;
        }

        // GET: FileList
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.FileList.ToListAsync());
        }

        // GET: FileList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileList = await _context.FileList
                .Where(list => list.Id == id)
                .Include(list => list.FileNames)
                .FirstOrDefaultAsync();
            if (fileList == null)
            {
                return NotFound();
            }

            return View(fileList);
        }

        // GET: FileList/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: FileList/Create
        [ActionName("Create")]
        [EvenActionConstraint]
        [HttpGet]
        public IActionResult Create2()
        {
            return View("Create2");
        }

        // POST: FileList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(nameof(FileList.Name), nameof(FileList.FileNames))] FileList fileList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fileList);
        }

        // POST: FileList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ActionName("Create")]
        [Consumes("multipart/form-data")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFiles(
            [Bind(nameof(FileList.Name), nameof(FileList.Files))] FileList fileList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fileList);
        }

        // GET: FileList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileList = await _context.FileList.SingleOrDefaultAsync(m => m.Id == id);
            if (fileList == null)
            {
                return NotFound();
            }

            return View(fileList);
        }

        // POST: FileList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FileList fileList)
        {
            if (id != fileList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileListExists(fileList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View(fileList);
        }

        // GET: FileList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileList = await _context.FileList.SingleOrDefaultAsync(m => m.Id == id);
            if (fileList == null)
            {
                return NotFound();
            }

            return View(fileList);
        }

        // POST: FileList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileList = await _context.FileList.SingleOrDefaultAsync(m => m.Id == id);
            _context.FileList.Remove(fileList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                var fileList = argument as FileList;
                if (fileList != null)
                {
                    if (fileList.FileNames == null)
                    {
                        fileList.FileNames = new List<File>();
                    }
                    else
                    {
                        fileList.FileNames.RemoveAll(file => string.IsNullOrEmpty(file.Name));
                    }

                    if (fileList.Files != null)
                    {
                        foreach (var file in fileList.Files)
                        {
                            fileList.FileNames.Add(new File { Name = file.FileName });
                        }
                    }
                }
            }

            return base.OnActionExecutionAsync(context, next);
        }

        private bool FileListExists(int id)
        {
            return _context.FileList.Any(e => e.Id == id);
        }
    }
}
