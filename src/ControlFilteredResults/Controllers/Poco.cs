using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlFilteredResults.Data;
using ControlFilteredResults.Models;
using ControlFilteredResults.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace ControlFilteredResults.Controllers
{
    [Controller]
    public class Poco
    {
        private readonly FileListContext _context;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly ICompositeViewEngine _viewEngine;

        private ITempDataDictionary _tempData;
        private IUrlHelper _urlHelper;

        public Poco(
            FileListContext context,
            ITempDataDictionaryFactory tempDataFactory,
            IUrlHelperFactory urlHelperFactory,
            ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
            _context = context;
            _tempDataFactory = tempDataFactory;
            _urlHelperFactory = urlHelperFactory;
        }

        [ControllerContext]
        public ControllerContext Context { get; set; }

        public HttpContext HttpContext => Context.HttpContext;

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

        public IUrlHelper UrlHelper
        {
            get
            {
                if (_urlHelper == null)
                {
                    _urlHelper = _urlHelperFactory.GetUrlHelper(Context);
                }

                return _urlHelper;
            }
        }

        [ViewDataDictionary]
        public ViewDataDictionary ViewData { get; set; }

        public async Task<IActionResult> Index()
        {
            var model = await _context.FileList.ToListAsync();
            var viewData = new ViewDataDictionary<List<FileList>>(ViewData, model);

            return new ViewResult
            {
                TempData = TempData,
                ViewData = viewData,
                ViewName = "Index",
                ViewEngine = _viewEngine,
            };
        }

        public IActionResult Component(int? id)
        {
            return new ViewComponentResult
            {
                Arguments = new { Id = id },
                TempData = TempData,
                ViewData = ViewData,
                ViewComponentType = typeof(PocoComponent),
                ViewEngine = _viewEngine,
            };
        }

        public async Task<IActionResult> Direct(int? id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            var fileList = await _context.FileList
                .Where(list => list.Id == id)
                .Include(list => list.FileNames)
                .FirstOrDefaultAsync();
            if (fileList == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(fileList);
        }

        public IActionResult Indirect(int? id)
        {
            return new RedirectToActionResult(nameof(Direct), controllerName: null, routeValues: new { Id = id })
            {
                Permanent = false,
                UrlHelper = UrlHelper,
            };
        }
    }
}
