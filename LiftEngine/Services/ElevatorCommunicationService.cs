using Grpc.Core;

namespace LiftEngine.Services;

public class ElevatorCommunicationService : Elevator.ElevatorBase
{
    private readonly ILogger<ElevatorCommunicationService> _logger;
    private readonly LiftService _liftService;

    public ElevatorCommunicationService(ILogger<ElevatorCommunicationService> logger, LiftService liftService)
    {
        _logger = logger;
        _liftService = liftService;
    }

    public override Task<RequestReply> RequestLift(RequestModel request, ServerCallContext context)
    {
        var destination = request.Floor;

        _liftService.Request(destination);

        return Task.FromResult(new RequestReply());
    }
}
