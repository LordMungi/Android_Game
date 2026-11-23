
public struct OnGameEnded : IEvent
{
    public int points;
    public int coins;

    public void Assign(params object[] parameters)
    {
        points = (int)parameters[0];
        coins = (int)parameters[1];
    }

    public void Reset()
    {
        points = 0;
        coins = 0;
    }
}
