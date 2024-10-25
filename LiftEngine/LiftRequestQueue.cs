namespace LiftEngine;

public class LiftRequestQueue
{
    private readonly Queue<int> _requests = new Queue<int>();
    private readonly object _lock = new object();

    public void AddRequest(int floor)
    {
        lock (_lock)
        {
            _requests.Enqueue(floor);
        }
    }

    public int? GetNextRequest()
    {
        lock (_lock)
        {
            return _requests.Count > 0 ? _requests.Dequeue() : (int?)null;
        }
    }
}
