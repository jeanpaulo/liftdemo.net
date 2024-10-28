using Grpc.Core;
using LiftEngine;

namespace LiftEngine.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly LiftService _liftService;
        public GreeterService(ILogger<GreeterService> logger, LiftService liftService)
        {
            _logger = logger;
            _liftService = liftService;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var destination = Int32.Parse(request.Name);

            _liftService.Request(destination);

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
