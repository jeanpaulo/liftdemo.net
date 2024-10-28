namespace LiftEngine;

public class LiftService
{
    private readonly LiftModule _lift;

    public LiftService()
    {
        _lift = new LiftModule(-2, 10, 0).Start();
    }


    public void Request(int destination)
    {
        _lift.RequestMoveTo(destination);
    }

}
