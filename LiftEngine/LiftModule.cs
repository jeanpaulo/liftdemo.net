namespace LiftEngine;

public sealed class LiftModule(int minFloor, int maxFloor, int resetFloor)
{
    private readonly int _minFloor = minFloor;
    private readonly int _maxFloor = maxFloor;

    private int _currentFloor = resetFloor;
    private LiftState _liftState = LiftState.Halted;

    private readonly LiftRequestQueue _requestQueue = new LiftRequestQueue();
    private CancellationTokenSource _cts;

    private bool isMoving => _liftState == LiftState.Moving;

    public void Reset(int floor)
    {
        _currentFloor = floor;
        _liftState = LiftState.Stopped;
    }

    private async Task ProcessRequests(CancellationToken token)
    {
        _liftState = LiftState.Stopped;

        while (!token.IsCancellationRequested)
        {
            int? nextFloor = _requestQueue.GetNextRequest();

            if (nextFloor.HasValue && nextFloor != _currentFloor)
            {
                Console.WriteLine($"Processing request to floor {nextFloor.Value}...");
                await MoveTo(nextFloor.Value, token);
            } else
            {
                await Task.Delay(1000);
            }
        }

        _liftState = LiftState.Halted;
    }

    public LiftModule Start()
    {
        _cts = new CancellationTokenSource();
        Task.Run(() => ProcessRequests(_cts.Token));

        return this;
    }

    public void Stop() 
    { 
        _cts.Cancel();
        _cts = null;
    }

    public async void RequestMoveTo(int destination)
    {
        // validations
        _requestQueue.AddRequest(destination);
        Console.WriteLine($"Request to floor {destination} added.");
    }

    private async Task MoveTo(int destination, CancellationToken token)
    {
        if (_liftState == LiftState.Halted)
        {
            Console.WriteLine("Lift is not operational");
            Console.WriteLine($"Lift Status: {_liftState.ToString()}");
            return;
        }

        _liftState = LiftState.Moving;
        Console.WriteLine($"Lift going from {_currentFloor} to {destination}");
        await Task.Delay(500);
        Console.WriteLine("Lift starts to move...");
        await Task.Delay(1000); // Time for the machinery to start the movement process

        bool goingUp = destination > _currentFloor;

        while (_currentFloor != destination && !token.IsCancellationRequested)
        {
            await Task.Delay(1700);
            _currentFloor += goingUp ? 1 : -1;
            Console.WriteLine($"Floor: {_currentFloor}");
        }

        await Task.Delay(800); // Time for full-stop on the floor
        _liftState = LiftState.Stopped;
        //PrintStatus();

        if (!token.IsCancellationRequested)
        {
            _liftState = LiftState.Stopped;
            PrintStatus();
        }
    }

    public void PrintStatus()
    {
        Console.WriteLine($"-".PadRight(18, '-'));
        Console.WriteLine($"Floor: {_currentFloor}");
        Console.WriteLine($"Status: {_liftState.ToString()}");
        Console.WriteLine($"-".PadRight(18, '-'));
    }
}

public enum LiftState
{
    Halted = 0,
    Stopped = 1,
    Moving = 2
}