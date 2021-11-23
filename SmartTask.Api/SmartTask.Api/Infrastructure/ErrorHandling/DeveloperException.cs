using FluentValidation.Results;
using System.Collections.Generic;
namespace SmartTask.Core.Api.Infrastructure.ErrorHandling
{
    public class DeveloperException
    {
        public string Message { get; set; }

        public DeveloperException InnerException { get; set; }

        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}
