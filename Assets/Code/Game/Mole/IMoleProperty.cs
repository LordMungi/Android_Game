using UnityEngine;

public interface IMoleProperty : IResettable
{
    public bool CanKill();
    public void Tap();
    public void Shake();

}
