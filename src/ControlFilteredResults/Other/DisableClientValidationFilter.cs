using System;
using System.Threading.Tasks;
using ControlFilteredResults.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;

namespace ControlFilteredResults.Other
{
    public class DisableClientValidationFilter : IAsyncResultFilter
    {
        private readonly MvcViewOptions _options;

        public DisableClientValidationFilter(IOptions<MvcViewOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            object action;
            context.RouteData.Values.TryGetValue("action", out action);
            if (string.Equals(action as string, nameof(BaseController.Create), StringComparison.Ordinal) &&
                (context.Result is ViewResult || context.Result is ViewViewComponentResult))
            {
                var savedValidation = _options.HtmlHelperOptions.ClientValidationEnabled;
                _options.HtmlHelperOptions.ClientValidationEnabled = false;

                try
                {
                    return next();
                }
                finally
                {
                    _options.HtmlHelperOptions.ClientValidationEnabled = savedValidation;
                }
            }

            return next();
        }
    }
}
