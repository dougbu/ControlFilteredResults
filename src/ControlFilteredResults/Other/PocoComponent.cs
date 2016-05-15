using System.Linq;
using System.Threading.Tasks;
using ControlFilteredResults.Data;
using ControlFilteredResults.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace ControlFilteredResults.Other
{
    [ViewComponent]
    public class PocoComponent
    {
        private readonly FileListContext _context;
        private readonly ICompositeViewEngine _viewEngine;

        public PocoComponent(
            FileListContext context,
            ICompositeViewEngine viewEngine)
        {
            _context = context;
            _viewEngine = viewEngine;
        }

        [ViewComponentContext]
        public ViewComponentContext Context { get; set; }

        public HttpContext HttpContext => Context.ViewContext.HttpContext;

        public ITempDataDictionary TempData => Context.ViewContext.TempData;

        public ViewDataDictionary ViewData => Context.ViewData;

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var fileList = await _context.FileList
                .Where(list => list.Id == id)
                .Include(list => list.FileNames)
                .FirstOrDefaultAsync();
            if (fileList == null)
            {
                return null;
            }

            var viewData = new ViewDataDictionary<FileList>(ViewData, fileList);

            return new ViewViewComponentResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewEngine = _viewEngine,
                ViewName = "/Views/Poco/Details.cshtml",
            };
        }
    }
}
