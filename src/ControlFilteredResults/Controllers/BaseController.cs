using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlFilteredResults.Data;
using ControlFilteredResults.Models;
using ControlFilteredResults.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace ControlFilteredResults.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        private readonly FileListContext _context;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly ICompositeViewEngine _viewEngine;

        private ITempDataDictionary _tempData;

        public BaseController(
            FileListContext context,
            ITempDataDictionaryFactory tempDataFactory,
            ICompositeViewEngine viewEngine)
        {
            _context = context;
            _tempDataFactory = tempDataFactory;
            _viewEngine = viewEngine;
        }

        public ITempDataDictionary TempData
        {
            get
            {
                if (_tempData == null)
                {
                    _tempData = _tempDataFactory.GetTempData(HttpContext);
                }

                return _tempData;
            }
        }

        [ViewDataDictionary]
        public ViewDataDictionary ViewData { get; set; }

        [AllowAnonymous]
        // GET: Base
        public async Task<IActionResult> Index()
        {
            var model = await _context.FileList.ToListAsync();
            var viewData = new ViewDataDictionary<List<FileList>>(ViewData, model);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // GET: Base/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // GET: Base/Create
        [TypeFilter(typeof(DisableClientValidationFilter))]
        public IActionResult Create()
        {
            return new ViewResult
            {
                TempData = TempData,
                ViewData = ViewData,
                ViewEngine = _viewEngine,
            };
        }

        // POST: Base/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RandomName]
        [TypeFilter(typeof(HandleInvalidOperationFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] FileList fileList)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals("Joe", fileList.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Joe has been barred");
                }

                _context.Add(fileList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // GET: Base/Edit/5
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

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // POST: Base/Edit/5
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

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // GET: Base/Delete/5
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

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
            };
        }

        // POST: Base/Delete/5
        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileList = await _context.FileList.SingleOrDefaultAsync(m => m.Id == id);
            _context.FileList.Remove(fileList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool FileListExists(int id)
        {
            return _context.FileList.Any(e => e.Id == id);
        }
    }
}
