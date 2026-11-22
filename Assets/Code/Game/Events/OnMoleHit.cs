public struct OnMoleHit : IEvent
{
    public int pointsValue;

    public void Assign(params object[] parameters)
    {
        pointsValue = (int)parameters[0];
    }

    public void Reset()
    {
        pointsValue = 0; 
    }
}
