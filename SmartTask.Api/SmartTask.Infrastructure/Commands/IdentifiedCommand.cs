using MediatR;
using System;
namespace SmartTask.Infrastructure.Commands
{
    public class IdentifiedCommand<T, R> : IRequest<R>
      where T : IRequest<R>
    {

        public T Command { get; }
        public Guid Key { get; }

        public IdentifiedCommand(T command, Guid key)
        {
            Command = command;
            Key = key;
        }
    }
}
