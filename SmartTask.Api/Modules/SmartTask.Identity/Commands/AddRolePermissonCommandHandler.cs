using MediatR;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Database;
using SmartTask.Infrastructure.Idempotency;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.Identity.Commands
{
    public class AddRolePermissonCommandHandler : IRequestHandler<AddRolePermissonCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserQueries _userQueries;
        private readonly SmartTaskDbContext _context;


        public AddRolePermissonCommandHandler(IRoleRepository roleRepository, IUserQueries userQueries, SmartTaskDbContext context)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(AddRolePermissonCommand request, CancellationToken cancellationToken)
        {
            var role = await _userQueries.GetRoleAsyncById(request.RoleId);

            _context.RolePermissons.RemoveRange(_context.RolePermissons.Where(p => p.RoleId == role.Id));

            foreach (var p in request.Permissons)
            {
                role.AddPermission(p);
            }

            _roleRepository.UpdateAsync(role);
            await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public class AddPermissonIdentifiedCommandHandler : IdentifiedCommandHandler<AddRolePermissonCommand, bool>
        {
            public AddPermissonIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
            {
            }

            protected override bool CreateResultForDuplicateRequest()
            {
                return true; // Ignore duplicate requests
            }
        }
    }
}
