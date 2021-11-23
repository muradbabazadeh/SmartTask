using SmartTask.Domain.AggregatesModel.UserAggregate;
using System;
namespace SmartTask.Domain
{
    public class Editable<TUser> : Auditable<TUser> where TUser : User
    {
        public int? UpdatedById { get; protected set; }

        public DateTime? LastUpdateDateTime { get; protected set; }

        public TUser UpdatedBy { get; protected set; }

        public void SetEditFields(int? updatedById)
        {
            UpdatedById = updatedById;
            LastUpdateDateTime = DateTime.Now;
        }
    }
}
