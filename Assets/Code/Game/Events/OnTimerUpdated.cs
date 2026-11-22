public struct OnTimerUpdated : IEvent
{
    public int value;

    public void Assign(params object[] parameters)
    {
        value = (int)parameters[0];
    }

    public void Reset()
    {
        value = 0;
    }
}
