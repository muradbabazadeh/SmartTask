using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTask.Identity.ViewModels
{
    public class PermissionDTO
    {
        public string Name { get; set; }

        public List<PermissionParameterDTO> Parameters { get; set; }

        public List<UserPermissionParametrDTO> ParameterValues { get; set; }

        public T GetParameterValue<T>(string permissionParameterName)
        {
            var scopeParameter = ParameterValues?.FirstOrDefault();

            string paramValue = scopeParameter.Value; 
            paramValue = paramValue ?? scopeParameter.Value;
            var type = typeof(T);
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, paramValue);
            }
            else
            {
                return (T)Convert.ChangeType(paramValue, typeof(T));
            }
        }
    }
}
