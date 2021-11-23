using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Domain.AggregatesModel.PermissionAggregate.PermissionParametrValues
{
    public enum DoctorViewScope
    {
        All = 0,
        Region = 1,
        TreatmentArea = 2,
        Users = 3
    }
}
