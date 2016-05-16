using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControlFilteredResults.Other
{
    public class HandleInvalidOperationFilter : IAsyncExceptionFilter
    {
        private readonly HtmlEncoder _htmlEncoder;

        public HandleInvalidOperationFilter(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is InvalidOperationException && context.Result == null)
            {
                context.Result = new ContentResult
                {
                    Content = "<html><body><h3>" + _htmlEncoder.Encode(context.Exception.Message) + "</h3></body></html>",
                    ContentType = "text/html",
                    StatusCode = StatusCodes.Status200OK,
                };

                context.Exception = null;
            }

            return Task.CompletedTask;
        }
    }
}
