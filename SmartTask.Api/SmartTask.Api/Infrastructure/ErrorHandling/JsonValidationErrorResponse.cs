﻿using System.Collections.Generic;

namespace SmartTask.Core.Api.Infrastructure.ErrorHandling
{
    public class JsonValidationErrorResponse : JsonErrorResponse
    {
        public JsonValidationErrorResponse(string errorType, string message, object developerMessage) : base(errorType,
            message, developerMessage)
        {
            Errors = new List<ValidationError>();
        }

        public List<ValidationError> Errors { get; }
    }
}
