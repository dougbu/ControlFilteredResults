using System;
using ControlFilteredResults.Controllers;
using ControlFilteredResults.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControlFilteredResults.Other
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RandomNameAttribute : Attribute, IActionFilter, IOrderedFilter
    {
        private static readonly Random _random = new Random();
        private static readonly object _lock = new object();

        public int Order { get; } = -10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Generate a random name if Create submission left out required Name value.
            if (string.Equals(context.ActionDescriptor.Name, nameof(BaseController.Create), StringComparison.Ordinal) &&
                !context.ModelState.IsValid &&
                context.ModelState.Remove(nameof(FileList.Name)))
            {
                foreach (var argument in context.ActionArguments)
                {
                    var fileList = argument.Value as FileList;
                    if (fileList != null)
                    {
                        int randy;
                        lock (_lock)
                        {
                            randy = _random.Next(int.MaxValue);
                        }

                        fileList.Name = "George_" + randy;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // no-op
        }
    }
}
