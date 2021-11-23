using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Domain.AggregatesModel.PermissionAggregate.PermissionParametrValues
{
    public enum DoctorAddScope
    {
        All = 0,
        Region = 1,
        TreatmentArea = 2,
        Users = 3
    }
}
