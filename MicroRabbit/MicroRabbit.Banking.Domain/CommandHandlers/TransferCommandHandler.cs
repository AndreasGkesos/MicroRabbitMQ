using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _bus;
        private readonly ILogger<TransferCommandHandler> _logger;

        public TransferCommandHandler(IEventBus bus, ILogger<TransferCommandHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            // publish event to RabbitMQ
            _bus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));

            _logger.LogWarning($"Banking: Command Handler {GetType().Name} Handles {request.GetType()} - {JsonConvert.SerializeObject(request)}");

            return Task.FromResult(true);
        }
    }
}
