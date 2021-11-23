using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;


namespace SmartTask.Core.Api.Infrastructure.ErrorHandling
{
    public class ExceptionProfile : Profile
    {
        public ExceptionProfile()
        {
            CreateMap<ValidationException, DeveloperException>();

            CreateMap<Exception, DeveloperException>();
        }
    }
}
