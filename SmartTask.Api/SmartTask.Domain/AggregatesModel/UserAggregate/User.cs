using SmartTask.Domain.Exceptions;
using SmartTask.SharedKernel.Domain.Seedwork;
using System;
using System.Collections.Generic;
namespace SmartTask.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {

        public string FirstName { get; private set; }

        public string LastName { get; private set; }


        public string Email { get; private set; }

        public string PasswordHash { get; private set; }

        public DateTime? LastPasswordChangeDateTime { get; private set; }

        public bool Locked { get; private set; }
        public string RefreshToken { get; private set; }
        public bool IsDeleted { get; private set; }

        private readonly List<UserRole> _roles;
        public IReadOnlyCollection<UserRole> Roles => _roles;

        private readonly List<UserPermission> _permissions;
        public IReadOnlyCollection<UserPermission> Permissions => _permissions;


        protected User()
        {
            _roles = new List<UserRole>();
            _permissions = new List<UserPermission>();
        }

        public User(string email,string passwordHash, bool locked, string firstName, string lastName) : this()
        {
            Email = email;
            PasswordHash = passwordHash;
            Locked = locked;
            LastPasswordChangeDateTime = null;
            FirstName = firstName;
            LastName = lastName;
            IsDeleted = false;
        }

        public void SetDetails(string email, string passwordHash, string firstName, string lastName)
        {
            Email = email;
            PasswordHash = passwordHash;
            LastPasswordChangeDateTime = null;
            FirstName = firstName;
            LastName = lastName;
            IsDeleted = false;
        }

        public void SetDetails( string email, string firstName, string lastName)
        {
            Email = email;
            LastPasswordChangeDateTime = null;
            FirstName = firstName;
            LastName = lastName;
            IsDeleted = false;
        }

        public void AddToRole(int roleId)
        {
            _roles.Add(new UserRole(roleId));
        }

        public void AddPermission(int permissionId)
        {
            _permissions.Add(new UserPermission(permissionId));
        }

        public void SetPasswordHash(string oldPasswordHash, string newPasswordHash)
        {
            if (PasswordHash != oldPasswordHash)
            {
                throw new DomainException("Invalid old password");
            }

            if (PasswordHash != newPasswordHash)
            {
                PasswordHash = newPasswordHash;
                LastPasswordChangeDateTime = DateTime.Now;
            }
        }

        public void ResetPassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangeDateTime = DateTime.Now;
        }

        public void LockedUser(bool locked)
        {
            Locked = locked;
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
